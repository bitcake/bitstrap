using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Custom editor for all Transform components that adds some extra functionality.
	/// </summary>
	[CustomEditor( typeof( Transform ) )]
	[CanEditMultipleObjects]
	public class TransformEditor : Editor
	{
		private const float FIELD_WIDTH = 212.0f;

		private const float POSITION_MAX = 100000.0f;

		private const float BUTTON_WIDTH = 12.0f;

		private static GUIContent positionGUIContent = new GUIContent( LocalString( "Position" ),
			LocalString( "The local position of this Game Object relative to the parent." ) );

		private static GUIContent rotationGUIContent = new GUIContent( LocalString( "Rotation" ),
			LocalString( "The local rotation of this Game Object relative to the parent." ) );

		private static GUIContent scaleGUIContent = new GUIContent( LocalString( "Scale" ),
			LocalString( "The local scaling of this Game Object relative to the parent." ) );

		private static string positionWarningText = LocalString( "Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range." );

		private SerializedProperty positionProperty;
		private SerializedProperty rotationProperty;
		private SerializedProperty scaleProperty;
		private GUILayoutOption[] noOptions = new GUILayoutOption[0];
		private GUILayoutOption[] buttonWidthOption = new []{GUILayout.Width(BUTTON_WIDTH) };

        private static string LocalString(string text)
        {
#if UNITY_5 || UNITY_2017_1 || UNITY_2017_2
            return LocalizationDatabase.GetLocalizedString(text);
#else
			return text;
#endif
		}

		private void BeginPropertyWithReset()
		{
			EditorGUILayout.BeginHorizontal( noOptions );
		}

		private bool EndPropertyWithReset()
		{
			var rect = GUILayoutUtility.GetRect( BUTTON_WIDTH, EditorGUIUtility.singleLineHeight, buttonWidthOption );
			rect.x -= 2.0f;
			rect.y += 2.0f;

			bool reset = false;
			if( GUI.Button( rect, GUIContent.none, EditorHelper.Styles.Minus ) )
			{
				GUI.FocusControl( "" );
				reset = true;
			}

			EditorGUILayout.EndHorizontal();

			return reset;
		}

		public override void OnInspectorGUI()
		{
			if (!EditorGUIUtility.wideMode)
			{
				EditorGUIUtility.wideMode = true;
				EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - FIELD_WIDTH; // align field to right of inspector
			}
			serializedObject.Update();

			BeginPropertyWithReset();
			EditorGUILayout.PropertyField( positionProperty, positionGUIContent, noOptions);
			if( EndPropertyWithReset() )
				positionProperty.vector3Value = Vector3.zero;
			GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

			BeginPropertyWithReset();
			RotationPropertyField( rotationGUIContent );
			if( EndPropertyWithReset() )
				rotationProperty.quaternionValue = Quaternion.identity;
			GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

			BeginPropertyWithReset();
			EditorGUILayout.PropertyField( scaleProperty, scaleGUIContent, noOptions);
			if( EndPropertyWithReset() )
				scaleProperty.vector3Value = Vector3.one;
			GUILayout.Space(3f * EditorGUIUtility.standardVerticalSpacing);

			if ( !ValidatePosition( ( ( Transform ) target ).position ) )
				EditorGUILayout.HelpBox( positionWarningText, MessageType.Warning );

			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
		{
			positionProperty = serializedObject.FindProperty( "m_LocalPosition" );
			rotationProperty = serializedObject.FindProperty( "m_LocalRotation" );
			scaleProperty = serializedObject.FindProperty( "m_LocalScale" );
		}

		private bool ValidatePosition( Vector3 position )
		{
			return Mathf.Abs( position.x ) <= POSITION_MAX
				&& Mathf.Abs( position.y ) <= POSITION_MAX
				&& Mathf.Abs( position.z ) <= POSITION_MAX;
		}

		private void RotationPropertyField( GUIContent content )
		{
			Transform transform = ( Transform ) targets[0];
			Quaternion localRotation = transform.localRotation;

			foreach( Object t in targets )
			{
				if( !SameRotation( localRotation, ( ( Transform ) t ).localRotation ) )
				{
					EditorGUI.showMixedValue = true;
					break;
				}
			}

			EditorGUI.BeginChangeCheck();
			Vector3 eulerAngles = EditorGUILayout.Vector3Field( content, localRotation.eulerAngles, noOptions);
			if( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObjects( targets, "Rotation Changed" );
				foreach( Object obj in targets )
				{
					Transform t = ( Transform ) obj;
					t.localEulerAngles = eulerAngles;
				}

				rotationProperty.serializedObject.SetIsDifferentCacheDirty();
			}

			EditorGUI.showMixedValue = false;
		}

		private bool SameRotation( Quaternion rot1, Quaternion rot2 )
		{
			if( rot1.x != rot2.x )
				return false;
			if( rot1.y != rot2.y )
				return false;
			if( rot1.z != rot2.z )
				return false;
			if( rot1.w != rot2.w )
				return false;
			return true;
		}
	}
}