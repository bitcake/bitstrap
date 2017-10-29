using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BitStrap
{
	[CanEditMultipleObjects]
	[CustomEditor( typeof( TweenShader ) )]
	public class TweenShaderEditor : Editor
	{
		private readonly object[] emptyArgs = new object[0];

		private ReorderableList shaderProperiesList;
		private Option<EditorCoroutine> playCoroutine;

		private MethodInfo initMethod;
		private MethodInfo updateMethod;
		private bool wasTweenEnabled;

		public override void OnInspectorGUI()
		{
			TryInitList();

			serializedObject.Update();

			var targetRendererProperty = serializedObject.GetMemberProperty<TweenShader>( t => t.targetRenderer );
			var durationProperty = serializedObject.GetMemberProperty<TweenShader>( t => t.duration );

			EditorGUILayout.PropertyField( targetRendererProperty );
			EditorGUILayout.PropertyField( durationProperty );

			shaderProperiesList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();

			var tweenShader = target as TweenShader;

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();

			if( GUILayout.Button( "Play Forward", EditorStyles.miniButtonLeft ) )
			{
				InitTween( tweenShader );
				tweenShader.PlayForward();
				UpdateTween( tweenShader );
			}

			if( GUILayout.Button( "Play Backward", EditorStyles.miniButtonMid ) )
			{
				InitTween( tweenShader );
				tweenShader.PlayBackward();
				UpdateTween( tweenShader );
			}

			if( GUILayout.Button( "Clear", EditorStyles.miniButtonRight ) )
			{
				InitTween( tweenShader );
				tweenShader.Clear();
				SceneView.RepaintAll();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void TryInitList()
		{
			if( shaderProperiesList != null )
				return;

			var shaderPropertiesProperty = serializedObject.GetMemberProperty<TweenShader>( t => t.shaderProperties );

			shaderProperiesList = new ReorderableList( serializedObject, shaderPropertiesProperty );
			shaderProperiesList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
			shaderProperiesList.drawElementCallback += DrawElement;
			shaderProperiesList.drawHeaderCallback += DrawHeader;
		}

		private void DrawElement( Rect rect, int index, bool isActive, bool isFocused )
		{
			SerializedProperty elementProperty = shaderProperiesList.serializedProperty.GetArrayElementAtIndex( index );
			EditorGUI.PropertyField( rect, elementProperty );
		}

		private void DrawHeader( Rect rect )
		{
			EditorGUI.LabelField( rect, shaderProperiesList.serializedProperty.displayName );
		}

		private void TryInitReflection()
		{
			if( initMethod != null && updateMethod != null )
				return;

			initMethod = typeof( TweenShader ).GetMethod( "Init", BindingFlags.Instance | BindingFlags.NonPublic );
			updateMethod = typeof( TweenShader ).GetMethod( "Update", BindingFlags.Instance | BindingFlags.NonPublic );
		}

		private void InitTween( TweenShader tweenShader )
		{
			TryInitReflection();
			initMethod.Invoke( tweenShader, emptyArgs );

			if( !tweenShader.duration.IsRunning )
				wasTweenEnabled = tweenShader.enabled;
		}

		private void UpdateTween( TweenShader tweenShader )
		{
			playCoroutine.IfSome( c => c.Stop() );

			if( tweenShader != null && !EditorApplication.isPlayingOrWillChangePlaymode )
				playCoroutine = EditorCoroutine.Start( UpdateTweenCoroutine( tweenShader ) );
		}

		private IEnumerator UpdateTweenCoroutine( TweenShader tweenShader )
		{
			while( true )
			{
				updateMethod.Invoke( tweenShader, emptyArgs );
				SceneView.RepaintAll();

				if( !tweenShader.duration.IsRunning )
					break;

				yield return null;
			}

			playCoroutine.IfSome( c => c.Stop() );
			playCoroutine = new None();

			tweenShader.enabled = wasTweenEnabled;
		}
	}
}