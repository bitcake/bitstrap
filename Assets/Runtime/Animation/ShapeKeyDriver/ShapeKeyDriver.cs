using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BitStrap;
using UnityEngine;
using Debug = UnityEngine.Debug;


[ExecuteInEditMode]
public class ShapeKeyDriver : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private ShapeKeyRecipe shapeKeyRecipe;
    
    // Update is called once per frame
    void Update()
    {
        if (shapeKeyRecipe.shapeKeyRotationConfig.driverTransform == null &&
            shapeKeyRecipe.shapeKeyPositionConfig.driverTransform == null)
            return;
        
        switch(shapeKeyRecipe.driverType)
        {
            case ShapeKeyRecipe.DriverType.Rotation:
                SwitchRotationConfig();
                break;
            case ShapeKeyRecipe.DriverType.Position:
                SwitchPositionConfig();
                break;
        }
    }

    private void SwitchRotationConfig()
    {
        switch (shapeKeyRecipe.shapeKeyRotationConfig.rotationType)
        {
            case ShapeKeyRotationConfig.RotationType.SingleAxis:
            {
                var currentRotation = shapeKeyRecipe.shapeKeyRotationConfig.GetTargetSingleRotation();
                var percentage = Mathf.InverseLerp(shapeKeyRecipe.shapeKeyRotationConfig.singleAxisStartRotation,
                    shapeKeyRecipe.shapeKeyRotationConfig.singleAxisEndRotation, currentRotation);
                var interpolation = Mathf.Lerp(0, 100,
                    shapeKeyRecipe.shapeKeyRotationConfig.interpolationCurve.Evaluate(percentage));
                skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyRecipe.shapeKeyDefinition.index, interpolation);
                break;
            }
            case ShapeKeyRotationConfig.RotationType.MultiAxis:
            {
                var start = Quaternion.Euler(shapeKeyRecipe.shapeKeyRotationConfig.multiAxisStartRotation);
                var end = Quaternion.Euler(shapeKeyRecipe.shapeKeyRotationConfig.multiAxisEndRotation);
                var mid = Quaternion.SlerpUnclamped(start, end, 0.5f);
                // var projected = Vector3.ProjectOnPlane(shapeKeyRecipe.shapeKeyRotationConfig.driverTransform.forward, normal);
                var maxAngle = Quaternion.Angle(start, end);

                if (maxAngle < 0)
                    maxAngle += 360.0f;

                var angleFromStart =
                    Quaternion.Angle(start, shapeKeyRecipe.shapeKeyRotationConfig.driverTransform.localRotation);

                if (angleFromStart < 0)
                    angleFromStart += 360.0f;

                var angleToEnd =
                    Quaternion.Angle(shapeKeyRecipe.shapeKeyRotationConfig.driverTransform.localRotation, end);

                if (angleToEnd < 0)
                    angleToEnd += 360.0f;

                var angleToMid = Quaternion.Angle(shapeKeyRecipe.shapeKeyRotationConfig.driverTransform.localRotation,
                    mid);

                if (angleToMid < 0)
                    angleToMid += 360.0f;

                var percentage = 0.0f;
                if (angleFromStart < angleToEnd)
                    percentage = Mathf.InverseLerp(maxAngle / 2.0f, 0.0f, angleToMid) / 2.0f;
                else
                    percentage = Mathf.InverseLerp(0.0f, maxAngle, angleToMid) + 0.5f;

                // var percentage = Mathf.InverseLerp(0, maxAngle, angle);

                // Debug.Log($"Max: {maxAngle}, Angle From Start: {angleFromStart}, Angle to End: {angleToEnd}, Angle to Mid: {angleToMid}");

                // var cross = Vector3.Cross(start, projected);
                // var max = normal.magnitude;
                // var t = cross.magnitude;
                // var percentage = t / max;

                var interpolation = shapeKeyRecipe.shapeKeyRotationConfig.interpolationCurve.Evaluate(percentage) * 100;

                skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyRecipe.shapeKeyDefinition.index, interpolation);
                break;
            }
        }
    }
    private void SwitchPositionConfig()
    {
        var posConf = shapeKeyRecipe.shapeKeyPositionConfig;
        
        switch (posConf.positionType)
        {
            case ShapeKeyPositionConfig.PositionType.OneAxis:
            {
                var currentPosition = posConf.GetTargetOneAxisPosition();
                var percentage = Mathf.InverseLerp(posConf.oneAxisStartPosition,
                    posConf.oneAxisEndPosition, currentPosition);
                var interpolation = Mathf.Lerp(0, 100,
                    posConf.interpolationCurve.Evaluate(percentage));
                skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyRecipe.shapeKeyDefinition.index, interpolation);
                break;
            }
            
            case ShapeKeyPositionConfig.PositionType.TwoAxis:
            {
                var currentPosition = posConf.GetTargetTwoAxisPosition();

                var percentageX = Mathf.InverseLerp(posConf.twoAxisStartPosition.x,
                    posConf.twoAxisEndPosition.x, currentPosition.x);

                var percentageY = Mathf.InverseLerp(posConf.twoAxisStartPosition.y,
                    posConf.twoAxisEndPosition.y, currentPosition.y);

                var percentageMedian = (percentageX + percentageY) / 2;
                
                var interpolation = Mathf.Lerp(0, 100, posConf.interpolationCurve.Evaluate(percentageMedian));
                skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyRecipe.shapeKeyDefinition.index, interpolation);
                break;
            }
            
            case ShapeKeyPositionConfig.PositionType.ThreeAxis:
            {
                var currentPosition = posConf.driverTransform.position;
                
                var percentageX = Mathf.InverseLerp(posConf.threeAxisStartPosition.x,
                    posConf.threeAxisEndPosition.x, currentPosition.x);

                var percentageY = Mathf.InverseLerp(posConf.threeAxisStartPosition.y,
                    posConf.threeAxisEndPosition.y, currentPosition.y);
                
                var percentageZ = Mathf.InverseLerp(posConf.threeAxisStartPosition.z,
                    posConf.threeAxisEndPosition.z, currentPosition.z);

                var percentageMedian = (percentageX + percentageY + percentageZ) / 3;
                
                var interpolation = Mathf.Lerp(0, 100, posConf.interpolationCurve.Evaluate(percentageMedian));
                skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyRecipe.shapeKeyDefinition.index, interpolation);
                
                break;
            }
        }
    }


    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
