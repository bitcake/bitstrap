using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ShapeKeyDriver : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private ShapeKeyRecipe shapeKeyRecipe;
    
    // Update is called once per frame
    void Update()
    {
        if (shapeKeyRecipe.shapeKeyRotationConfig.driverTransform == null)
            return;
        
        var currentRotation = shapeKeyRecipe.shapeKeyRotationConfig.driverTransform.localRotation.eulerAngles.y;
        Debug.Log(currentRotation);
        var percentage = Mathf.InverseLerp(shapeKeyRecipe.shapeKeyRotationConfig.singleAxisStartRotation, shapeKeyRecipe.shapeKeyRotationConfig.singleAxisEndRotation, currentRotation);
        var interpolation = Mathf.Lerp(0, 100, shapeKeyRecipe.shapeKeyRotationConfig.interpolationCurve.Evaluate(percentage));
        Debug.Log(interpolation);
        skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyRecipe.shapeKeyDefinition.index, interpolation);
    }
}
