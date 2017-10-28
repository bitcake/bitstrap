using System.Collections;
using UnityEngine;

namespace BitStrap
{
	public static class BitArrayExtensions
	{
#if NETFX_CORE
		public static void CopyTo( this BitArray self, System.Array array, int index )
		{
			( self as ICollection ).CopyTo( array, index );
		}
#endif
	}
}