using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Searches through a target class in order to find all button attributes (<see cref="ButtonAttribute"/>).
	/// </summary>
	public sealed class ButtonAttributeHelper
	{
		private static object[] emptyParamList = new object[0];

		private IList<MethodInfo> methods = new List<MethodInfo>();
		private Object targetObject;

		public void Init( Object targetObject )
		{
			this.targetObject = targetObject;
			methods =
				targetObject.GetType()
					.GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
					.Where( m =>
							 m.GetCustomAttributes( typeof( ButtonAttribute ), false ).Length == 1 &&
							 m.GetParameters().Length == 0 &&
							 !m.ContainsGenericParameters
					).ToList();
		}

		public void DrawButtons()
		{
			if( methods.Count > 0 )
			{
				EditorGUILayout.HelpBox( "Click to execute methods!", MessageType.None );
				ShowMethodButtons();
			}
		}

		private void ShowMethodButtons()
		{
			foreach( MethodInfo method in methods )
			{
				string buttonText = ObjectNames.NicifyVariableName( method.Name );
				if( GUILayout.Button( buttonText ) )
				{
					method.Invoke( targetObject, emptyParamList );
				}
			}
		}
	}
}