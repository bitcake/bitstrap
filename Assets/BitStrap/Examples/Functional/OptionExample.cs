using UnityEngine;

namespace BitStrap.Examples
{
	public class OptionExample : MonoBehaviour
	{
		[Button]
		public void TestNoOption()
		{
			var a = 1;
			var b = 2;

			var r = a + b;

			Debug.Log( r );
		}

		[Button]
		public void TestWithOption()
		{
			var a = Option.Some( 1 );
			var b = Option.Some( 2 );

			var r = from x in a
					from y in b
					select x + y;

			int rValue;
			if( r.TryGet( out rValue ) )
				Debug.Log( rValue );
			else
				Debug.Log( "No Value" );
		}

		[Button]
		public void TestWithOptionAndNone()
		{
			Option<int> a = 1;
			Option<int> b = new None();

			var r = from x in a
					from y in b
					select x + y;

			int rValue;
			if( r.TryGet( out rValue ) )
				Debug.Log( rValue );
			else
				Debug.Log( "No Value" );
		}

		[Button]
		public void TestWithNull()
		{
			var a = Option.Some( ( object ) null );
			Debug.Log( "Has value? " + a.HasValue );
		}
	}
}