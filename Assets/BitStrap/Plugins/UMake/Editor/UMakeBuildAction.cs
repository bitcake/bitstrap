using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public class UMakeBuildAction : ScriptableObject
	{
		public virtual void Execute( UMake umake, UMakeTarget target )
		{
		}

		public virtual void OnInspectorGUI( UMakeBuildActionEditor editor )
		{
			editor.DrawBaseInspectorGUI();
		}
	}
}