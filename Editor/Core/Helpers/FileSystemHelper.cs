using System.IO;

namespace BitStrap
{
    /// <summary>
    /// Complementary methods to the System.IO classes.
    /// </summary>
    public static class FileSystemHelper
    {
        /// <summary>
        /// Copy a directory (including its contents) to another location.
        /// </summary>
        /// <param name="fromDirectory"></param>
        /// <param name="toDirectory"></param>
        public static void CopyDirectory( string fromDirectory, string toDirectory )
        {
            Directory.CreateDirectory( toDirectory );

            fromDirectory = Path.GetFullPath( fromDirectory );
            string[] files = Directory.GetFiles( fromDirectory, "*.*", SearchOption.AllDirectories );
            string[] directories = Directory.GetDirectories( fromDirectory, "*.*", SearchOption.AllDirectories );

            foreach( string directory in directories )
            {
                string directoryPath = Path.GetFullPath( directory );
                string newDirectoryPath = directoryPath.Replace( fromDirectory, toDirectory );

                Directory.CreateDirectory( newDirectoryPath );
            }

            foreach( string file in files )
            {
                string filePath = Path.GetFullPath( file );
                string newFilePath = filePath.Replace( fromDirectory, toDirectory );

                File.Copy( filePath, newFilePath );
            }
        }
    }
}
