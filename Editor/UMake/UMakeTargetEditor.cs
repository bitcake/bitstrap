using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

namespace BitStrap
{
	[CustomEditor( typeof( UMakeTarget ) )]
	public sealed class UMakeTargetEditor : Editor
	{
		public enum BuildAction
		{
			None,
			PreActions,
			Build,
			PostActions,
			OpenFolder
		}

		private ReorderableList preBuildActionsList;
		private ReorderableList postBuildActionsList;

		public static bool CanBuild
		{
			get
			{
				return !EditorApplication.isCompiling && !EditorApplication.isPaused
					&& !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isUpdating;
			}
		}

		public static void ExecuteAction( UMakeTarget t, BuildAction action )
		{
			UMake umake;
			if( !UMake.Get().TryGet( out umake ) )
				return;

			string buildPath;
			UMakeTarget.Path targetPath;

			switch( action )
			{
			case BuildAction.PreActions:
				EditorApplication.delayCall += () => t.ExecutePreBuildActions( umake );
				break;
			case BuildAction.Build:
				buildPath = UMake.GetBuildPath();
				EditorApplication.delayCall += () => t.Build( umake, buildPath);
				break;
			case BuildAction.PostActions:
				EditorApplication.delayCall += () => t.ExecutePostBuildActions( umake );
				break;
			case BuildAction.OpenFolder:
				targetPath = t.GetTargetPath( umake.version, UMake.GetBuildPath() );
				EditorUtility.RevealInFinder( targetPath.directoryPath );
				break;
			}
		}

		public override void OnInspectorGUI()
		{
			var umakeTarget = target as UMakeTarget;
			Undo.RecordObject( umakeTarget, "UMakeTarget" );

			EditorGUI.BeginChangeCheck();

			using( BoxGroup.Do( "Build Settings" ) )
			{
				ShowBuildSettings( umakeTarget );
			}

			EditorGUILayout.Space();
			BuildAction action = ShowActions();
			EditorGUILayout.Space();

			ShowPreBuildActions( umakeTarget );
			ShowPostBuildActions( umakeTarget );

			if( EditorGUI.EndChangeCheck() )
				EditorUtility.SetDirty( umakeTarget );

			GUILayout.FlexibleSpace();

			ExecuteAction( umakeTarget, action );
		}

		private void ShowBuildSettings( UMakeTarget t )
		{
			t.buildTarget = ( BuildTarget ) EditorGUILayout.EnumPopup( "Build Target", t.buildTarget );

#if UNITY_5 || UNITY_2017_1 || UNITY_2017_2
			t.buildOptions = ( BuildOptions ) EditorGUILayout.EnumMaskField( "Build Options", t.buildOptions );
#else
			t.buildOptions = ( BuildOptions ) EditorGUILayout.EnumFlagsField( "Build Options", t.buildOptions );
#endif

			t.fileNameOverride = EditorGUILayout.TextField( "File Name Override", t.fileNameOverride );
		}

		private void ShowPreBuildActions( UMakeTarget t )
		{
			if( preBuildActionsList == null )
				preBuildActionsList = CreateBuildActionsList( "Pre Build Actions", t.preBuildActions );

			preBuildActionsList.DoLayoutList();
		}

		private void ShowPostBuildActions( UMakeTarget t )
		{
			if( postBuildActionsList == null )
				postBuildActionsList = CreateBuildActionsList( "Post Build Actions", t.postBuildActions );

			postBuildActionsList.DoLayoutList();
		}

		private ReorderableList CreateBuildActionsList( string label, List<UMakeBuildAction> buildActions )
		{
			var buildActionsList = new ReorderableList( buildActions, typeof( UMakeBuildAction ) );
			buildActionsList.drawHeaderCallback += r => EditorGUI.LabelField( r, label, EditorStyles.boldLabel );
			buildActionsList.drawElementCallback += ( r, i, active, focused ) =>
			{
				var actions = buildActions;
				actions[i] = EditorGUI.ObjectField( r, actions[i], typeof( UMakeBuildAction ), false ) as UMakeBuildAction;
			};

			buildActionsList.onAddCallback += l => l.list.Add( null );

			return buildActionsList;
		}

		private BuildAction ShowActions()
		{
			BuildAction action;

			using( Horizontal.Do() )
			{
				action = BuildAction.None;

				if( GUILayout.Button( "Open Folder", EditorStyles.miniButton ) )
					action = BuildAction.OpenFolder;

				GUILayout.FlexibleSpace();

				GUILayout.Label( "Execute", EditorStyles.centeredGreyMiniLabel );
				GUI.enabled = CanBuild;

				if( GUILayout.Button( "Pre Actions", EditorStyles.miniButtonLeft ) )
					action = BuildAction.PreActions;

				if( GUILayout.Button( "Build", EditorStyles.miniButtonMid, GUILayout.Width( 64.0f ) ) )
					action = BuildAction.Build;

				if( GUILayout.Button( "Post Actions", EditorStyles.miniButtonRight ) )
					action = BuildAction.PostActions;

				GUI.enabled = true;
			}

			return action;
		}
	}
}