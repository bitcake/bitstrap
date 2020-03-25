using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace BitStrap
{
	/// <summary>
	/// A window like the inspector that shows the information of a scriptable object in a separate window.
	/// The window loses it's reference once unity is closed, but the name is stored, so in most cases a recovery
	/// is possible with the button shown when the reference is lost.
	/// This window is used by <see cref="OpenAssetListener"/>.
	/// </summary>
	public class ScriptableObjectInspectorWindow : EditorWindow
	{
		private static List<ScriptableObjectInspectorWindow> openWindows = new List<ScriptableObjectInspectorWindow>();
		private ScriptableObject scriptableObject;
		private Editor editor;
		private Vector2 scrollPosition = Vector2.zero;

		public static void Init(ScriptableObject scriptableObject)
		{
			for (int i = 0; i < openWindows.Count; i++)
			{
				var currentWindow = openWindows[i];
				// Check for the name to offer solution when the editor was closed
				if (currentWindow.titleContent.text == scriptableObject.name)
				{
					if (currentWindow.scriptableObject == null)
					{
						currentWindow.SetScriptableObject(scriptableObject);
					}
					currentWindow.Focus();
					currentWindow.Show();
					return;
				}
			}
			ScriptableObjectInspectorWindow window = (ScriptableObjectInspectorWindow) EditorWindow.CreateInstance(typeof(ScriptableObjectInspectorWindow));
			window.titleContent = new GUIContent(scriptableObject.name);
			window.SetScriptableObject(scriptableObject);
			openWindows.Add(window);
			window.Show();
		}

		private void SetScriptableObject(ScriptableObject scriptableObject)
		{
			this.scriptableObject = scriptableObject;
			this.editor = Editor.CreateEditor(scriptableObject);
		}

		void OnGUI()
		{
			if (scriptableObject == null)
			{
				if (!openWindows.Contains(this))
				{
					openWindows.Add(this);
				}
				GUILayout.Space(30);
				EditorGUILayout.HelpBox("This window forgot its scriptable object due to a editor restart.", MessageType.Error);
				if (GUILayout.Button("Find the scriptable object"))
				{
					string searchString = this.titleContent.text + " t:ScriptableObject";
					string[] assets = AssetDatabase.FindAssets(searchString);
					if (assets.Length == 0)
					{
						EditorUtility.DisplayDialog("No assets found", "No assets with the name " + searchString + " were found.", "Ok");
						return;
					}
					string assetPath = AssetDatabase.GUIDToAssetPath(assets[0]);
					SetScriptableObject(AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath));
				}
				return;
			}
			float width = this.position.width;
			float height = this.position.height;

			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));
			{
				editor.OnInspectorGUI();
			}
			GUILayout.EndScrollView();
		}

		void OnDestroy()
		{
			openWindows.Remove(this);
		}
	}
}