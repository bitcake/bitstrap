using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class AttributesExample : MonoBehaviour
	{
		[Header( "This is an example of the custom attributes in BitStrap." )]
		[LayerSelector]
		public int selectedLayer;

		[TagSelector]
		public string selectedTag;

		[ReadOnly]
		public int readOnlyInt = 7;

		[ReadOnly( onlyInPlaymode = true )]
		public int readOnlyIntOnlyInPlaymode = 7;

		[HelpBox( "This is a HelpBox.", HelpBoxAttribute.MessageType.Warning )]
		public int fieldWithHelpBox = 2;

		[Unit( "m/s" )]
		public float velocityWithUnit = 5.0f;

		[FolderPath]
		public string relativeFolderPath = string.Empty;

		[FolderPath( false )]
		public string absoluteFolderPath = string.Empty;

		[ExponentInfo]
		public long largeNumber = 1000000000L;

		public GameObject nullGameObjectReference;

		[Nullable]
		public GameObject nullableGameObjectReference;

		public DummyScriptableObjectExample scriptableObject;

		[InlineScriptableObject]
		public DummyScriptableObjectExample scriptableObjectWithAttribute;

		[Button]
		public void ButtonTest()
		{
			Debug.Log( "You pressed the button and executed a method." );
		}
	}
}