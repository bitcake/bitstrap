using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace BitStrap
{
	/// <summary>
	/// Check the BitStrap's preferences at "Edit/Preferences/BitStrap".
	/// </summary>
	public static class BitStrapPreferences
	{
		private static SortedList<string, System.Action> drawPreferencesCallback = new SortedList<string, System.Action>();

		public static void RegisterPreference( System.Action drawCallback )
		{
			if( drawCallback != null && drawCallback.Method != null )
			{
				string typeName = drawCallback.Method.DeclaringType.Name;
				drawPreferencesCallback[typeName] = drawCallback;
			}
		}

		[PreferenceItem( "BitStrap" )]
		private static void OnPreferencesGUI()
		{
			if( GUILayout.Button( "Open Web Documentation" ) )
				BitStrapDocs.OpenDocs();

			foreach( var pair in drawPreferencesCallback )
			{
				if( pair.Value != null )
					pair.Value();
			}

			GUILayout.FlexibleSpace();
		}
	}
}
