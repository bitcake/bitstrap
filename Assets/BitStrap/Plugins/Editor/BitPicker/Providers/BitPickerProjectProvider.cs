using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace BitStrap
{
	public sealed class BitPickerProjectProvider : BitPickerProvider
	{
		public bool excludeFolders = true;
		public string[] openAssetByExtensions = {
			".cs"
		};

		public override void Provide( List<BitPickerItem> providedItems )
		{
			foreach( var path in AssetDatabase.GetAllAssetPaths() )
			{
				if( excludeFolders && AssetDatabase.IsValidFolder( path ) )
					continue;

				providedItems.Add( new BitPickerItem(
					this,
					Path.GetFileName( path ),
					path,
					null
				) );
			}
		}

		public override string GetProvisionSource()
		{
			return "Project";
		}

		public override Texture2D GetItemIcon( BitPickerItem item )
		{
			var asset = AssetDatabase.LoadAssetAtPath<Object>( item.fullName );
			return AssetPreview.GetMiniThumbnail( asset );
		}

		public override void OnSelectItem( BitPickerItem selectedItem )
		{
			if( AssetDatabase.IsValidFolder( selectedItem.fullName ) )
			{
				EditorUtility.RevealInFinder( selectedItem.fullName );
				return;
			}

			var asset = AssetDatabase.LoadAssetAtPath<Object>( selectedItem.fullName );
			if( asset != null )
			{
				EditorGUIUtility.PingObject( asset );
				Selection.activeObject = asset;

				var extension = Path.GetExtension( selectedItem.fullName );
				foreach( var e in openAssetByExtensions )
				{
					if( extension == e )
					{
						AssetDatabase.OpenAsset( asset );
						return;
					}
				}
			}
		}
	}
}