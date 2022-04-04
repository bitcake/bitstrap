using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [System.Serializable]
    public class BitFolder
    {
        public string folderName;
        [NonSerialized] public bool isExpanded = true;
        [NonSerialized] public bool locked = true;
        [NonSerialized] public bool markedForDeletion;
        public List<BitFolder> childFolders = new();
    }

    public class BitFolderManager
    {
        public static BitFolder LoadBitFolderFromJson()
        {
            string jsonPath = "project_structure.json";
            string jsonRelativePath = "Assets/" + jsonPath;
            TextAsset jsonAsset;
            BitFolder bitFolder;

            jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( jsonRelativePath );
            if( jsonAsset == null )
            {
                File.WriteAllText( Path.Combine( Application.dataPath, jsonPath ), "{}" );
                AssetDatabase.ImportAsset( jsonRelativePath );
                jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( jsonRelativePath );
            }

            bitFolder = JsonUtility.FromJson<BitFolder>( jsonAsset.text );
            return bitFolder;
        }
        
        public static string GetBitPipeFolderPath( BitFolder bitFolder, string folderToGetPath, string initialRelativePath="Assets/" )
        {
            var folderPath = Path.Join( initialRelativePath, bitFolder.folderName );
            if( bitFolder.folderName == folderToGetPath )
                return folderPath;
        
            foreach( var childFolder in bitFolder.childFolders )
            {
                var childFolderPath = GetBitPipeFolderPath( childFolder, folderToGetPath, folderPath );
                if( childFolder.folderName == folderToGetPath )
                    return childFolderPath;
            }

            return "";
        }
        
        public static bool CheckFolderExists( BitFolder bitFolder, string folderToCheck )
        {
            if( bitFolder.folderName == folderToCheck )
            {
                Debug.Log( "ACHOOOOOOOOU" );
                return true;
            }        
            foreach( var childFolder in bitFolder.childFolders )
            {
                CheckFolderExists( childFolder, folderToCheck );
                if( childFolder.folderName == folderToCheck )
                {
                    Debug.Log( "ACHOOOOOOOOU NOS FILHO" );
                    return true;
                }
            }

            return false;
        }
    }
}