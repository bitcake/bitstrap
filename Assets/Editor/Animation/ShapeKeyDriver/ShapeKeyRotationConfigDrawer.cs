using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer(typeof(ShapeKeyRotationConfig))]
    public class ShapeKeyRotationConfigDrawer : PropertyDrawer
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

            // Draw Rotation Config
            var indentLevel = EditorGUI.indentLevel;
            using (IndentLevel.Do(indentLevel + 1))
            using (LabelWidth.Do(128.0f))
            {
                var driverTransformProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.driverTransform);
                var rotationTypeProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.rotationType);
                var interpolationCurve = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.interpolationCurve);
                
                var startRotationLabel = new GUIContent("Start Rotation");
                var endRotationLabel = new GUIContent("End Rotation");
                
                EditorGUI.PropertyField(row1, driverTransformProperty);
                EditorGUI.PropertyField(row2, rotationTypeProperty);
                
                row4.Left(row4.width * 0.5f, out var leftRect).Expand(out var rightRect);
                leftRect.Right(26.0f, out var leftButtonRect).Left(-10.0f).Expand(out var outLeftRect);
                rightRect.Right(26.0f, out var rightButtonRect).Left(-10.0f).Expand(out var outRightRect);
                
                if (rotationTypeProperty.enumValueIndex == (int)ShapeKeyRotationConfig.RotationType.SingleAxis)
                {
                    var rotationAxisProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.rotationAxis);
                    var startValueProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.singleAxisStartRotation);
                    var endValueProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.singleAxisEndRotation);
                    
                    // Draw Property
                    EditorGUI.PropertyField(row3, rotationAxisProperty);
                    
                    EditorGUI.PropertyField(outLeftRect, startValueProperty, startRotationLabel);
                    if (GUI.Button(leftButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                    {
                        switch ((ShapeKeyRotationConfig.RotationAxis)rotationAxisProperty.enumValueIndex)
                        {
                            case ShapeKeyRotationConfig.RotationAxis.X:
                                startValueProperty.floatValue = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform).x;
                                break;
                            case ShapeKeyRotationConfig.RotationAxis.Y:
                                startValueProperty.floatValue = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform).y;
                                break;
                            case ShapeKeyRotationConfig.RotationAxis.Z:
                                startValueProperty.floatValue = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform).z;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    
                    EditorGUI.PropertyField(outRightRect, endValueProperty, endRotationLabel);
                    if (GUI.Button(rightButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                    {
                        switch (rotationAxisProperty.enumValueIndex)
                        {
                            case 0:
                                endValueProperty.floatValue = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform).x;
                                break;
                            case 1:
                                endValueProperty.floatValue = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform).y;
                                break;
                            case 2:
                                endValueProperty.floatValue = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform).z;
                                break;
                        }
                    }
                }
                else
                {
                    var startRotationProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.multiAxisStartRotation);
                    var endRotationProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.multiAxisEndRotation);
                    
                    EditorGUI.PropertyField(outLeftRect, startRotationProperty, startRotationLabel);
                    if (GUI.Button(leftButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                    {
                        startRotationProperty.vector3Value = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform);
                    }
                    
                    EditorGUI.PropertyField(outRightRect, endRotationProperty, endRotationLabel);
                    if (GUI.Button(rightButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                    {
                        endRotationProperty.vector3Value = GetDriverRotation(driverTransformProperty.objectReferenceValue as Transform);
                    }
                }

                var curveLabel = new GUIContent("Curve");
                
                row5.height = EditorHelper.singleLineHeight * 3;
                EditorGUI.PropertyField(row5, interpolationCurve, curveLabel);
                
            }
        }

        private Vector3 GetDriverRotation(Transform transform)
        {
            return transform.localEulerAngles;
        }
    }
}
