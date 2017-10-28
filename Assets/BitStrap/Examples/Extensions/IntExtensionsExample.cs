using UnityEngine;

namespace BitStrap.Examples
{
	public class IntExtensionsExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public int ValueToMap = 5;

		public int OriginalMinRange = 0;
		public int OriginalMaxRange = 10;
		public int NewMinRange = 0;
		public int NewMaxRange = 1000;
		public int NewValue;

		[Button]
		public void MapRange()
		{
			NewValue = ValueToMap.MapRange( OriginalMinRange, OriginalMaxRange, NewMinRange, NewMaxRange );
			Debug.LogFormat( "Old value: {0}, new value: {1}", ValueToMap, NewValue );
		}
	}
}
