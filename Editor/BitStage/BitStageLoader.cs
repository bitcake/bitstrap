using System;
using System.IO;
using System.Linq;
using BitStrap;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
class BitStageLoader
{
    static BitStageLoader()
    {
        EditorSceneManager.sceneOpened += CheckForPersistentScene;
    }

    static void CheckForPersistentScene( Scene scene, OpenSceneMode mode )
    {
        var sceneName = scene.name;
        var splitScene = scene.name.Split( "_" );
        if (!String.Equals( splitScene.Last(), "p", StringComparison.OrdinalIgnoreCase ))
            return;
        var scenePath = scene.path;
        var parentDirName = Directory.GetParent( scenePath )?.Name;
        sceneName = sceneName.Remove( sceneName.Length - 2 );
        if (sceneName != parentDirName)
            return;
        Debug.Log( $"Nome da cena é {sceneName}" );
        Debug.Log( $"Nome do diretorio é {parentDirName}" );

    }
}