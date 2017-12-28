using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Custom editor for all ScriptableObject scripts in order to draw buttons for all button attributes (<see cref="ButtonAttribute"/>).
	/// </summary>
	[CustomEditor( typeof( ScriptableObject ), true, isFallback = true )]
	[CanEditMultipleObjects]
	public sealed class ScriptableObjectButtonsEditor : Editor
	{
		private ButtonAttributeHelper helper = new ButtonAttributeHelper();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			helper.DrawButtons();
		}

		private void OnEnable()
		{
			helper.Init( target );
		}
	}
}