using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Makes it easy to work with EditorPrefs treating them as properties.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public abstract class EditorPrefProperty<T>
	{
		[SerializeField]
		protected string key;

		private T value;
		private bool initialized = false;

		/// <summary>
		/// Use this property to get/set this editor pref.
		/// </summary>
		public T Value
		{
			get { RetrieveValue(); return value; }
			set { SaveValue( value ); }
		}

		protected EditorPrefProperty( string prefKey )
		{
			key = prefKey;
			value = default( T );
			initialized = false;
		}

		/// <summary>
		/// Deletes the key from the editor prefs and resets the value property.
		/// </summary>
		public void DeleteKey()
		{
			EditorPrefs.DeleteKey( key );
			initialized = false;
		}

		protected void RetrieveValue()
		{
			if( !initialized )
			{
				value = OnRetrieveValue();
				initialized = true;
			}
		}

		protected void SaveValue( T newValue )
		{
			if( AreDifferent( value, newValue ) )
			{
				value = newValue;
				OnSaveValue( value );
			}
		}

		protected abstract T OnRetrieveValue();

		protected abstract void OnSaveValue( T value );

		protected abstract bool AreDifferent( T a, T b );
	}

	/// <summary>
	/// A specialization of EditorPrefProperty for strings.
	/// </summary>
	[System.Serializable]
	public sealed class EditorPrefString : EditorPrefProperty<string>
	{
		private readonly string defaultValue = "";

		public EditorPrefString( string key ) : base( key )
		{
		}

		public EditorPrefString( string key, string defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override string OnRetrieveValue()
		{
			return EditorPrefs.GetString( key, defaultValue );
		}

		protected override void OnSaveValue( string value )
		{
			EditorPrefs.SetString( key, value );
		}

		protected override bool AreDifferent( string a, string b )
		{
			return a != b;
		}
	}

	/// <summary>
	/// A specialization of EditorPrefProperty class for ints.
	/// </summary>
	[System.Serializable]
	public sealed class EditorPrefInt : EditorPrefProperty<int>
	{
		private readonly int defaultValue = 0;

		public EditorPrefInt( string key ) : base( key )
		{
		}

		public EditorPrefInt( string key, int defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override int OnRetrieveValue()
		{
			return EditorPrefs.GetInt( key, defaultValue );
		}

		protected override void OnSaveValue( int value )
		{
			EditorPrefs.SetInt( key, value );
		}

		protected override bool AreDifferent( int a, int b )
		{
			return a != b;
		}
	}

	/// <summary>
	/// A specialization of EditorPrefProperty class for floats.
	/// </summary>
	[System.Serializable]
	public sealed class EditorPrefFloat : EditorPrefProperty<float>
	{
		private readonly float defaultValue = 0.0f;

		public EditorPrefFloat( string key ) : base( key )
		{
		}

		public EditorPrefFloat( string key, float defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override float OnRetrieveValue()
		{
			return EditorPrefs.GetFloat( key, defaultValue );
		}

		protected override void OnSaveValue( float value )
		{
			EditorPrefs.SetFloat( key, value );
		}

		protected override bool AreDifferent( float a, float b )
		{
			return Mathf.Approximately( a, b );
		}
	}

	/// <summary>
	/// A specialization of EditorPrefProperty class for bool.
	/// </summary>
	[System.Serializable]
	public sealed class EditorPrefBool : EditorPrefProperty<bool>
	{
		private readonly bool defaultValue = false;

		public EditorPrefBool( string key ) : base( key )
		{
		}

		public EditorPrefBool( string key, bool defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override bool OnRetrieveValue()
		{
			return EditorPrefs.GetBool( key, defaultValue );
		}

		protected override void OnSaveValue( bool value )
		{
			EditorPrefs.SetBool( key, value );
		}

		protected override bool AreDifferent( bool a, bool b )
		{
			return a != b;
		}
	}
}
