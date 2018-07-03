namespace BitStrap
{
	public struct None
	{
	}

	public static class Option
	{
		public sealed class UnwrapNoneException : System.Exception
		{
		}
	}

	public struct Option<A>
	{
		private readonly static bool IsValue;

		private readonly A value;
		private readonly bool isSome;

		public bool IsSome
		{
			get { return isSome && ( IsValue || !ReferenceEquals( value, null ) && !value.Equals( null ) ); }
		}

		static Option()
		{
			IsValue = typeof( A ).IsValueType;
		}

		public Option( A value )
		{
			this.value = value;
			isSome = true;
			isSome = IsSome;
		}

		public static implicit operator Option<A>( A value )
		{
			return new Option<A>( value );
		}

		public static implicit operator Option<A>( None value )
		{
			return default( Option<A> );
		}

		public bool TryGet( out A value )
		{
			if( IsSome )
			{
				value = this.value;
				return true;
			}

			value = default( A );
			return false;
		}

		public B Match<B>( System.Func<A, B> some, System.Func<B> none )
		{
			if( IsSome )
				return some( value );

			return none();
		}

		public Unit Match( System.Action<A> some, System.Action none )
		{
			if( IsSome )
				some( value );
			else
				none();

			return Functional.Unit;
		}

		public Result<A, E> OkOr<E>( E error )
		{
			if( IsSome )
				return new Result<A, E>( value );

			return new Result<A, E>( error );
		}

		public Result<A, E> OkOrElse<E>( System.Func<E> onError )
		{
			if( IsSome )
				return new Result<A, E>( value );

			return new Result<A, E>( onError() );
		}

		public A Unwrap()
		{
			if( IsSome )
				return value;

			throw new Option.UnwrapNoneException();
		}

		public A UnwrapOr( A noneValue )
		{
			if( IsSome )
				return value;

			return noneValue;
		}

		public A UnwrapOrElse( System.Func<A> onNone )
		{
			if( IsSome )
				return value;

			return onNone();
		}

		public bool IfSome( System.Action<A> onSome )
		{
			if( IsSome )
			{
				onSome( value );
				return true;
			}

			return false;
		}

		public Option<B> And<B>( Option<B> other )
		{
			if( IsSome )
				return other;

			return default( Option<B> );
		}

		public Option<B> AndThen<B>( System.Func<A, Option<B>> onOther )
		{
			if( IsSome )
				return onOther( value );

			return default( Option<B> );
		}

		public Option<A> Or( Option<A> other )
		{
			if( IsSome )
				return this;

			return other;
		}

		public Option<A> OrElse( System.Func<Option<A>> onOther )
		{
			if( IsSome )
				return this;

			return onOther();
		}

		public Option<B> Select<B>( System.Func<A, B> select )
		{
			if( IsSome )
				return select( value );

			return default( Option<B> );
		}

		public Option<A> Where( System.Predicate<A> predicate )
		{
			if( IsSome && predicate( value ) )
				return this;

			return default( Option<A> );
		}

		public Option<C> SelectMany<B, C>( System.Func<A, Option<B>> func, System.Func<A, B, C> select )
		{
			if( !IsSome )
				return default( Option<C> );

			var b = func( value );
			if( !b.IsSome )
				return default( Option<C> );

			return select( value, b.value );
		}
	}
}