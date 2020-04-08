using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( TweenShaderProperty ) )]
	public sealed class TweenShaderPropertyDrawer : PropertyDrawer
	{
		private const float TypeWidth = 64.0f;
		private const float CurveWidth = 32.0f;
		private const float FromToPadding = 32.0f;
		private const float FromToLabelWidth = 48.0f;

#if UNITY_5 || UNITY_2017
		private readonly static ColorPickerHDRConfig colorPickerConfig = new ColorPickerHDRConfig( 0.0f, float.MaxValue, 0.0f, float.MaxValue );
#endif

		private readonly static GUIContent fromLabel = new GUIContent( "From" );
		private readonly static GUIContent toLabel = new GUIContent( "To" );

		private TweenShaderPropertiesCache propertiesCache = new TweenShaderPropertiesCache();

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			return EditorGUIUtility.singleLineHeight * 3;
		}

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			var nameProperty = property.GetMemberProperty<TweenShaderProperty>( p => p.name );
			var typeProperty = property.GetMemberProperty<TweenShaderProperty>( p => p.type );
			var curveProperty = property.GetMemberProperty<TweenShaderProperty>( p => p.curve );
			var fromProperty = property.GetMemberProperty<TweenShaderProperty>( p => p.from );
			var toProperty = property.GetMemberProperty<TweenShaderProperty>( p => p.to );

			var mainRow = position.Row( 0 );
			var fromRect = position.Row( 1 ).Right( -FromToPadding );
			var toRect = position.Row( 2 ).Right( -FromToPadding );

			Rect nameRect, typeRect, curveRect;
			nameRect = mainRow.Right( CurveWidth, out curveRect ).Right( TypeWidth, out typeRect );

			var tweenShader = property.serializedObject.targetObject as TweenShader;
			var targetRenderer = tweenShader.targetRenderer;

			if( targetRenderer == null )
			{
				nameProperty.stringValue = EditorGUI.TextField( nameRect, nameProperty.stringValue );
				typeProperty.enumValueIndex = EditorGUI.Popup( typeRect, typeProperty.enumValueIndex, typeProperty.enumDisplayNames );
			}
			else
			{
				propertiesCache.UpdateProperties( tweenShader );

				int index = System.Array.IndexOf( propertiesCache.propertyNameOptions, nameProperty.stringValue );
				index = EditorGUI.Popup( nameRect, index, propertiesCache.propertyNameOptions );

				if( index < 0 )
					return;

				var shaderProperty = propertiesCache.properties[index];

				nameProperty.stringValue = shaderProperty.name;
				typeProperty.enumValueIndex = ( int ) shaderProperty.type;

				using( DisabledGroup.Do( true ) )
				{
					EditorGUI.Popup( typeRect, typeProperty.enumValueIndex, typeProperty.enumDisplayNames );
				}
			}

			EditorGUI.PropertyField( curveRect, curveProperty, GUIContent.none );

			using( LabelWidth.Do( FromToLabelWidth ) )
			{
				Vector4 fromVector = fromProperty.vector4Value;
				Vector4 toVector = toProperty.vector4Value;

				var type = ( TweenShaderProperty.Type ) typeProperty.enumValueIndex;
				switch( type )
				{
				case TweenShaderProperty.Type.Float:
					fromVector.x = EditorGUI.FloatField( fromRect, fromLabel, fromVector.x );
					fromProperty.vector4Value = fromVector;

					toVector.x = EditorGUI.FloatField( toRect, toLabel, toVector.x );
					toProperty.vector4Value = toVector;
					break;

				case TweenShaderProperty.Type.Vector:
					float labelWidth = EditorGUIUtility.labelWidth;

					EditorGUI.LabelField( fromRect.Left( labelWidth ), fromLabel );
					fromVector = EditorGUI.Vector4Field( fromRect.Right( -labelWidth ).Row( 0 ), "", fromVector );
					fromProperty.vector4Value = fromVector;

					EditorGUI.LabelField( toRect.Left( labelWidth ), toLabel );
					toVector = EditorGUI.Vector4Field( toRect.Right( -labelWidth ).Row( 0 ), "", toVector );
					toProperty.vector4Value = toVector;

					break;

				case TweenShaderProperty.Type.Color:
#if UNITY_5 || UNITY_2017
					fromVector = EditorGUI.ColorField( fromRect, fromLabel, fromVector, true, true, true, colorPickerConfig );
					toVector = EditorGUI.ColorField( toRect, toLabel, toVector, true, true, true, colorPickerConfig );
#else
					fromVector = EditorGUI.ColorField( fromRect, fromLabel, fromVector, true, true, true );
					toVector = EditorGUI.ColorField( toRect, toLabel, toVector, true, true, true );
#endif

					fromProperty.vector4Value = fromVector;
					toProperty.vector4Value = toVector;

					break;
				}
			}

			property.serializedObject.ApplyModifiedProperties();
		}
	}
}