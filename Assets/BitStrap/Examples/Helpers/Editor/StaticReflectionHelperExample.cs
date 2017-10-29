using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open this window by navigating in Unity Editor to "Window/BitStrap Examples/Extensions/StaticReflectionHelper".
	/// </summary>
	public sealed class StaticReflectionHelperExample : EditorWindow
	{
		public sealed class TestClass
		{
			public static int staticTestField;
			public int testField;
			public static int staticTestProperty { get; set; }
			public int testProperty { get; set; }

			public static void StaticTestMethodNoReturn()
			{
			}

			public static int StaticTestMethod()
			{
				return 0;
			}

			public void TestMethodNoReturn()
			{
			}

			public int TestMethod()
			{
				return 0;
			}
		}

		[MenuItem( "Window/BitStrap Examples/Helpers/StaticReflectionHelper" )]
		public static void CreateWindow()
		{
			GetWindow<StaticReflectionHelperExample>().Show();
		}

		private void OnGUI()
		{
			GUILayout.Label( "Members" );

			if( GUILayout.Button( "Get TestClass field name" ) )
			{
				string memberName;
				if( StaticReflectionHelper.GetMemberName<TestClass>( c => c.testField ).TryGet( out memberName ) )
					Debug.Log( memberName );
			}

			if( GUILayout.Button( "Get TestClass property name" ) )
			{
				string memberName;
				if( StaticReflectionHelper.GetMemberName<TestClass>( c => c.testProperty ).TryGet( out memberName ) )
					Debug.Log( memberName );
			}

			if( GUILayout.Button( "Get TestClass static field name" ) )
			{
				string memberName;
				if( StaticReflectionHelper.GetMemberName( () => TestClass.staticTestField ).TryGet( out memberName ) )
					Debug.Log( memberName );
			}

			if( GUILayout.Button( "Get TestClass static property name" ) )
			{
				string memberName;
				if( StaticReflectionHelper.GetMemberName( () => TestClass.staticTestProperty ).TryGet( out memberName ) )
					Debug.Log( memberName );
			}

			GUILayout.Label( "Methods" );

			if( GUILayout.Button( "Get TestClass method name" ) )
			{
				Option.IfSome(
					StaticReflectionHelper.GetMemberName<TestClass>( c => c.TestMethod() ),
					StaticReflectionHelper.GetMethod<TestClass>( c => c.TestMethod() ),
					( memberName, methodInfo ) => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name )
				);
			}

			if( GUILayout.Button( "Get TestClass no return method name" ) )
			{
				Option.IfSome(
					StaticReflectionHelper.GetMemberName<TestClass>( c => c.TestMethodNoReturn() ),
					StaticReflectionHelper.GetMethod<TestClass>( c => c.TestMethodNoReturn() ),
					( memberName, methodInfo ) => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name )
				);
			}

			if( GUILayout.Button( "Get TestClass static methods name" ) )
			{
				Option.IfSome(
					StaticReflectionHelper.GetMemberName( () => TestClass.StaticTestMethod() ),
					StaticReflectionHelper.GetMethod( () => TestClass.StaticTestMethod() ),
					( memberName, methodInfo ) => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name )
				);
			}

			if( GUILayout.Button( "Get TestClass no return static methods name" ) )
			{
				Option.IfSome(
					StaticReflectionHelper.GetMemberName( () => TestClass.StaticTestMethodNoReturn() ),
					StaticReflectionHelper.GetMethod( () => TestClass.StaticTestMethodNoReturn() ),
					( memberName, methodInfo ) => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name )
				);
			}
		}

		private static None DebugLog( string message )
		{
			Debug.Log( message );
			return new None();
		}
	}
}
