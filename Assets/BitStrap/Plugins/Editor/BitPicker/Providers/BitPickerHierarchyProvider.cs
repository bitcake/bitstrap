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
			var transformsCache = new List<Transform>( 32 );
			var pathBuilder = new StringBuilder( 128 );
			var pathComponentsCache = new List<string>( 16 );

			for( var i = 0; i < SceneManager.sceneCount; i++ )
			{
				var scene = SceneManager.GetSceneAt( i );

				rootGameObjectsCache.Clear();
				scene.GetRootGameObjects( rootGameObjectsCache );

				foreach( var root in rootGameObjectsCache )
				{
					transformsCache.Clear();
					root.GetComponentsInChildren<Transform>( true, transformsCache );

					foreach( var transform in transformsCache )
					{
						pathBuilder.Length = 0;
						pathBuilder.Append( scene.name );
						pathBuilder.Append( "/" );

						pathComponentsCache.Clear();
						for( var t = transform; t != null; t = t.parent )
							pathComponentsCache.Add( t.name );

						for( var j = pathComponentsCache.Count - 1; j > 0; j-- )
						{
							pathBuilder.Append( pathComponentsCache[j] );
							pathBuilder.Append( "/" );
						}
						pathBuilder.Append( pathComponentsCache[0] );

						providedItems.Add( new BitPickerItem(
							this,
							transform.name,
							pathBuilder.ToString(),
							transform.gameObject
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
			return AssetPreview.GetMiniTypeThumbnail( typeof( GameObject ) );
		}

		public override void OnPingItem( BitPickerItem item )
		{
			var gameObject = item.data as GameObject;
			if( gameObject != null )
			{
				EditorGUIUtility.PingObject( gameObject );
				Selection.activeGameObject = gameObject;
			}
		}

		public override void OnOpenItem( BitPickerItem item, string pattern )
		{
			var gameObject = item.data as GameObject;
			if( gameObject != null )
			{
				EditorGUIUtility.PingObject( gameObject );
				Selection.activeGameObject = gameObject;
			}
		}
	}
}