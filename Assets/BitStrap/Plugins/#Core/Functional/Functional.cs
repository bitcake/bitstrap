namespace BitStrap
{
	public static class Functional
	{
		public static readonly Unit Unit = default( Unit );
		public static readonly None None = default( None );

		public static object Ignore
		{
			set { }
		}

		public static Unit Do( System.Action callback )
		{
			callback();
			return Unit;
		}

		public static T Do<T>( System.Func<T> callback )
		{
			return callback();
		}
	}
}