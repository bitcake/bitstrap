using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer(typeof(ExponentInfoAttribute))]
	public class ExponentInfoDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label);
			ExponentInfoAttribute labelAttribute = attribute as ExponentInfoAttribute;
			if (labelAttribute == null)
			{
				return;
			}

			string exp = string.Empty;
			if (property.type.Equals("int"))
			{
				var value = property.intValue;
				if (Math.Abs(value) < 1000)
				{
					return;
				}
				exp = value.ToString("G2", CultureInfo.InvariantCulture);
			}
			else if (property.type.Equals("long"))
			{
				var value = property.longValue;
				if (Math.Abs(value) < 1000)
				{
					return;
				}
				exp = value.ToString("G2", CultureInfo.InvariantCulture);
			}
			else if (property.type.Equals("float"))
			{
				var value = property.floatValue;
				if (Math.Abs(value) < 1000 &&
					value.ToString(CultureInfo.InvariantCulture).Length < 5)
				{
					return;
				}
				exp = value.ToString("G2", CultureInfo.InvariantCulture);
			}
			else if (property.type.Equals("double"))
			{
				var value = property.doubleValue;
				if (Math.Abs(value) < 1000 &&
					value.ToString(CultureInfo.InvariantCulture).Length < 5)
				{
					return;
				}
				exp = value.ToString("G2", CultureInfo.InvariantCulture);
			}
			GUI.Label(position.Right(labelAttribute.width + 2f), exp, labelAttribute.labelStyle);
		}
	}
}