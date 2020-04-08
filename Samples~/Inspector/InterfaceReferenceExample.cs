using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class InterfaceReferenceExample : MonoBehaviour
	{
		[Header( "This field only accepts classes that implement an interface." )]
		[RequireInterface( typeof( IMyInterface ) )]
		public MonoBehaviour myInterfaceReference;

		[Button]
		public void TestMyInterfaceReference()
		{
			var myInterface = myInterfaceReference as IMyInterface;
			myInterface.MyMethod();
		}
	}
}