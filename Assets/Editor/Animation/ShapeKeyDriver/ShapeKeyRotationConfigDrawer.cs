using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer(typeof(ShapeKeyRotationConfig))]
    public class ShapeKeyRotationConfigDrawer : PropertyDrawer
    {
        static readonly GUIContent CurveLabel = new GUIContent("Curve");
        private Vector3 driverTransformEulerAngles;

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            if (property.isExpanded)
                return EditorHelper.SingleLineHeight * 8;
            
            return EditorHelper.SingleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PropertyDrawerHelper.LoadAttributeTooltip( this, label );
            var rowFoldout = position.Row(0);
            var rowDriverTransform = position.Row(1);
            var rowRotationType = position.Row(2);
            var rowAxis = position.Row(3);
            var rowValues = position.Row(4);
            var rowCurve = position.Row(5);
            
            property.isExpanded = EditorGUI.Foldout(rowFoldout, property.isExpanded, label);

            if (!property.isExpanded)
                return;

            // Draw Rotation Config
            using (IndentLevel.Do(EditorGUI.indentLevel + 1))
            using (LabelWidth.Do(128.0f))
            {
                var driverTransformProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.driverTransform);
                var rotationTypeProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.rotationType);
                var interpolationCurve = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.interpolationCurve);
                
                var startRotationLabel = new GUIContent("Start Rotation");
                var endRotationLabel = new GUIContent("End Rotation");
                
                EditorGUI.PropertyField(rowDriverTransform, driverTransformProperty);
                EditorGUI.PropertyField(rowRotationType, rotationTypeProperty);
                
                rowValues.Left(rowValues.width * 0.5f, out var leftRect).Expand(out var rightRect);
                leftRect.Right(26.0f, out var leftButtonRect).Left(-2.0f).Expand(out var outLeftRect);
                rightRect.Right(26.0f, out var rightButtonRect).Left(-2.0f).Expand(out var outRightRect);

                if (driverTransformProperty.objectReferenceValue != null)
                    driverTransformEulerAngles = ((Transform)driverTransformProperty.objectReferenceValue).localEulerAngles;

                if (rotationTypeProperty.enumValueIndex == (int)ShapeKeyRotationConfig.RotationType.SingleAxis)
                {
                    var rotationAxisProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.rotationAxis);
                    var startValueProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.singleAxisStartRotation);
                    var endValueProperty = property.GetMemberProperty<ShapeKeyRotationConfig>( k => k.singleAxisEndRotation);
                    
                    // Draw Property
                    EditorGUI.PropertyField(rowAxis, rotationAxisProperty);
                    
                    EditorGUI.PropertyField(outLeftRect, startValueProperty, startRotationLabel);
                    if (GUI.Button(leftButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                    {
                        switch ((ShapeKeyRotationConfig.RotationAxis)rotationAxisProperty.enumValueIndex)
                        {
                            case ShapeKeyRotationConfig.RotationAxis.X:
                                startValueProperty.floatValue = driverTransformEulerAngles.x;
                                break;
                            case ShapeKeyRotationConfig.RotationAxis.Y:
                                startValueProperty.floatValue = driverTransformEulerAngles.y;
                                break;
                            case ShapeKeyRotationConfig.RotationAxis.Z:
                                startValueProperty.floatValue = driverTransformEulerAngles.z;
                                break;
                        }
                    }
                    
                    EditorGUI.PropertyField(outRightRect, endValueProperty, endRotationLabel);
                    if (GUI.Button(rightButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                    {
                        switch (rotationAxisProperty.enumValueIndex)
                        {
                            case 0:
                                endValueProperty.floatValue = driverTransformEulerAngles.x;
                                break;
                            case 1:
                                endValueProperty.floatValue = driverTransformEulerAngles.y;
                                break;
                            case 2:
                                endValueProperty.floatValue = driverTransformEulerAngles.z;
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
                        startRotationProperty.vector3Value = driverTransformEulerAngles;
                    }
                    
                    EditorGUI.PropertyField(outRightRect, endRotationProperty, endRotationLabel);
                    if (GUI.Button(rightButtonRect, EditorGUIUtility.IconContent("d_Transform Icon")))
                    {
                        endRotationProperty.vector3Value = driverTransformEulerAngles;
                    }
                }

                
                rowCurve.height = EditorHelper.SingleLineHeight * 3;
                EditorGUI.PropertyField(rowCurve, interpolationCurve, CurveLabel);
                
            }
        }
    }
}
