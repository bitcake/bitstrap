using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class ColliderImporter : AssetPostprocessor
{
	public override int GetPostprocessOrder()
	{
		return 10;
	}
    private void OnPostprocessModel(GameObject g)
    {
        foreach (Transform child in g.GetComponentsInChildren<Transform>())
        {
            String[] objectNameSplit = child.name.Split('_');
            switch (objectNameSplit[0])
            {
                case "UBX":
                    child.gameObject.AddComponent<BoxCollider>();
                    RemoveMesh(child);
                    break;
                
                case "UCX":
                    MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    RemoveMesh(child);
                    break;
                
                case "UME":
                    child.gameObject.AddComponent<MeshCollider>();
                    RemoveMesh(child);
                    break;
                
                case "UCP":
                    child.gameObject.AddComponent<CapsuleCollider>();
                    RemoveMesh(child);
                    break;
                
                case "USP":
                    child.gameObject.AddComponent<SphereCollider>();
                    RemoveMesh(child);
                    break;
            }
        }
    }

    private static void RemoveMesh(Transform child)
    {
        var meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
        var meshFilter = child.gameObject.GetComponent<MeshFilter>();
        Object.DestroyImmediate(meshRenderer);
        Object.DestroyImmediate(meshFilter);
    }
}
