using System;
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

#if UNITY_2019_1_OR_NEWER
		[SettingsProvider()]
		private static SettingsProvider OnPreferencesGUI()
		{
			Vector2 scroll = Vector2.zero;
			return new SettingsProvider("Settings/BitStrap", SettingsScope.User)
			{
				label = "BitStrap",
				guiHandler = (searchContext) =>
				{
					if (GUILayout.Button("Open Web Documentation"))
						BitStrapDocs.OpenDocs();

					EditorGUILayout.Separator();
					foreach (var pair in drawPreferencesCallback)
					{
						if (pair.Value != null)
							pair.Value();
					}

					EditorGUILayout.Separator();
					GUILayout.FlexibleSpace();
				}
			};
		}

#else
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
#endif
	}
}
