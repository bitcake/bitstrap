using UnityEngine;

namespace BitStrap.Examples
{
	public class StringExtensionsExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public string stringA = "car";

		public string stringB = "cars";

		public string camelCaseString = "CamelCaseString";
		public string camelCaseSeparator = ", ";

		[Button]
		public void CalculateDistanceBetweenStringsAAndB()
		{
			Debug.LogFormat( "The amount of changes made in '{0}' to reach '{1}' is {2}", stringA, stringB, stringA.Distance( stringB ) );
		}

		[Button]
		public void SeparateCamelCaseInTestString()
		{
			Debug.Log( camelCaseString.SeparateCamelCase( camelCaseSeparator ) );
		}
	}
}
