namespace BitStrap
{
	/// <summary>
	/// Put this attribute above one of your MonoBehaviour method and it will draw
	/// a button in the inspector automatically.
	///
	/// NOTE: the method must not have any params and can not be static.
	///
	/// <code>
	/// <para>[Button]</para>
	/// <para>public void MyMethod()</para>
	/// <para>{</para>
	/// <para>    Debug.Log( "HELLO HELLO HELLO!!" );</para>
	/// <para>}</para>
	/// </code>
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Method )]
	public class ButtonAttribute : System.Attribute
	{
	}
}
