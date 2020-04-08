using System.Collections;
using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class PromiseExample : MonoBehaviour
	{
		[Button]
		public void Wait1SecondAndPrintResult()
		{
			if( !Application.isPlaying )
			{
				Debug.LogWarning( "In order to see Promise working, please enter Play mode." );
				return;
			}

			Debug.Log( "Creating a promise" );

			var promise = new Promise<string>();
			promise.Then( v =>
				Debug.LogFormat( "After promise completion, got value '{0}'", v )
			);

			AfterSeconds( 1.0f, () =>
			{
				promise.Complete( "This is a promised value" );
			} );
		}

		[Button]
		public void Wait1SecondAndPrintResultWithLinq()
		{
			if( !Application.isPlaying )
			{
				Debug.LogWarning( "In order to see Promise working, please enter Play mode." );
				return;
			}

			Debug.Log( "Creating a promise" );

			var promise = new Promise<string>();

			Functional.Ignore =
				from a in promise
				select Functional.Do( () =>
					Debug.LogFormat( "After promise completion, got value '{0}'", a )
				);

			AfterSeconds( 1.0f, () =>
			{
				promise.Complete( "This is a promised value" );
			} );
		}

		[Button]
		public void OneWait1SecAndOther2SecsThenPrintResult()
		{
			if( !Application.isPlaying )
			{
				Debug.LogWarning( "In order to see Promise working, please enter Play mode." );
				return;
			}

			Debug.Log( "Creating promises" );

			var promiseA = new Promise<string>();
			var promiseB = new Promise<int>();

			Functional.Ignore =
				from a in promiseA
				from b in promiseB
				select Functional.Do( () =>
					Debug.LogFormat( "After all promise completion, got values '{0}' and '{1}'", a, b )
				);

			AfterSeconds( 2.0f, () =>
			{
				promiseA.Complete( "This is a promised value" );
			} );

			AfterSeconds( 1.0f, () =>
			{
				promiseB.Complete( 17 );
			} );
		}

		private void AfterSeconds( float seconds, System.Action callback )
		{
			StartCoroutine( AfterSecondsCoroutine( seconds, callback ) );
		}

		private IEnumerator AfterSecondsCoroutine( float seconds, System.Action callback )
		{
			yield return new WaitForSeconds( seconds );
			callback();
		}
	}
}