using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace BitStrap
{
	public sealed class BitPickerProjectProvider : BitPickerProvider
	{
		public bool excludeFolders = true;

		public override void Provide( List<BitPickerItem> providedItems )
		{
			foreach( var path in AssetDatabase.GetAllAssetPaths() )
			{
				if( excludeFolders && AssetDatabase.IsValidFolder( path ) )
					continue;

				providedItems.Add( new BitPickerItem( this, Path.GetFileName( path ), path ) );
			}
		}

		public override string GetItemProvisionSource( BitPickerItem item )
		{
			return "Project";
		}

		public override Texture2D GetItemIcon( BitPickerItem item )
		{
			var asset = AssetDatabase.LoadAssetAtPath<Object>( item.fullName );
			var icon = AssetPreview.GetMiniThumbnail( asset );
			if( icon == null )
				icon = Texture2D.whiteTexture;

			return icon;
		}

		public override void OnSelectItem( BitPickerItem selectedItem )
		{
			var asset = AssetDatabase.LoadAssetAtPath<Object>( selectedItem.fullName );
			if( asset != null )
			{
				EditorGUIUtility.PingObject( asset );
				Selection.activeObject = asset;
				//AssetDatabase.OpenAsset( asset );
			}
		}
	}
}