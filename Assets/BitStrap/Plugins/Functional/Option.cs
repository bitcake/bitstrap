namespace BitStrap
{
	public struct None
	{
	}

	public static class Option
	{
		public static Option<T> Some<T>( T value )
		{
			return new Option<T>( value );
		}
	}

	public struct Option<A>
	{
		private static readonly bool IsValueType;

		static Option()
		{
			IsValueType = typeof( A ).IsByValue();
		}

		private readonly A value;

		public readonly bool hasValue;

		public Option( A value )
		{
			this.value = value;
			hasValue = IsValueType || value != null;
		}

		public static implicit operator Option<A>( A value )
		{
			return new Option<A>( value );
		}

		public static implicit operator Option<A>( None value )
		{
			return default( Option<A> );
		}

		public bool TryGet( out A retValue )
		{
			if( hasValue )
			{
				retValue = value;
				return true;
			}

			retValue = default( A );
			return false;
		}

		public B Match<B>( System.Func<A, B> onSome, System.Func<B> onNone )
		{
			if( hasValue )
				return onSome( value );
			return onNone();
		}

		public A Or( A orValue )
		{
			if( hasValue )
				return value;

			return orValue;
		}

		public A Or( System.Func<A> onNone )
		{
			if( hasValue )
				return value;

			return onNone();
		}

		public Option<B> Select<B>( System.Func<A, B> select )
		{
			if( !hasValue )
				return new None();

			return select( value );
		}

		public Option<A> Where( System.Predicate<A> predicate )
		{
			if( !hasValue || !predicate( value ) )
				return new None();

			return this;
		}

		public Option<C> SelectMany<B, C>( System.Func<A, Option<B>> func, System.Func<A, B, C> select )
		{
			if( !hasValue )
				return new None();

			var b = func( value );
			if( !b.hasValue )
				return new None();

			return select( value, b.value );
		}
	}
}