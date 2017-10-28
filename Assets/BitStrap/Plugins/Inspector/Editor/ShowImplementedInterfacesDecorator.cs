using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( ShowImplementedInterfacesAttribute ) )]
	public sealed class ShowImplementedInterfacesDecorator : DecoratorDrawer
	{
		private const float Padding = 8.0f;
		private const float Width = 256.0f;
		private const float TitleLabelGray = 0.25f;
		private const float InterfaceLabelGray = 0.18f;
		private const float InterfacePadding = 8.0f;
		private const float OddBackgroundGray = 0.72f;

		private static GUIStyle titleStyle = null;
		private static GUIStyle interfaceStyle = null;
		private static GUIStyle boxStyle = null;

		public override float GetHeight()
		{
			var showInterfacesAttribute = attribute as ShowImplementedInterfacesAttribute;
			if( showInterfacesAttribute == null )
				return base.GetHeight();

			int interfaceCount = showInterfacesAttribute.type.GetInterfaces().Length;
			return EditorGUIUtility.singleLineHeight * ( 1 + interfaceCount ) + Padding;
		}

		public override void OnGUI( Rect position )
		{
			TryInitStyles();

			var showInterfacesAttribute = attribute as ShowImplementedInterfacesAttribute;
			if( showInterfacesAttribute == null )
				return;

			position = position.Center( Width );

			var box = new Rect( position );
			box.height -= Padding - 1;
			GUI.Box( box, GUIContent.none, GUI.skin.window );

			Rect titlePosition = position.Row( 0 );
			EditorGUI.LabelField( titlePosition, "Implemented Interfaces", titleStyle );

			var defaultBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = new Color( OddBackgroundGray, OddBackgroundGray, OddBackgroundGray, 1.0f );

			var interfaceTypes = showInterfacesAttribute.type.GetInterfaces();
			for( int i = 0; i < interfaceTypes.Length; i++ )
			{
				Rect interfacePosition = position.Row( i + 1 );
				var interfaceType = interfaceTypes[i];

				if( i % 2 == 1 )
					GUI.Box( interfacePosition.Right( -1.0f ), GUIContent.none, boxStyle );

				EditorGUI.LabelField( interfacePosition.Right( -InterfacePadding ), interfaceType.Name, interfaceStyle );
			}

			GUI.backgroundColor = defaultBackgroundColor;
		}

		private static void TryInitStyles()
		{
			if( titleStyle == null )
			{
				titleStyle = new GUIStyle( EditorStyles.boldLabel );
				titleStyle.alignment = TextAnchor.MiddleCenter;
				titleStyle.normal.textColor = new Color( TitleLabelGray, TitleLabelGray, TitleLabelGray, 1.0f );
			}

			if( interfaceStyle == null )
			{
				interfaceStyle = new GUIStyle( EditorStyles.label );
				interfaceStyle.normal.textColor = new Color( InterfaceLabelGray, InterfaceLabelGray, InterfaceLabelGray, 1.0f );
			}

			if( boxStyle == null )
			{
				boxStyle = new GUIStyle( GUI.skin.box );
				boxStyle.border = new RectOffset( 0, 0, 0, 0 );
				boxStyle.normal.background = Texture2D.whiteTexture;
			}
		}
	}
}