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

		public static Option<T> Some<T>( T value )
		{
			return new Option<T>( value );
		}

		public static void IfSome<A, B>( Option<A> a, Option<B> b, System.Action<A, B> predicate )
		{
			A valueA;
			if( !a.TryGet( out valueA ) )
				return;

			B valueB;
			if( !b.TryGet( out valueB ) )
				return;

			predicate( valueA, valueB );
		}

		public static void IfSome<A, B, C>( Option<A> a, Option<B> b, Option<C> c, System.Action<A, B, C> predicate )
		{
			A valueA;
			if( !a.TryGet( out valueA ) )
				return;

			B valueB;
			if( !b.TryGet( out valueB ) )
				return;

			C valueC;
			if( !c.TryGet( out valueC ) )
				return;

			predicate( valueA, valueB, valueC );
		}

		public static void IfSome<A, B, C, D>( Option<A> a, Option<B> b, Option<C> c, Option<D> d, System.Action<A, B, C, D> predicate )
		{
			A valueA;
			if( !a.TryGet( out valueA ) )
				return;

			B valueB;
			if( !b.TryGet( out valueB ) )
				return;

			C valueC;
			if( !c.TryGet( out valueC ) )
				return;

			D valueD;
			if( !d.TryGet( out valueD ) )
				return;

			predicate( valueA, valueB, valueC, valueD );
		}
	}

	public struct Option<A>
	{
		private readonly A value;
		private readonly bool isSome;

		public bool IsSome
		{
			get { return isSome && value != null; }
		}

		public Option( A value )
		{
			this.value = value;
			isSome = value != null;
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

			return new Unit();
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

		public Unit IfSome( System.Action<A> onSome )
		{
			if( IsSome )
				onSome( value );

			return new Unit();
		}

		public Option<B> And<B>( Option<B> other )
		{
			if( IsSome )
				return other;

			return new None();
		}

		public Option<B> AndThen<B>( System.Func<A, Option<B>> onOther )
		{
			if( IsSome )
				return onOther( value );

			return new None();
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

			return new None();
		}

		public Option<A> Where( System.Predicate<A> predicate )
		{
			if( IsSome && predicate( value ) )
				return this;

			return new None();
		}

		public Option<C> SelectMany<B, C>( System.Func<A, Option<B>> func, System.Func<A, B, C> select )
		{
			if( !IsSome )
				return new None();

			var b = func( value );
			if( !b.IsSome )
				return new None();

			return select( value, b.value );
		}
	}
}