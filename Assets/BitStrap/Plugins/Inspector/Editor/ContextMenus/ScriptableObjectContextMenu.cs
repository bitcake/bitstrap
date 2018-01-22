using UnityEditor;

namespace BitStrap
{
	public class ScriptableObjectContextMenu
	{
		[MenuItem("CONTEXT/ScriptableObject/Select in Project View")]
		public static void SelectScriptabelObject(MenuCommand menuCommand)
		{
			EditorGUIUtility.PingObject(menuCommand.context);
		}
	}
}