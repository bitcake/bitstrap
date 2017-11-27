using UnityEngine;

namespace BitStrap
{
	public struct Unit
	{
		public static Unit Do( System.Action action )
		{
			action();
			return new Unit();
		}
	}
}