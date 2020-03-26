namespace BitStrap
{
	public static class Result
	{
		public sealed class UnwrapErrorException : System.Exception
		{
		}
	}

	public struct Result<A, E>
	{
		private readonly A value;
		private readonly E error;

		public readonly bool isOk;

		public Option<A> Ok
		{
			get { return isOk ? new Option<A>( value ) : new Option<A>(); }
		}

		public Option<E> Error
		{
			get { return !isOk ? new Option<E>( error ) : new Option<E>(); }
		}

		public Result( A value )
		{
			this.value = value;
			error = default( E );
			isOk = true;
		}

		public Result( E error )
		{
			value = default( A );
			this.error = error;
			isOk = false;
		}

		public static implicit operator Result<A, E>( A value )
		{
			return new Result<A, E>( value );
		}

		public static implicit operator Result<A, E>( E error )
		{
			return new Result<A, E>( error );
		}

		public B Match<B>( System.Func<A, B> ok, System.Func<E, B> error )
		{
			if( isOk )
				return ok( value );

			return error( this.error );
		}

		public Unit Match( System.Action<A> ok, System.Action<E> error )
		{
			if( isOk )
				ok( value );
			else
				error( this.error );

			return Functional.Unit;
		}

		public A Unwrap()
		{
			if( isOk )
				return value;

			throw new Result.UnwrapErrorException();
		}

		public A UnwrapOr( A errorValue )
		{
			if( isOk )
				return value;

			return errorValue;
		}

		public A UnwrapOrElse( System.Func<E, A> onError )
		{
			if( isOk )
				return value;

			return onError( error );
		}

		public Unit IfOk( System.Action<A> onOk )
		{
			if( isOk )
				onOk( value );

			return Functional.Unit;
		}

		public Result<B, E> And<B>( Result<B, E> other )
		{
			if( isOk )
				return other;

			return new Result<B, E>( error );
		}

		public Result<B, E> AndThen<B>( System.Func<A, Result<B, E>> onOther )
		{
			if( isOk )
				return onOther( value );

			return new Result<B, E>( error );
		}

		public Result<A, E> Or( Result<A, E> other )
		{
			if( isOk )
				return this;

			return other;
		}

		public Result<A, E> OrElse( System.Func<E, Result<A, E>> onOther )
		{
			if( isOk )
				return this;

			return onOther( error );
		}

		public Result<B, E> Select<B>( System.Func<A, B> select )
		{
			if( isOk )
				return select( value );

			return new Result<B, E>( error );
		}

		public Result<A, E> Where( System.Predicate<A> predicate )
		{
			if( isOk && predicate( value ) )
				return this;

			return new Result<A, E>( error );
		}

		public Result<C, E> SelectMany<B, C>( System.Func<A, Result<B, E>> func, System.Func<A, B, C> select )
		{
			if( !isOk )
				return new Result<C, E>( error );

			var b = func( value );
			if( !b.isOk )
				return new Result<C, E>( error );

			return select( value, b.value );
		}
	}
}
