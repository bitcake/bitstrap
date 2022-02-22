using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer(typeof(ShapeKeyDefinition))]
    public class ShapeKeyDrawer : PropertyDrawer
    {
        string[] blendShapeNames = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (FindSkinnedMeshRenderer(property) == null)
                return 0.0f;
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {   
            PropertyDrawerHelper.LoadAttributeTooltip( this, label );
            
            var nameProperty = property.GetMemberProperty<ShapeKeyDefinition>( p => p.name );

            var skinnedMeshRenderer = FindSkinnedMeshRenderer(property);
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
                
                int currentIndex = System.Array.IndexOf(blendShapeNames, nameProperty.stringValue);

                EditorGUI.BeginChangeCheck();
                currentIndex = EditorGUI.Popup( position, label.text, currentIndex, blendShapeNames );

                if( EditorGUI.EndChangeCheck() )
                {
                    nameProperty.stringValue = blendShapeNames[currentIndex];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private SkinnedMeshRenderer FindSkinnedMeshRenderer(SerializedProperty property)
        {
            var behaviour = property.serializedObject.targetObject as MonoBehaviour;
            if (behaviour != null)
            {
                if (fieldInfo.GetAttribute<SkinnedMeshRendererFieldAttribute>(false)
                    .TryGet(out var skinnedMeshRendererFieldAttribute))
                {
                    var skinnedMeshRendererProperty =
                        property.serializedObject.FindProperty(skinnedMeshRendererFieldAttribute.skinnedMeshRendererFieldName);
                    if (skinnedMeshRendererProperty != null)
                        return skinnedMeshRendererProperty.objectReferenceValue as SkinnedMeshRenderer;
                }
                else
                {
                    var skinnedMeshRendererProperty = property.serializedObject.FindProperty("skinnedMeshRenderer");
                    if (skinnedMeshRendererProperty != null)
                        return skinnedMeshRendererProperty.objectReferenceValue as SkinnedMeshRenderer;
                }
            }
            
            return null;
        }
    }
}
