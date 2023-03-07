using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class DummyScriptableObjectExample : ScriptableObject
	{
		public float floatValue;
		public int intValue;
		[Space]
		public string stringValue;

		[Header("Header")]
		[Range(0, 100)]
		public float rangeExample;
		[Unit("m/s2")]
		public float unitOfAcceleration = 5.0f;
	}
}
