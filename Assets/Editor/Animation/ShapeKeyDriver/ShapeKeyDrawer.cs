using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer(typeof(ShapeKeyDefinition))]
    public class ShapeKeyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {   
            PropertyDrawerHelper.LoadAttributeTooltip( this, label );

            var behaviour = property.serializedObject.targetObject as MonoBehaviour;

            SkinnedMeshRenderer skinnedMeshRenderer = null;
            var nameProperty = property.GetMemberProperty<ShapeKeyDefinition>( p => p.name );

            if( behaviour != null )
            {
                skinnedMeshRenderer = behaviour.GetComponent<SkinnedMeshRenderer>();
            }

            if (skinnedMeshRenderer != null)
            {
                var blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
                var blendShapeNames = new List<string>();
                for (int i = 0; i < blendShapeCount; i++)
                {
                    blendShapeNames.Add(skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i));
                }
                
                var popupOptions = blendShapeNames.Select( x => new GUIContent( x ) ).ToArray();
                int currentIndex = blendShapeNames.IndexOf(nameProperty.stringValue);

                EditorGUI.BeginChangeCheck();
                currentIndex = EditorGUI.Popup( position, label, currentIndex, popupOptions );

                if( EditorGUI.EndChangeCheck() )
                {
                    nameProperty.stringValue = blendShapeNames[currentIndex];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
