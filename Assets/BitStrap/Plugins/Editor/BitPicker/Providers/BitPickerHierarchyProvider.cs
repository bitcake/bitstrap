using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text;

namespace BitStrap
{
	public sealed class BitPickerHierarchyProvider : BitPickerProvider
	{
		public override void Provide( List<BitPickerItem> providedItems )
		{
			var rootGameObjectsCache = new List<GameObject>( 128 );
			var componentCache = new List<Component>( 64 );
			var pathBuilder = new StringBuilder( 128 );
			var pathComponentsCache = new List<string>( 16 );

			for( var i = 0; i < SceneManager.sceneCount; i++ )
			{
				var scene = SceneManager.GetSceneAt( i );

				rootGameObjectsCache.Clear();
				scene.GetRootGameObjects( rootGameObjectsCache );

				foreach( var root in rootGameObjectsCache )
				{
					componentCache.Clear();
					root.GetComponentsInChildren<Component>( true, componentCache );

					foreach( var component in componentCache )
					{
						pathBuilder.Length = 0;
						pathBuilder.Append( scene.name );
						pathBuilder.Append( "/" );

						pathComponentsCache.Clear();
						for( var t = component.transform; t != null; t = t.parent )
							pathComponentsCache.Add( t.name );

						for( var j = pathComponentsCache.Count - 1; j > 0; j-- )
						{
							pathBuilder.Append( pathComponentsCache[j] );
							pathBuilder.Append( "/" );
						}
						pathBuilder.Append( pathComponentsCache[0] );

						providedItems.Add( new BitPickerItem(
							this,
							string.Format( "{0} ({1})", component.name, component.GetType().Name ),
							pathBuilder.ToString(),
							component
						) );
					}
				}
			}
		}

		public override string GetProvisionSource()
		{
			return "Hierarchy";
		}

		public override Texture2D GetItemIcon( BitPickerItem item )
		{
			var component = item.data as Component;
			return AssetPreview.GetMiniTypeThumbnail( component.GetType() );
		}

		public override void OnPingItem( BitPickerItem item )
		{
			var component = item.data as Component;
			if( component != null )
			{
				EditorGUIUtility.PingObject( component.gameObject );
				Selection.activeGameObject = component.gameObject;
			}
		}

		public override void OnOpenItem( BitPickerItem item, string pattern )
		{
			var component = item.data as Component;
			if( component != null )
			{
				EditorGUIUtility.PingObject( component.gameObject );
				Selection.activeGameObject = component.gameObject;
			}
		}

		public override Object[] GetItemDragReferences( BitPickerItem item )
		{
			var component = item.data as Component;
			if( component == null )
				return null;
			return new Object[] { component.gameObject };
		}
	}
}
