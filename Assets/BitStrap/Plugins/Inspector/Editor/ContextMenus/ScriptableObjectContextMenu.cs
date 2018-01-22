using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public class ScriptableObjectContextMenu
	{
		[MenuItem("CONTEXT/ScriptableObject/Select Script", false, 10)]
		public static void SelectScriptabelObjectScript(MenuCommand menuCommand)
		{
			var serializedObject = new SerializedObject(menuCommand.context);
			var scriptProperty = serializedObject.FindProperty("m_Script");
			var scriptObject = scriptProperty.objectReferenceValue;
			Selection.activeObject = scriptObject;
		}

		[MenuItem("CONTEXT/ScriptableObject/Show in Project View")]
		public static void SelectScriptabelObject(MenuCommand menuCommand)
		{
			EditorGUIUtility.PingObject(menuCommand.context);
		}
	}
}