using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( ExponentInfoAttribute ) )]
	public sealed class ExponentInfoDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );
			EditorGUI.PropertyField( position, property, label );

			var exponentInfoAttribute = attribute as ExponentInfoAttribute;

			string exp = string.Empty;
			if( property.propertyType == SerializedPropertyType.Integer )
			{
				var value = property.intValue;
				if( Mathf.Abs( value ) < 1000 )
					return;

				exp = value.ToString( "G2", CultureInfo.InvariantCulture );
			}
			else if( property.propertyType.Equals( "long" ) )
			{
				var value = property.longValue;
				if( Mathf.Abs( value ) < 1000 )
					return;

				exp = value.ToString( "G2", CultureInfo.InvariantCulture );
			}
			else if( property.propertyType == SerializedPropertyType.Float )
			{
				var value = property.floatValue;
				if( Mathf.Abs( value ) < 1000 &&
					value.ToString( CultureInfo.InvariantCulture ).Length < 5 )
					return;

				exp = value.ToString( "G2", CultureInfo.InvariantCulture );
			}
			else if( property.type.Equals( "double" ) )
			{
				var value = property.doubleValue;
				if( System.Math.Abs( value ) < 1000 &&
					value.ToString( CultureInfo.InvariantCulture ).Length < 5 )
					return;

				exp = value.ToString( "G2", CultureInfo.InvariantCulture );
			}

			UnitDrawer.DrawUnit( position, exp, exponentInfoAttribute.style );
		}
	}
}