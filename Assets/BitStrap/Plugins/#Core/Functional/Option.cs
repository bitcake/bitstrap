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
		private static readonly bool IsValueType;

		static Option()
		{
			IsValueType = typeof( A ).IsByValue();
		}

		private readonly A value;
		private readonly bool hasValue;

		public bool HasValue
		{
			get { return hasValue && ( IsValueType || value != null ); }
		}

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

		public bool TryGet( out A value )
		{
			if( HasValue )
			{
				value = this.value;
				return true;
			}

			value = default( A );
			return false;
		}

		public B Match<B>( System.Func<A, B> some, System.Func<B> none )
		{
			if( HasValue )
				return some( value );
			return none();
		}

		public A Or( A noneValue )
		{
			if( HasValue )
				return value;

			return noneValue;
		}

		public A Or( System.Func<A> none )
		{
			if( HasValue )
				return value;

			return none();
		}

		public void IfSome( System.Action<A> some )
		{
			if( HasValue )
				some( value );
		}

		public Option<B> Select<B>( System.Func<A, B> select )
		{
			if( !HasValue )
				return new None();

			return select( value );
		}

		public Option<A> Where( System.Predicate<A> predicate )
		{
			if( !HasValue || !predicate( value ) )
				return new None();

			return this;
		}

		public Option<C> SelectMany<B, C>( System.Func<A, Option<B>> func, System.Func<A, B, C> select )
		{
			if( !HasValue )
				return new None();

			var b = func( value );
			if( !b.HasValue )
				return new None();

			return select( value, b.value );
		}
	}
}