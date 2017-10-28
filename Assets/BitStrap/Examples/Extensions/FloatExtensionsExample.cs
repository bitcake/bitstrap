using UnityEngine;

namespace BitStrap.Examples
{
	public class FloatExtensionsExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public float ValueToMap = .25f;

		public float OriginalMinRange = 0;
		public float OriginalMaxRange = 1;
		public float NewMinRange = 0;
		public float NewMaxRange = 10;
		public float NewValue;

		[Button]
		public void MapRange()
		{
			NewValue = ValueToMap.MapRange( OriginalMinRange, OriginalMaxRange, NewMinRange, NewMaxRange );
			Debug.LogFormat( "Old value: {0}, new value: {1}", ValueToMap, NewValue );
		}
	}
}
