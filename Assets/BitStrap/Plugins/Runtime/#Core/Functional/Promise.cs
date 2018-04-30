using System.Collections.Generic;

namespace BitStrap
{
	public sealed class Promise<A>
	{
		private Option<A> result;
		private readonly SafeAction<A> onComplete = new SafeAction<A>();

		public Option<A> Result
		{
			get { return result; }
		}

		public void Then( System.Action<A> callback )
		{
			A value;
			if( result.TryGet( out value ) )
				callback( value );
			else
				onComplete.Register( callback );
		}

		public void Complete( A value )
		{
			result = value;
			onComplete.Call( value );
			onComplete.UnregisterAll();
		}

		public Promise<B> Select<B>( System.Func<A, B> select )
		{
			var pb = new Promise<B>();
			Then( a => pb.Complete( select( a ) ) );

			return pb;
		}

		public Promise<A> Where( System.Predicate<A> predicate )
		{
			var p = new Promise<A>();
			Then( a =>
			{
				if( predicate( a ) )
					p.Complete( a );
			} );

			return p;
		}

		public Promise<C> SelectMany<B, C>( System.Func<A, Promise<B>> func, System.Func<A, B, C> select )
		{
			var pc = new Promise<C>();
			Then( a =>
			{
				var pb = func( a );
				pb.Then( b => pc.Complete( select( a, b ) ) );
			} );

			return pc;
		}
	}
}