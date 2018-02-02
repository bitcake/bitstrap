using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	[CustomEditor( typeof( ReferencesObjectBase ), true )]
	public sealed class ReferencesObjectEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var referencesObjectBase = target as ReferencesObjectBase;

			string referenceCountLabel = string.Format( "{0} references of <{1}>", referencesObjectBase.ReferenceCount, referencesObjectBase.ReferencedType.Name );
			EditorGUILayout.LabelField( referenceCountLabel, EditorStyles.centeredGreyMiniLabel );

			using( Horizontal.Do() )
			{
				using( LabelWidth.Do( 72.0f ) )
				{
					SerializedProperty rootFolderProperty = serializedObject.GetMemberProperty<ReferencesBase>( r => r.rootFolder );
					EditorGUILayout.PropertyField( rootFolderProperty );
				}

				if( GUILayout.Button( "Update References", EditorStyles.miniButton, GUILayout.Width( 108.0f ) ) )
				{
					referencesObjectBase.UpdateReferences();
					serializedObject.ApplyModifiedProperties();
				}
			}

			if( referencesObjectBase.ContainsNullReference )
			{
				Rect warningRect = GUILayoutUtility.GetRect( GUIContent.none, EditorStyles.label );

				GUI.Box( warningRect, GUIContent.none );
				var style = new GUIStyle( EditorStyles.centeredGreyMiniLabel );
				style.normal.textColor = Color.red;
				GUI.Label( warningRect, "Null references found! Please, update the references.", style );
			}
		}
	}
}