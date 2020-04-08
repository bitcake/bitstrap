using System.Collections.Generic;
using UnityEngine;

namespace BitStrap.Examples
{
	public class ListExtensionsExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public List<int> list = new List<int>( new int[] { 0, 0, 1, 2, 3 } );

		[Button]
		public void ForEachIterationWithNoGarbage()
		{
			foreach( var element in list.Iter() )
			{
				Debug.LogFormat( "This is an iteration. Element = {0}", element );
			}
		}

		[Button]
		public void ElementZeroCount()
		{
			Debug.LogFormat( "There are {0} zeros in the list.", list.Count( e => e == 0 ) );
		}

		[Button]
		public void AreAllZeros()
		{
			Debug.LogFormat( "Are all elements in list zero? {0}.", list.All( e => e == 0 ) );
		}

		[Button]
		public void IsThereAnyZeros()
		{
			Debug.LogFormat( "Is there any zero element in list? {0}.", list.Any( e => e == 0 ) );
		}

		[Button]
		public void GetFirstElementOrDefaultValue()
		{
			Debug.LogFormat( "First element or -999 value is {0}.", list.First().UnwrapOr( -999 ) );
		}

		[Button]
		public void PrettyPrintList()
		{
			Debug.Log( list.ToStringFull() );
		}
	}
}
