using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer(typeof(ShapeKeyPositionConfig))]
    public class ShapeKeyPositionConfigDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            if (property.isExpanded)
                return EditorHelper.singleLineHeight * 8;
            
            return EditorHelper.singleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PropertyDrawerHelper.LoadAttributeTooltip( this, label );
            var row0 = position.Row(0);
            var row1 = position.Row(1);
            var row2 = position.Row(2);
            var row3 = position.Row(3);
            var row4 = position.Row(4);
            var row5 = position.Row(5);

            property.isExpanded = EditorGUI.Foldout(row0, property.isExpanded, label);
            

            if (!property.isExpanded)
                return;

            var indentLevel = EditorGUI.indentLevel;
            using (IndentLevel.Do(indentLevel + 1))
            using (LabelWidth.Do(128.0f))
            {
                var driverTransformProperty = property.GetMemberProperty<ShapeKeyPositionConfig>( k => k.driverTransform);
                EditorGUI.PropertyField(row1, driverTransformProperty);
                
                var positionTypeProperty = property.GetMemberProperty<ShapeKeyPositionConfig>( k => k.positionType);
                EditorGUI.PropertyField(row2, positionTypeProperty);
                
                var startPropertyLabel = new GUIContent("Start Position");
                var endPropertyLabel = new GUIContent("End Position");
                
                switch ((ShapeKeyPositionConfig.PositionType)positionTypeProperty.enumValueIndex)
                {
                    case ShapeKeyPositionConfig.PositionType.OneAxis:
                    {
                        var axisProperty = property.GetMemberProperty<ShapeKeyPositionConfig>( k => k.positionOneAxis);
                        var positionAxisLabel = new GUIContent("Axis");
                        EditorGUI.PropertyField(row3, axisProperty, positionAxisLabel);

                        row4.Left(row4.width * 0.5f, out var leftRect).Expand(out var rightRect);
                        leftRect.Right(26.0f, out var leftButtonRect).Left(-10.0f).Expand(out var outLeftRect);
                        rightRect.Right(26.0f, out var rightButtonRect).Left(-10.0f).Expand(out var outRightRect);

                        var startValueProperty = property.GetMemberProperty<ShapeKeyPositionConfig>( k => k.oneAxisStartPosition);
                        EditorGUI.PropertyField(outLeftRect, startValueProperty, startPropertyLabel);
                        
                        if (GUI.Button(leftButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                        {
                            SetOneAxis(axisProperty, startValueProperty, driverTransformProperty);
                        }
                        
                        var endValueProperty = property.GetMemberProperty<ShapeKeyPositionConfig>( k => k.oneAxisEndPosition);
                        EditorGUI.PropertyField(outRightRect, endValueProperty, endPropertyLabel);
                        
                        if (GUI.Button(rightButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                        {
                            SetOneAxis(axisProperty, endValueProperty, driverTransformProperty);
                        }
                        
                        break;
                    }
                    case ShapeKeyPositionConfig.PositionType.TwoAxis:
                    {
                        var axisProperty = property.GetMemberProperty<ShapeKeyPositionConfig>(k => k.positionTwoAxis);
                        var positionAxisLabel = new GUIContent("Axis");
                        EditorGUI.PropertyField(row3, axisProperty, positionAxisLabel);

                        row4.Left(row4.width * 0.5f, out var leftRect).Expand(out var rightRect);
                        
                        var startValueProperty = property.GetMemberProperty<ShapeKeyPositionConfig>( k => k.twoAxisStartPosition);
                        EditorGUI.PropertyField(leftRect, startValueProperty, startPropertyLabel);
                        
                        var endValueProperty = property.GetMemberProperty<ShapeKeyPositionConfig>( k => k.twoAxisEndPosition);
                        EditorGUI.PropertyField(rightRect, endValueProperty, endPropertyLabel);
                        
                        break;
                    }
                    case ShapeKeyPositionConfig.PositionType.ThreeAxis:
                    {
                        row4.Left(row4.width * 0.5f, out var leftRect).Expand(out var rightRect);

                        var startValueProperty = property.GetMemberProperty<ShapeKeyPositionConfig>(k => k.threeAxisStartPosition);
                        EditorGUI.PropertyField(leftRect, startValueProperty, startPropertyLabel);

                        var endValueProperty = property.GetMemberProperty<ShapeKeyPositionConfig>(k => k.threeAxisEndPosition);
                        EditorGUI.PropertyField(rightRect, endValueProperty, endPropertyLabel);

                        break;
                    }
                    case ShapeKeyPositionConfig.PositionType.DistanceFromTarget:
                        var targetTransformProperty = property.GetMemberProperty<ShapeKeyPositionConfig>(k => k.targetTransform);
                        EditorGUI.PropertyField(row4, targetTransformProperty);
                        
                        break;
                }
                var interpolationCurve = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.interpolationCurve);
                var curveLabel = new GUIContent("Curve");
                
                row5.height = EditorHelper.singleLineHeight * 3;
                EditorGUI.PropertyField(row5, interpolationCurve, curveLabel);
            }
        }

        private void SetOneAxis(SerializedProperty axisProperty, SerializedProperty propertyToSet,
            SerializedProperty driverTransformProperty)
        {
            switch ((ShapeKeyPositionConfig.PositionOneAxis)axisProperty.enumValueIndex)
            {
                case ShapeKeyPositionConfig.PositionOneAxis.X:
                    propertyToSet.floatValue =
                        GetDriverPosition(driverTransformProperty.objectReferenceValue as Transform).x;
                    break;
                case ShapeKeyPositionConfig.PositionOneAxis.Y:
                    propertyToSet.floatValue =
                        GetDriverPosition(driverTransformProperty.objectReferenceValue as Transform).y;
                    break;
                case ShapeKeyPositionConfig.PositionOneAxis.Z:
                    propertyToSet.floatValue =
                        GetDriverPosition(driverTransformProperty.objectReferenceValue as Transform).z;
                    break;
            }
        }

        private Vector3 GetDriverPosition(Transform transform)
        {
            return transform.position;
        }
    }
}
