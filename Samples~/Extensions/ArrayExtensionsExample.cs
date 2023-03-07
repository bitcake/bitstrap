using UnityEngine;

namespace BitStrap.Examples
{
	public class ArrayExtensionsExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public int[] array = { 0, 0, 1, 2, 3 };

		[Button]
		public void ElementZeroCount()
		{
			Debug.LogFormat( "There are {0} zeros in the array.", array.Count( e => e == 0 ) );
		}

		[Button]
		public void AreAllZeros()
		{
			Debug.LogFormat( "Are all elements in array zero? {0}.", array.All( e => e == 0 ) );
		}

		[Button]
		public void IsThereAnyZeros()
		{
			Debug.LogFormat( "Is there any zero element in array? {0}.", array.Any( e => e == 0 ) );
		}

		[Button]
		public void GetFirstElementOrDefaultValue()
		{
			Debug.LogFormat( "First element or -999 value is {0}.", array.First().UnwrapOr( -999 ) );
		}

		[Button]
		public void PrettyPrintArray()
		{
			Debug.Log( array.ToStringFull() );
		}
	}
}
