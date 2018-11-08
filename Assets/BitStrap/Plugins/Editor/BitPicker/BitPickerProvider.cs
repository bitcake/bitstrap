using UnityEngine;
using System.Collections.Generic;

namespace BitStrap
{
	public class BitPickerProvider : ScriptableObject
	{
		public virtual void Provide( List<BitPickerItem> providedItems )
		{
		}

		public virtual string GetProvisionSource()
		{
			return "<nowhere>";
		}

		public virtual Texture2D GetItemIcon( BitPickerItem item )
		{
			return BitPickerItem.EmptyIcon;
		}

		public virtual void OnPingItem( BitPickerItem item )
		{
		}

		public virtual void OnOpenItem( BitPickerItem item, string pattern )
		{
			Debug.LogFormat( "On selected item {0} ({1})", item.name, item.fullName );
		}

		public virtual Object[] GetItemDragReferences( BitPickerItem item )
		{
			return null;
		}
	}
}
