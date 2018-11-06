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

		public virtual void OnSelectItem( BitPickerItem selectedItem )
		{
			Debug.LogFormat( "On selected item {0} ({1})", selectedItem.name, selectedItem.fullName );
		}
	}
}
