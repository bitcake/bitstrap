using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;

[System.Serializable]
public class BitFolder
{
    public string folderName;
    public bool isExpanded = true;
    public bool markedForDeletion;
    public List<BitFolder> childFolders = new();
}

public class FolderStructure : EditorWindow
{
    private const string jsonPath = "Editor/BitPipe/project_structure.json";
    public BitFolder bitFolder;
    private TextAsset jsonAsset;

    private SerializedObject so;

    [MenuItem( "Window/BitStrap/BitPipe" )]
    public static void OpenFolderStructure() => GetWindow<FolderStructure>();

    private void OnEnable()
    {
        so = new SerializedObject( this );
        bitFolder = new BitFolder();
        jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( "Assets/" + jsonPath );
        if( jsonAsset == null )
        {
            File.WriteAllText( Path.Combine( Application.dataPath, jsonPath ), "{}" );
            AssetDatabase.ImportAsset( "Assets/" + jsonPath );
            jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( "Assets/" + jsonPath );
        }

        bitFolder = JsonUtility.FromJson<BitFolder>( jsonAsset.text );
    }

    private void DrawBitFolder( BitFolder bitFolder )
    {
        EditorGUILayout.BeginHorizontal();
        bitFolder.isExpanded =
            EditorGUILayout.Toggle( bitFolder.isExpanded, EditorStyles.foldoutHeader, GUILayout.Width( 16 ) );
        
        // TextField for typing the name of the folders, delete any added WhiteSpaces
        bitFolder.folderName = EditorGUILayout.DelayedTextField( bitFolder.folderName ).Replace( " ", "" );
        if( GUILayout.Button( "+", GUILayout.Width( 25 ) ) )
        {
            bitFolder.childFolders.Add( new BitFolder() );
        }

        if( GUILayout.Button( "-", GUILayout.Width( 25 ) ) )
        {
            if( EditorUtility.DisplayDialog( "Are you sure?", $"Do you REALLY want to delete {bitFolder.folderName}?",
                "Yes", "No" ) )
                bitFolder.markedForDeletion = true;
        }

        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;

        if( bitFolder.isExpanded )
        {
            foreach( var folder in bitFolder.childFolders )
            {
                DrawBitFolder( folder );
            }
        }

        for( var index = bitFolder.childFolders.Count - 1; index >= 0; index-- )
        {
            var folder = bitFolder.childFolders[index];
            if( folder.markedForDeletion )
                bitFolder.childFolders.Remove( folder );
        }

        EditorGUI.indentLevel--;
    }

    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.LabelField( "BitPipe", EditorStyles.largeLabel );
        EditorGUI.BeginChangeCheck();
        DrawBitFolder( bitFolder );
        if( EditorGUI.EndChangeCheck() )
        {
            UpdateJson();
        }

        GUILayout.Space( 20 );
        if( GUILayout.Button( "Generate Folders", GUILayout.Height( 35 ) ) )
        {
            // if( EditorUtility.DisplayDialog( "Are you sure?", $"This will generate empty folders in your project.",
            //     "Yes", "No" ) )
            GenerateFolders( bitFolder, Application.dataPath );
        }

        so.ApplyModifiedProperties();
    }

    private void GenerateFolders( BitFolder folder, string parentPath )
    {
        var path = parentPath + "/" + folder.folderName;
        // Debug.Log( "Generated Path: " + path );
        if( !Directory.Exists( path ) )
            Directory.CreateDirectory( path );

        foreach( var childFolder in folder.childFolders )
        {
            GenerateFolders( childFolder, path );
        }
    }

    private void UpdateJson()
    {
        File.WriteAllText( AssetDatabase.GetAssetPath( jsonAsset ), JsonUtility.ToJson( bitFolder, true ) );
        EditorUtility.SetDirty( jsonAsset );
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset( "Assets/" + jsonPath );
    }
}