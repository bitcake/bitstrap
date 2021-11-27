using System;
using System.Globalization;
using BitStrap;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class ColliderImporter : AssetPostprocessor
{
    private void OnPostprocessModel(GameObject g)
    {
        foreach (Transform child in g.transform)
        {
            String[] objectNameSplit = child.name.Split('_');
            switch (objectNameSplit[0])
            {
                case "UBX":
                    child.gameObject.AddComponent<BoxCollider>();
                    var boxMeshRenderer = child.gameObject.GetComponent<MeshRenderer>();
                    var boxMeshFilter = child.gameObject.GetComponent<MeshFilter>();
                    Object.DestroyImmediate(boxMeshRenderer);
                    Object.DestroyImmediate(boxMeshFilter);
                    break;
                
                case "UCX":
                    MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    var convexMeshRenderer = child.gameObject.GetComponent<MeshRenderer>();
                    var convexMeshFilter = child.gameObject.GetComponent<MeshFilter>();
                    Object.DestroyImmediate(convexMeshRenderer);
                    Object.DestroyImmediate(convexMeshFilter);
                    break;
                
                case "UCP":
                    child.gameObject.AddComponent<CapsuleCollider>();
                    var capsuleMeshRenderer = child.gameObject.GetComponent<MeshRenderer>();
                    var capsuleMeshFilter = child.gameObject.GetComponent<MeshFilter>();
                    Object.DestroyImmediate(capsuleMeshRenderer);
                    Object.DestroyImmediate(capsuleMeshFilter);
                    break;
                
                case "USP":
                    child.gameObject.AddComponent<SphereCollider>();
                    var sphereMeshRenderer = child.gameObject.GetComponent<MeshRenderer>();
                    var sphereMeshFilter = child.gameObject.GetComponent<MeshFilter>();
                    Object.DestroyImmediate(sphereMeshRenderer);
                    Object.DestroyImmediate(sphereMeshFilter);
                    break;
            }
        }
    }
}
