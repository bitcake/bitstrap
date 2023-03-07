using System.Collections.Generic;
using UnityEngine;

namespace BitStrap.Examples
{
	public class DictionaryExtensionsExample : MonoBehaviour
	{
		[System.Serializable]
		public struct Pair
		{
			public string key;
			public int value;
		}

		[Header( "Edit the fields and click the buttons to test them!" )]
		public Pair[] dictionary = new Pair[] {
			new Pair { key = "element0", value = 0 },
			new Pair { key = "element1", value = 1 },
			new Pair { key = "element2", value = 2 },
			new Pair { key = "element3", value = 3 },
		};

		private Dictionary<string, int> actualDictionary = new Dictionary<string, int>();

		[Button]
		public void ForEachIterationWithNoGarbage()
		{
			BuildDictionary();
			foreach( var pair in actualDictionary.Iter() )
			{
				Debug.LogFormat( "This is an iteration. Key = {0}, Value = {1}", pair.Key, pair.Value );
			}
		}

		[Button]
		public void ElementZeroCount()
		{
			BuildDictionary();
			Debug.LogFormat( "There are {0} zeros values in the dictionary.", actualDictionary.Count( e => e.Value == 0 ) );
		}

		[Button]
		public void AreAllZeros()
		{
			BuildDictionary();
			Debug.LogFormat( "Are all values in dictionary zero? {0}.", actualDictionary.All( e => e.Value == 0 ) );
		}

		[Button]
		public void IsThereAnyZeros()
		{
			BuildDictionary();
			Debug.LogFormat( "Is there any zero element value in dictionary? {0}.", actualDictionary.Any( e => e.Value == 0 ) );
		}

		[Button]
		public void GetFirstElementOrDefaultValue()
		{
			BuildDictionary();
			var value = from e in actualDictionary.First() select e.Value;
			Debug.LogFormat( "First element value or -999 value is {0}.", value.UnwrapOr( -999 ) );
		}

		[Button]
		public void PrettyPrintDictionary()
		{
			BuildDictionary();
			Debug.Log( actualDictionary.ToStringFull() );
		}

		private void BuildDictionary()
		{
			actualDictionary.Clear();
			foreach( var pair in dictionary )
			{
				actualDictionary.Add( pair.key, pair.value );
			}
		}
	}
}
