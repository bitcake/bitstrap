using UnityEngine;

namespace BitStrap.Examples
{
	public interface IMyInterface
	{
		void MyMethod();
	}

	public interface ISomeInterface { }

	public interface IAnotherInterface { }

	public interface IOneMoreInterface { }

	public sealed class MyClassThaImplementsMyInterface : MonoBehaviour, IMyInterface, ISomeInterface, IAnotherInterface, IOneMoreInterface
	{
		[ShowImplementedInterfaces( typeof( MyClassThaImplementsMyInterface ) )]
		public int dummyField;

		void IMyInterface.MyMethod()
		{
			Debug.Log( "This was called through IMyInterface" );
		}
	}
}