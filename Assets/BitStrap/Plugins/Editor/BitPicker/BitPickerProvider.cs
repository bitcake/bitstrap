using UnityEngine;
using System.Collections.Generic;

namespace BitStrap
{
	public class BitPickerProvider : ScriptableObject
	{
		public virtual void Provide( List<BitPickerItem> providedItems )
		{
		}

		public virtual string GetItemProvisionSource( BitPickerItem item )
		{
			return "<nowhere>";
		}

		public virtual Texture2D GetItemIcon( BitPickerItem item )
		{
			return Texture2D.whiteTexture;
		}

		public virtual void OnSelectItem( BitPickerItem selectedItem )
		{
		}
	}
}
