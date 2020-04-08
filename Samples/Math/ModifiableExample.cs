using UnityEngine;

namespace BitStrap.Examples
{
	public class ModifiableExample : MonoBehaviour
	{
		public ModifiableInt modifiableInt = new ModifiableInt( 10, ( a, b ) => a + b );

		public int modifier = 2;

		[Button]
		public void PrintModifiableInt()
		{
			if( Application.isPlaying )
			{
				Print();
			}
			else
			{
				Debug.LogWarning( "In order to see Modifiable working, please enter Play mode." );
			}
		}

		[Button]
		public void SetModifier()
		{
			if( Application.isPlaying )
			{
				modifiableInt.SetModifier( this, modifier );
				Print();
			}
			else
			{
				Debug.LogWarning( "In order to see Modifiable working, please enter Play mode." );
			}
		}

		[Button]
		public void RemoveModifier()
		{
			if( Application.isPlaying )
			{
				modifiableInt.RemoveModifier( this );
				Print();
			}
			else
			{
				Debug.LogWarning( "In order to see Modifiable working, please enter Play mode." );
			}
		}

		private void Print()
		{
			Debug.LogFormat( "Modified value: {0}; Original value {0}", modifiableInt.ModifiedValue, modifiableInt.OriginalValue );
		}
	}
}
