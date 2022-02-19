using System;
using UnityEngine;

[System.Serializable]
public struct ShapeKeyDefinition
{
    public string name;
    public int index;
}

[System.Serializable]
public struct ShapeKeyRotationConfig
{
    public enum RotationType
    {
        SingleAxis,
        MultiAxis
    }
    
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public Transform driverTransform;
    public RotationType rotationType;
    public RotationAxis rotationAxis;
    public float singleAxisStartRotation;
    public float singleAxisEndRotation;
    public Vector3 multiAxisStartRotation;
    public Vector3 multiAxisEndRotation;
    public AnimationCurve interpolationCurve;

    public float GetTargetSingleRotation()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X:
                return driverTransform.localEulerAngles.x;
            case RotationAxis.Y:
                return driverTransform.localEulerAngles.y;
            case RotationAxis.Z:
                return driverTransform.localEulerAngles.z;
        }

        return -1.0f;
    }
}

[System.Serializable]
public struct ShapeKeyPositionConfig
{
    public enum PositionType
    {
        OneAxis,
        TwoAxis,
        ThreeAxis,
    }
    
    public enum PositionOneAxis
    {
        X,
        Y,
        Z
    }

    public enum PositionTwoAxis
    {
        XY,
        XZ,
        YX,
        YZ,
        ZY,
        ZX
    }
    
    public Transform driverTransform;
    public Transform targetTransform;
    public PositionType positionType;
    public PositionOneAxis positionOneAxis;
    public PositionTwoAxis positionTwoAxis;
    public float oneAxisStartPosition;
    public float oneAxisEndPosition;
    public Vector2 twoAxisStartPosition;
    public Vector2 twoAxisEndPosition;
    public Vector3 threeAxisStartPosition;
    public Vector3 threeAxisEndPosition;
    public AnimationCurve interpolationCurve;

    public float GetTargetOneAxisPosition()
    {
        switch (positionOneAxis)
        {
            case PositionOneAxis.X:
                return driverTransform.position.x;
            case PositionOneAxis.Y:
                return driverTransform.position.y;
            case PositionOneAxis.Z:
                return driverTransform.position.z;
        }
        return -1.0f;
    }
    
    public Vector2 GetTargetTwoAxisPosition()
    {
        var position = driverTransform.position;
        
        switch (positionTwoAxis)
        {
            case PositionTwoAxis.XY:
                return new Vector2(position.x, position.y);
            case PositionTwoAxis.XZ:
                return new Vector2(position.x, position.z);
            case PositionTwoAxis.YX:
                return new Vector2(position.y, position.x);
            case PositionTwoAxis.YZ:
                return new Vector2(position.y, position.z);
            case PositionTwoAxis.ZY:
                return new Vector2(position.z, position.y);
            case PositionTwoAxis.ZX:
                return new Vector2(position.z, position.x);
        }
        return Vector2.zero;
    }
}

[System.Serializable]
public struct ShapeKeyRecipe
{
    public ShapeKeyDefinition shapeKeyDefinition;

    public enum DriverType
    {
        Rotation,
        Position,
        // MecanimParameter,
        // Callback,
    }

    public DriverType driverType;
    public ShapeKeyRotationConfig shapeKeyRotationConfig;
    public ShapeKeyPositionConfig shapeKeyPositionConfig;
}