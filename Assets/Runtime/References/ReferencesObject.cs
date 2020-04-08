using UnityEngine;

namespace BitStrap
{
	public class ReferencesObjectBase : ScriptableObject
	{
		public Object rootFolder;

#if UNITY_EDITOR
		public virtual System.Type ReferencedType { get { return typeof( Object ); } }
		public virtual int ReferenceCount { get { return 0; } }
		public virtual bool ContainsNullReference { get { return false; } }

		public virtual void UpdateReferences()
		{
		}
#endif
	}

	public class ReferencesObject<T> : ReferencesObjectBase where T : Object
	{
		public T[] references = new T[0];

		public Option<T> FindByName( string name )
		{
			foreach( var reference in references )
			{
				if( reference.name == name )
					return reference;
			}

			return Functional.None;
		}

#if UNITY_EDITOR
		public override System.Type ReferencedType { get { return typeof( T ); } }
		public override int ReferenceCount { get { return references.Length; } }
		public override bool ContainsNullReference { get { return references.Any( r => r == null ); } }

		public override void UpdateReferences()
		{
			if( rootFolder == null )
				references = ReferencesHelper.GetReferencesInProject<T>();
			else
				references = ReferencesHelper.GetReferencesInFolder<T>( rootFolder );
		}
#endif
	}
}