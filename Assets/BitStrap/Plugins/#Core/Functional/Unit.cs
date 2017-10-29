using UnityEngine;

namespace BitStrap
{
	public struct Unit
	{
		public static Unit Do<T>( T a )
		{
			return default( Unit );
		}

		public static Unit Do( System.Action action )
		{
			action();
			return default( Unit );
		}
	}
}