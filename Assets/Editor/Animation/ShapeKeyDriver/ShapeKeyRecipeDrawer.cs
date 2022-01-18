using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer(typeof(ShapeKeyRecipe))]

    public class ShapeKeyRecipeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            if (property.isExpanded)
                return EditorHelper.singleLineHeight * 10;
            
            return EditorHelper.singleLineHeight;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var row0 = position.Row(0);
            var row1 = position.Row(1);
            var row2 = position.Row(2);
            var row3 = position.Row(3);
            var row4 = position.Row(4);
            var row5 = position.Row(5);

            var shapeKeyProperty = property.GetMemberProperty<ShapeKeyRecipe>( k => k.shapeKeyDefinition);
            EditorGUI.PropertyField(row0, shapeKeyProperty);
            
            var driverTypeProperty = property.GetMemberProperty<ShapeKeyRecipe>( k => k.driverType);
            EditorGUI.PropertyField(row1, driverTypeProperty);

            switch ((ShapeKeyRecipe.DriverType)driverTypeProperty.enumValueIndex)
            {
                case ShapeKeyRecipe.DriverType.Rotation:
                    var shapeKeyRotationProperty = property.GetMemberProperty<ShapeKeyRecipe>( k => k.shapeKeyRotationConfig);
                    EditorGUI.PropertyField(row2, shapeKeyRotationProperty);
                    break;
                case ShapeKeyRecipe.DriverType.Position:
                    var shapeKeyPositionProperty = property.GetMemberProperty<ShapeKeyRecipe>( k => k.shapeKeyPositionConfig);
                    EditorGUI.PropertyField(row2, shapeKeyPositionProperty);
                    break;
                case ShapeKeyRecipe.DriverType.MecanimParameter:
                    break;
                case ShapeKeyRecipe.DriverType.Callback:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}

