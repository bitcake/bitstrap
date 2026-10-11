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
				Functional.Ignore =
					from memberName in StaticReflectionHelper.GetMemberName<TestClass>( c => c.TestMethod() )
					from methodInfo in StaticReflectionHelper.GetMethod<TestClass>( c => c.TestMethod() )
					select Functional.Do( () => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name ) );
			}

			if( GUILayout.Button( "Get TestClass no return method name" ) )
			{
				Functional.Ignore =
					from memberName in StaticReflectionHelper.GetMemberName<TestClass>( c => c.TestMethodNoReturn() )
					from methodInfo in StaticReflectionHelper.GetMethod<TestClass>( c => c.TestMethodNoReturn() )
					select Functional.Do( () => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name ) );
			}

			if( GUILayout.Button( "Get TestClass static methods name" ) )
			{
				Functional.Ignore =
					from memberName in StaticReflectionHelper.GetMemberName( () => TestClass.StaticTestMethod() )
					from methodInfo in StaticReflectionHelper.GetMethod( () => TestClass.StaticTestMethod() )
					select Functional.Do( () => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name ) );
			}

			if( GUILayout.Button( "Get TestClass no return static methods name" ) )
			{
				Functional.Ignore =
					from memberName in StaticReflectionHelper.GetMemberName( () => TestClass.StaticTestMethodNoReturn() )
					from methodInfo in StaticReflectionHelper.GetMethod( () => TestClass.StaticTestMethodNoReturn() )
					select Functional.Do( () => Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name ) );
			}
		}
	}
}
