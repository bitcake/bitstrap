using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer(typeof(ShapeKeyRecipe))]
    public class ShapeKeyRecipeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            return EditorHelper.SingleLineHeight * 10;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rowShapeKeyDefinition = position.Row(0);
            var rowDriverType = position.Row(1);
            var rowShapeKeyConfigs = position.Row(2);

            var shapeKeyProperty = property.GetMemberProperty<ShapeKeyRecipe>( k => k.shapeKeyDefinition);
            EditorGUI.PropertyField(rowShapeKeyDefinition, shapeKeyProperty);
            
            var driverTypeProperty = property.GetMemberProperty<ShapeKeyRecipe>( k => k.driverType);
            EditorGUI.PropertyField(rowDriverType, driverTypeProperty);

            switch ((ShapeKeyRecipe.DriverType)driverTypeProperty.enumValueIndex)
            {
                case ShapeKeyRecipe.DriverType.Rotation:
                {
                    var shapeKeyRotationProperty =
                        property.GetMemberProperty<ShapeKeyRecipe>(k => k.shapeKeyRotationConfig);
                    EditorGUI.PropertyField(rowShapeKeyConfigs, shapeKeyRotationProperty);
                    break;
                }
                case ShapeKeyRecipe.DriverType.Position:
                {
                    var shapeKeyPositionProperty =
                        property.GetMemberProperty<ShapeKeyRecipe>(k => k.shapeKeyPositionConfig);
                    EditorGUI.PropertyField(rowShapeKeyConfigs, shapeKeyPositionProperty);
                    break;
                } 
                // TODO: implement
                // case ShapeKeyRecipe.DriverType.MecanimParameter:
                //     break;
                // case ShapeKeyRecipe.DriverType.Callback:
                //     break;
            }

        }
    }
}

