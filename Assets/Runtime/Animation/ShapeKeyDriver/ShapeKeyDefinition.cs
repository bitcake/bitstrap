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
}

[System.Serializable]
public struct ShapeKeyRecipe
{
    public ShapeKeyDefinition shapeKeyDefinition;

    public enum DriverType
    {
        Rotation,
        Position,
        MecanimParameter,
        Callback,
    }

    public DriverType driverType;
    public ShapeKeyRotationConfig shapeKeyRotationConfig;
}