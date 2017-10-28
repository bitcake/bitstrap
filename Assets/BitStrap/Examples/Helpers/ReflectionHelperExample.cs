using UnityEngine;

namespace BitStrap.Examples
{
	public class ReflectionHelperExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public int intValue1 = 1;

		public int intValue2 = 2;
		public int intValue3 = 3;

		public string stringValue1 = "string1";
		public string stringValue2 = "string2";

		[Button]
		public void ListAllIntegerFields()
		{
			int[] allIntValues = ReflectionHelper.GetFieldValuesOfType<int>( this );
			Debug.LogFormat( "All Integer values: {0}", allIntValues.ToStringFull() );
		}

		[Button]
		public void ListAllStringFields()
		{
			string[] allStringValues = ReflectionHelper.GetFieldValuesOfType<string>( this );
			Debug.LogFormat( "All String values: {0}", allStringValues.ToStringFull() );
		}

		[Button]
		public void ListAllBooleanFields()
		{
			bool[] allBooleanValues = ReflectionHelper.GetFieldValuesOfType<bool>( this );
			Debug.LogFormat( "All Boolean values: {0}", allBooleanValues.ToStringFull() );
		}
	}
}
