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
        string[] blendShapeNames = null;
        
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
                if (blendShapeNames == null)
                {
                    var blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
                    blendShapeNames = new string[blendShapeCount];
                    for (int i = 0; i < blendShapeCount; i++)
                    {
                        blendShapeNames[i] = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i);
                    }
                }
                
                int currentIndex = Array.IndexOf(blendShapeNames, nameProperty.stringValue);

                EditorGUI.BeginChangeCheck();
                currentIndex = EditorGUI.Popup( position, label.text, currentIndex, blendShapeNames );

                if( EditorGUI.EndChangeCheck() )
                {
                    nameProperty.stringValue = blendShapeNames[currentIndex];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
