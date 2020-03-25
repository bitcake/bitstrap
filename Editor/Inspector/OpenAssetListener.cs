using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Diagnostics;
using System.IO;

namespace BitStrap
{
	/// <summary>
	/// Controls the custom behavior of double clicks on scriptable objects.
	/// The scriptable object will be opened in anew window instead of only showing it in the inspector.
	/// </summary>
	public class OpenAssetListener
	{
		[OnOpenAsset( 999 )]
		public static bool HandleOpenAsset( int instanceID, int line )
		{
			UnityEngine.Object obj = EditorUtility.InstanceIDToObject( instanceID );
			if( obj == null )
			{
				return false;
			}

			Type type = obj.GetType();

			if( type.IsSubclassOf( typeof( ScriptableObject ) ) )
			{
				ScriptableObjectInspectorWindow.Init( ( ScriptableObject ) obj );
				return true;
			}

			return false; // No custom option was hit, leaving opening file to next handler
		}

		private static bool RunApplication( string applicationPath, string filePath )
		{
			Process process = new Process();
			process.StartInfo.FileName = applicationPath;
			process.StartInfo.Arguments = filePath;
			return process.Start();
		}
	}
}