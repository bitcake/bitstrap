using UnityEngine;

namespace BitStrap.Examples
{
	public class StringHelperExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!", order = 0 )]
		[HelpBox( "if passed with same values, it won't generate garbage!", HelpBoxAttribute.MessageType.Warning, order = 1 )]
		public string format = "This is a format with the number {0}.";

		public int number = 99;

		[Button]
		public void FormatStringWithNumber()
		{
			string formattedString = StringHelper.Format( format, number );
			Debug.Log( formattedString );
		}
	}
}
