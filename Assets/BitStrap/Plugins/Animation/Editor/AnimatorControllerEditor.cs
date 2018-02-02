using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace BitStrap
{
	[CustomEditor( typeof( AnimatorController ) )]
	public class AnimatorControllerEditor : Editor
	{
		private List<AnimationClip> animations = new List<AnimationClip>();
		private UnityEditorInternal.ReorderableList animationList;
		private Texture2D animationIcon;
		private GUIContent animationIconGuiContent;

		public void OnEnable()
		{
			LoadAnimationList();
			animationIcon = AssetPreview.GetMiniTypeThumbnail( typeof( AnimationClip ) );
			animationIconGuiContent = new GUIContent( animationIcon );
		}

		public override void OnInspectorGUI()
		{
			SetupAnimationList();
			animationList.DoLayoutList();
		}

		private void LoadAnimationList()
		{
			animations.Clear();

			var assets = AssetDatabase.LoadAllAssetsAtPath( AssetDatabase.GetAssetPath( target ) );

			foreach( var asset in assets.OrderBy( a => a.name ) )
			{
				AnimationClip animationAsset = asset as AnimationClip;

				if( animationAsset != null )
				{
					animations.Add( animationAsset );
				}
			}
		}

		private void SetupAnimationList()
		{
			if( animationList != null )
				return;

			animationList = new UnityEditorInternal.ReorderableList( animations, typeof( AnimationClip ), false, true, true, true );
			animationList.onChangedCallback += OnAnimationListChanged;
			animationList.onAddCallback += OnAddAnimation;
			animationList.onRemoveCallback += OnRemoveAnimation;

			animationList.drawHeaderCallback += DrawAnimationListHeader;
			animationList.drawElementCallback += DrawAnimationListElement;
		}

		private void DrawAnimationListHeader( Rect rect )
		{
			EditorGUI.LabelField( rect, "Animations" );
		}

		private void DrawAnimationListElement( Rect rect, int index, bool isActive, bool isFocused )
		{
			AnimationClip animation = animations[index];
			Undo.RecordObject( animation, "AnimatorControllerEditor" );

			EditorGUI.BeginChangeCheck();
			using( LabelWidth.Do( 24.0f ) )
			{
				animation.name = EditorGUI.DelayedTextField( rect, animationIconGuiContent, animation.name );
			}

			if( EditorGUI.EndChangeCheck() )
			{
				AssetDatabase.SaveAssets();
				LoadAnimationList();
				EditorUtility.SetDirty( animation );
			}
		}

		private void OnAddAnimation( UnityEditorInternal.ReorderableList list )
		{
			AnimationClip selectedAnimation = null;
			if( list.index >= 0 && list.index < list.list.Count )
				selectedAnimation = list.list[list.index] as AnimationClip;

			if( selectedAnimation != null )
			{
				var selectedPath = AssetDatabase.GetAssetPath( selectedAnimation );
				var newAnimation = Object.Instantiate( selectedAnimation );

				AssetDatabase.AddObjectToAsset( newAnimation, target );
				AssetDatabase.SaveAssets();
				AssetDatabase.ImportAsset( selectedPath );
			}
			else
			{
				var clip = AnimatorController.AllocateAnimatorClip( "New Clip" );
				AssetDatabase.AddObjectToAsset( clip, target );
				AssetDatabase.SaveAssets();
			}
		}

		private void OnAnimationListChanged( UnityEditorInternal.ReorderableList list )
		{
			LoadAnimationList();
		}

		private void OnRemoveAnimation( UnityEditorInternal.ReorderableList list )
		{
			AnimationClip animation = list.list[list.index] as AnimationClip;

			if( animation != null )
			{
				Object.DestroyImmediate( animation, true );
				AssetDatabase.SaveAssets();
			}
		}
	}
}
