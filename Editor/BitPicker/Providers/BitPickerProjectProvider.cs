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
		public string[] folderWhitelist = {
			"Assets",
			"ProjectSettings"
		};

		public override void Provide( List<BitPickerItem> providedItems )
		{
			foreach( var path in AssetDatabase.GetAllAssetPaths() )
			{
				if( excludeFolders && AssetDatabase.IsValidFolder( path ) )
					continue;

				var isWhitelisted = false;
				foreach( var whitelistedFolder in folderWhitelist )
				{
					if( path.StartsWith( whitelistedFolder ) )
					{
						isWhitelisted = true;
						break;
					}
				}
				if( !isWhitelisted )
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

		public override void OnPingItem( BitPickerItem item )
		{
			var asset = AssetDatabase.LoadAssetAtPath<Object>( item.fullName );
			if( asset != null )
			{
				EditorGUIUtility.PingObject( asset );
				Selection.activeObject = asset;
			}
		}

		public override void OnOpenItem( BitPickerItem item, string pattern )
		{
			if( AssetDatabase.IsValidFolder( item.fullName ) )
			{
				EditorUtility.RevealInFinder( item.fullName );
				return;
			}

			var asset = AssetDatabase.LoadAssetAtPath<Object>( item.fullName );
			if( asset != null )
			{
				var extension = Path.GetExtension( item.fullName );
				foreach( var e in openAssetByExtensions )
				{
					if( extension == e )
					{
						var args = BitPickerHelper.GetArgs( pattern );
						var lineNumber = 0;

						if( int.TryParse( args, out lineNumber ) )
							AssetDatabase.OpenAsset( asset, lineNumber );
						else
							AssetDatabase.OpenAsset( asset );

						return;
					}
				}

				EditorGUIUtility.PingObject( asset );
				Selection.activeObject = asset;
			}
		}

		public override Object[] GetItemDragReferences( BitPickerItem item )
		{
			var asset = AssetDatabase.LoadAssetAtPath<Object>( item.fullName );
			if( asset == null)
				return null;
			return new Object[] { asset };
		}
	}
}