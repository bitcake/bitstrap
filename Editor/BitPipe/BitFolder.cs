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
        public List<BitFolder> childFolders = new List<BitFolder>();
        public BitFolder parentFolder;

        public BitFolder GetChildBitFolderOfName( string childFolderName )
        {
            foreach( var childFolder in childFolders )
            {
                if( childFolder.folderName == childFolderName )
                    return childFolder;
            }

            return null;
        }
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
            SetUpParentReferences( bitFolder );
            return bitFolder;
        }

        private static void SetUpParentReferences( BitFolder bitFolder )
        {
            foreach( var childFolder in bitFolder.childFolders )
            {
                childFolder.parentFolder = bitFolder;
                SetUpParentReferences( childFolder );
            }
        }
        
        public static bool GetBitPipeFolderPath( BitFolder bitFolder, string folderToGetPath,
            out string folderPath, string initialRelativePath = "Assets/" )
        {
	        folderPath = Path.Combine(initialRelativePath, bitFolder.folderName);
            if( bitFolder.folderName == folderToGetPath )
                return true;

            foreach( var childFolder in bitFolder.childFolders )
            {
                if( GetBitPipeFolderPath( childFolder, folderToGetPath, out var outFolderPath, folderPath ) )
                {
                    folderPath = outFolderPath;
                    return true;
                }
            }

            return false;
        }

        public static bool CheckFolderNameExists( BitFolder bitFolder, string folderNameToCheck )
        {
            if( bitFolder.folderName == folderNameToCheck )
                return true;

            foreach( var childFolder in bitFolder.childFolders )
            {
                if( CheckFolderNameExists( childFolder, folderNameToCheck ) )
                    return true;
            }

            return false;
        }

        public static bool CheckFolderPathExists( BitFolder bitFolder, string pathToCheck,
            string initialRelativePath = "Assets/" )
        {
	        var folderPath = Path.Combine(initialRelativePath, bitFolder.folderName);
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