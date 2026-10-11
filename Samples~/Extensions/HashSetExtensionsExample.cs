using System.Collections.Generic;
using UnityEngine;

namespace BitStrap.Examples
{
	public class HashSetExtensionsExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public string[] hashSet = new string[] {
			"element0",
			"element1",
			"element2",
			"element3",
		};

		private HashSet<string> actualHashSet = new HashSet<string>();

		[Button]
		public void ForEachIterationWithNoGarbage()
		{
			BuildHashSet();
			foreach( var element in actualHashSet.Iter() )
			{
				Debug.LogFormat( "This is an iteration. Element = {0}", element );
			}
		}

		[Button]
		public void EmptyElementsCount()
		{
			BuildHashSet();
			Debug.LogFormat( "There are {0} empty elements in the dictionary.", actualHashSet.Count( e => string.IsNullOrEmpty( e ) ) );
		}

		[Button]
		public void AreAllEmpty()
		{
			BuildHashSet();
			Debug.LogFormat( "Are all empty elements? {0}.", actualHashSet.All( e => string.IsNullOrEmpty( e ) ) );
		}

		[Button]
		public void IsThereAnyEmptyElement()
		{
			BuildHashSet();
			Debug.LogFormat( "Is there any empty element in hashset? {0}.", actualHashSet.Any( e => string.IsNullOrEmpty( e ) ) );
		}

		[Button]
		public void GetFirstElementOrDefaultValue()
		{
			BuildHashSet();
			Debug.LogFormat( "First element or 'None' is {0}.", actualHashSet.First().UnwrapOr( "None" ) );
		}

		[Button]
		public void PrettyPrintHashSet()
		{
			BuildHashSet();
			Debug.Log( actualHashSet.ToStringFull() );
		}

		private void BuildHashSet()
		{
			actualHashSet.Clear();
			foreach( var element in hashSet )
			{
				actualHashSet.Add( element );
			}
		}
	}
}