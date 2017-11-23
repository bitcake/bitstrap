using UnityEngine;

namespace BitStrap
{
	public struct Unit
	{
		public static Unit Do<T>( T a )
		{
			return new Unit();
		}

		public static Unit Do( System.Action action )
		{
			if( action != null )
				action();
			return new Unit();
		}
	}
}