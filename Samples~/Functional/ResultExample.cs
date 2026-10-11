using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class ResultExample : MonoBehaviour
	{
		public sealed class ErrorExample
		{
		}

		[Button]
		public void TestNoResult()
		{
			var a = 1;
			var b = 2;

			var r = a + b;

			Debug.Log( r );
		}

		[Button]
		public void TestWithResult()
		{
			Result<int, ErrorExample> a = 1;
			Result<int, ErrorExample> b = 2;

			var r = from x in a
					from y in b
					select x + y;

			r.Match(
				ok: v => Debug.Log( v ),
				error: e => Debug.Log( "Error" )
			);
		}

		[Button]
		public void TestWithResultAndError()
		{
			Result<int, ErrorExample> a = 1;
			Result<int, ErrorExample> b = new ErrorExample();

			var r = from x in a
					from y in b
					select x + y;

			r.Match(
				ok: v => Debug.Log( v ),
				error: e => Debug.Log( "Error" )
			);
		}
	}
}