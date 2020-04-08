using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomEditor( typeof( UMake ) )]
	public sealed class UMakeEditor : Editor
	{
		[MenuItem( "Window/BitStrap/Select UMake %&b" )]
		public static void Select()
		{
			UMake.Get().IfSome( umake =>
			{
				Selection.activeObject = umake;
				EditorGUIUtility.PingObject( umake );
			} );
		}

		public override void OnInspectorGUI()
		{
			using( Horizontal.Do() )
			{
				EditorGUI.BeginChangeCheck();
				string buildPath = EditorGUILayout.TextField( "Build Path", UMake.BuildPathPref );
				if( EditorGUI.EndChangeCheck() )
					UMake.BuildPathPref = buildPath;

				if( string.IsNullOrEmpty( buildPath ) )
				{
					Rect position = GUILayoutUtility.GetLastRect().Right( -EditorGUIUtility.labelWidth );
					EditorGUI.LabelField( position, "Pick folder on build.", EditorStyles.centeredGreyMiniLabel );
				}

				if( GUILayout.Button( "Change", GUILayout.Width( 64.0f ) ) )
				{
					string path = EditorUtility.OpenFolderPanel( "Build Path", UMake.BuildPathPref, "Builds" );
					if( !string.IsNullOrEmpty( path ) )
						UMake.BuildPathPref = path;
				}
			}

			base.OnInspectorGUI();
		}
	}
}