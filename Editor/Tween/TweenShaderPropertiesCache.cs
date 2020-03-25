using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace BitStrap
{
	public sealed class TweenShaderPropertiesCache
	{
		public struct ShaderProperty
		{
			public string name;
			public TweenShaderProperty.Type type;
		}

		public readonly List<ShaderProperty> properties = new List<ShaderProperty>();
		public string[] propertyNameOptions;
		private TweenShader previousTweenShader = null;

		public void UpdateProperties( TweenShader tweenShader )
		{
			if( tweenShader != null && tweenShader != previousTweenShader )
			{
				previousTweenShader = tweenShader;
				BuildShaderPropertyList( tweenShader );
			}
		}

		private void BuildShaderPropertyList( TweenShader tweenShader )
		{
			var targetRenderer = tweenShader.targetRenderer;

			properties.Clear();

			if( targetRenderer == null )
				return;

			var sharedMaterials = targetRenderer.sharedMaterials;
			foreach( var material in sharedMaterials )
			{
				var shader = material.shader;
				int propertyCount = ShaderUtil.GetPropertyCount( shader );
				for( int i = 0; i < propertyCount; i++ )
				{
					var propertyType = ShaderUtil.GetPropertyType( shader, i );
					switch( propertyType )
					{
					case ShaderUtil.ShaderPropertyType.Float:
					case ShaderUtil.ShaderPropertyType.Range:
						properties.Add( new ShaderProperty
						{
							name = ShaderUtil.GetPropertyName( shader, i ),
							type = TweenShaderProperty.Type.Float
						} );
						break;

					case ShaderUtil.ShaderPropertyType.Vector:
						properties.Add( new ShaderProperty
						{
							name = ShaderUtil.GetPropertyName( shader, i ),
							type = TweenShaderProperty.Type.Vector
						} );
						break;

					case ShaderUtil.ShaderPropertyType.Color:
						properties.Add( new ShaderProperty
						{
							name = ShaderUtil.GetPropertyName( shader, i ),
							type = TweenShaderProperty.Type.Color
						} );
						break;

					case ShaderUtil.ShaderPropertyType.TexEnv:
						properties.Add( new ShaderProperty
						{
							name = ShaderUtil.GetPropertyName( shader, i ) + "_ST",
							type = TweenShaderProperty.Type.Vector
						} );
						break;
					}
				}
			}

			propertyNameOptions = properties.Select( p => p.name ).ToArray();
		}
	}
}