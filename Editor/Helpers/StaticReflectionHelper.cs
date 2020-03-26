using System.Linq.Expressions;
using System.Reflection;

namespace BitStrap
{
	/// <summary>
	/// Bunch of static reflection helper methods. These are generally used to get some class member's name
	/// without the use of magic strings. I.e. obj.GetType().GetMember( "MemeberName" );
	/// </summary>
	public static class StaticReflectionHelper
	{
		/// <summary>
		/// Get a class member's name without the use of magic strings.
		/// Static member and method version.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<string> GetMemberName( Expression<System.Func<object>> expression )
		{
			return GetMemberName( expression.Body );
		}

		/// <summary>
		/// Get a class member's name without the use of magic strings.
		/// Instance member and method version.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<string> GetMemberName<T>( Expression<System.Func<T, object>> expression )
		{
			return GetMemberName( expression.Body );
		}

		/// <summary>
		/// Get a class member's name without the use of magic strings.
		/// Static method with no return version.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<string> GetMemberName( Expression<System.Action> expression )
		{
			return GetMemberName( expression.Body );
		}

		/// <summary>
		/// Get a class member's name without the use of magic strings.
		/// Instance method with no return version.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<string> GetMemberName<T>( Expression<System.Action<T>> expression )
		{
			return GetMemberName( expression.Body );
		}

		/// <summary>
		/// Get a class method without the use of magic strings.
		/// Static method version.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<MethodInfo> GetMethod( Expression<System.Func<object>> expression )
		{
			return GetMethod( expression.Body );
		}

		/// <summary>
		/// Get a class method without the use of magic strings.
		/// Instance method version.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<MethodInfo> GetMethod<T>( Expression<System.Func<T, object>> expression )
		{
			return GetMethod( expression.Body );
		}

		/// <summary>
		/// Get a class method without the use of magic strings.
		/// Static method with no return version.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<MethodInfo> GetMethod( Expression<System.Action> expression )
		{
			return GetMethod( expression.Body );
		}

		/// <summary>
		/// Get a class method without the use of magic strings.
		/// Instance method with no return version.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Option<MethodInfo> GetMethod<T>( Expression<System.Action<T>> expression )
		{
			return GetMethod( expression.Body );
		}

		private static Option<string> GetMemberName( Expression expression )
		{
			var memberExpression = expression as MemberExpression;
			if( memberExpression != null )
				return memberExpression.Member.Name;

			var methodCallExpression = expression as MethodCallExpression;
			if( methodCallExpression != null )
				return methodCallExpression.Method.Name;

			var unaryExpression = expression as UnaryExpression;
			if( unaryExpression != null )
				return GetMemberName( unaryExpression.Operand );

			return Functional.None;
		}

		private static Option<MethodInfo> GetMethod( Expression expression )
		{
			var methodCallExpression = expression as MethodCallExpression;
			if( methodCallExpression != null )
				return methodCallExpression.Method;

			var unaryExpression = expression as UnaryExpression;
			if( unaryExpression != null )
				return GetMethod( unaryExpression.Operand );

			return Functional.None;
		}
	}
}
