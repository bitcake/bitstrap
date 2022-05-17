using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( Object ), true )]
	public sealed class RequiredReferenceDrawer : PropertyDrawer
	{
		public static bool IsNull( SerializedProperty property )
		{
			return property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null;
		}

		public static void DrawFieldWarning( Rect position )
		{
			Texture warningIcon = EditorGUIUtility.FindTexture( "console.warnicon.sml" );

			Rect warningRect = position.Right( warningIcon.width );
			warningRect.x -= 20.0f;
			warningRect.height = warningIcon.height;

#if UNITY_5
			GUI.DrawTexture( warningRect, warningIcon, ScaleMode.ScaleToFit, true, 1.0f );
#else
			GUI.DrawTexture( warningRect, warningIcon, ScaleMode.ScaleToFit, true, 1.0f, Color.white, 0.0f, 0.0f );
#endif
		}

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			var referenceRequired = fieldInfo.GetAttribute<RequiredReferenceAttribute>( false );

			Object target = property.serializedObject.targetObject;
			bool isFromMonoBehaviour = target != null && target is MonoBehaviour;

			if( referenceRequired.IsSome && isFromMonoBehaviour && IsNull( property ) )
			{
				GUI.backgroundColor = Color.red;
				EditorGUI.PropertyField( position, property, label, true );
				GUI.backgroundColor = Color.white;

				DrawFieldWarning(position);
			}
			else
			{
				EditorGUI.PropertyField( position, property, label, true );
			}
		}
	}
}