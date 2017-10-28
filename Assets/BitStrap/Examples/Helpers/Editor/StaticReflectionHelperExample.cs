using UnityEditor;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open this window by navigating in Unity Editor to "Window/BitStrap Examples/Extensions/StaticReflectionHelper".
	/// </summary>
	public class StaticReflectionHelperExample : EditorWindow
	{
		public class TestClass
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
				string memberName = StaticReflectionHelper.GetMemberName<TestClass>( c => c.testField );
				Debug.Log( memberName );
			}

			if( GUILayout.Button( "Get TestClass property name" ) )
			{
				string memberName = StaticReflectionHelper.GetMemberName<TestClass>( c => c.testProperty );
				Debug.Log( memberName );
			}

			if( GUILayout.Button( "Get TestClass static field name" ) )
			{
				string memberName = StaticReflectionHelper.GetMemberName( () => TestClass.staticTestField );
				Debug.Log( memberName );
			}

			if( GUILayout.Button( "Get TestClass static property name" ) )
			{
				string memberName = StaticReflectionHelper.GetMemberName( () => TestClass.staticTestProperty );
				Debug.Log( memberName );
			}

			GUILayout.Label( "Methods" );

			if( GUILayout.Button( "Get TestClass method name" ) )
			{
				string memberName = StaticReflectionHelper.GetMemberName<TestClass>( c => c.TestMethod() );
				var methodInfo = StaticReflectionHelper.GetMethod<TestClass>( c => c.TestMethod() );
				Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name );
			}

			if( GUILayout.Button( "Get TestClass no return method name" ) )
			{
				string memberName = StaticReflectionHelper.GetMemberName<TestClass>( c => c.TestMethodNoReturn() );
				var methodInfo = StaticReflectionHelper.GetMethod<TestClass>( c => c.TestMethodNoReturn() );
				Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name );
			}

			if( GUILayout.Button( "Get TestClass static methods name" ) )
			{
				string memberName = StaticReflectionHelper.GetMemberName( () => TestClass.StaticTestMethod() );
				var methodInfo = StaticReflectionHelper.GetMethod( () => TestClass.StaticTestMethod() );
				Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name );
			}

			if( GUILayout.Button( "Get TestClass no return static methods name" ) )
			{
				string memberName = StaticReflectionHelper.GetMemberName( () => TestClass.StaticTestMethodNoReturn() );
				var methodInfo = StaticReflectionHelper.GetMethod( () => TestClass.StaticTestMethodNoReturn() );
				Debug.Log( memberName + ", MethodInfo.Name: " + methodInfo.Name );
			}
		}
	}
}
