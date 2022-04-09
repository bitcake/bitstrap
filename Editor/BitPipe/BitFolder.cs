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
        public const string BitFolderJsonPath = "Assets/project_structure.json";

        public static BitFolder LoadBitFolderFromJson()
        {
            string jsonFileName = "project_structure.json";

            var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( BitFolderJsonPath );
            if( jsonAsset == null )
            {
                File.WriteAllText( Path.Combine( Application.dataPath, jsonFileName ), "{}" );
                AssetDatabase.ImportAsset( BitFolderJsonPath );
                jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( BitFolderJsonPath );
            }

            var bitFolder = JsonUtility.FromJson<BitFolder>( jsonAsset.text );
            return bitFolder;
        }

        public static string GetBitPipeFolderPath( BitFolder bitFolder, string folderToGetPath,
            string initialRelativePath = "Assets/" )
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

        public static bool CheckFolderNameExists( BitFolder bitFolder, string folderNameToCheck )
        {
            if( bitFolder.folderName == folderNameToCheck )
            {
                return true;
            }

            foreach( var childFolder in bitFolder.childFolders )
            {
                CheckFolderNameExists( childFolder, folderNameToCheck );
                if( childFolder.folderName == folderNameToCheck )
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckFolderPathExists( BitFolder bitFolder, string pathToCheck,
            string initialRelativePath = "Assets/" )
        {
            var folderPath = Path.Join( initialRelativePath, bitFolder.folderName );
            folderPath = folderPath.Replace( "\\", "/" );
            if( pathToCheck == folderPath )
                return true;

            foreach( var childFolder in bitFolder.childFolders )
            {
                var childFolderPath = CheckFolderPathExists( childFolder, pathToCheck, folderPath );
                if( childFolderPath )
                    return true;
            }

            return false;
        }
    }
}