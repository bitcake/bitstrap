using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class DummySingleton : Singleton<DummySingleton>
	{
		public int dummyIntField = 8;
	}
}
