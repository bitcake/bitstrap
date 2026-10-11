using UnityEngine;

namespace BitStrap.Examples
{
	public class FastRandomExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public int seed = 137;

		[ReadOnly]
		public int randomInteger = 0;

		private FastRandom randomGenerator = new FastRandom();

		[Button]
		public void SetSeed()
		{
			randomGenerator.SetSeed( seed );
		}

		[Button]
		public void GenerateRandomIntegerForSeed()
		{
			randomInteger = randomGenerator.GetNextInt();
		}

		private void Awake()
		{
			randomGenerator.SetSeed( seed );
		}
	}
}
