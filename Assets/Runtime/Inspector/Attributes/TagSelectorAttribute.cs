using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Put this attribute above an int field and it will draw like a tag picker in the inspector.
    /// </summary>
    [System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false )]
    public class TagSelectorAttribute : PropertyAttribute
    {
    }
}
