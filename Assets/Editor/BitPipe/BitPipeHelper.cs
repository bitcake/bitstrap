using System.IO;
using System.Linq;
using UnityEditor;

namespace BitStrap
{
    public static class BitPipeHelper
    {
        public static string CreateResourcesFolder( string path )
        {
            string dirPath = path;
            if( File.Exists( dirPath ) )
            {
                dirPath = Path.GetDirectoryName( dirPath );
                return CreateFolder( dirPath );
            }

            if( Directory.Exists( dirPath ) )
            {
                return CreateFolder( dirPath );
            }

            return "";
        }

        private static string CreateFolder( string dirPath )
        {
            var dirName = dirPath.Split( Path.DirectorySeparatorChar ).Last() + "_Resources";
            var resourcesFolder = Path.Combine( dirPath, dirName );
            
            if(!Directory.Exists( resourcesFolder ))
                AssetDatabase.CreateFolder( dirPath, dirName );

            return resourcesFolder;
        }
    }
}