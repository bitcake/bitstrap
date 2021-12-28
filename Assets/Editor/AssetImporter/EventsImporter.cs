using System;
using System.Globalization;
using BitStrap;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class EventsImporter : AssetPostprocessor
{
    private void OnPostprocessModel(GameObject g)
    {
	    Debug.Log(g);
	    var modelImporter = assetImporter as ModelImporter;
	    Debug.Log(modelImporter.clipAnimations);
    }

    private static void RemoveMesh(Transform child)
    {
        var meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
        var meshFilter = child.gameObject.GetComponent<MeshFilter>();
        Object.DestroyImmediate(meshRenderer);
        Object.DestroyImmediate(meshFilter);
    }
}
