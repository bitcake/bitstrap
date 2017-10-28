using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Makes it easy to work with PlayerPrefs treating them as properties.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public abstract class PlayerPrefProperty<T>
	{
		[SerializeField]
		protected string key;

		private T value;
		private bool initialized = false;

		/// <summary>
		/// Use this property to get/set this Player pref.
		/// </summary>
		public T Value
		{
			get { RetrieveValue(); return value; }
			set { SaveValue( value ); }
		}

		protected PlayerPrefProperty( string prefKey )
		{
			key = prefKey;
			value = default( T );
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
			value = newValue;
			OnSaveValue( value );
		}

		protected abstract T OnRetrieveValue();

		protected abstract void OnSaveValue( T value );
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty for strings.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefString : PlayerPrefProperty<string>
	{
		private string defaultValue = "";

		public PlayerPrefString( string key ) : base( key )
		{
		}

		public PlayerPrefString( string key, string defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override string OnRetrieveValue()
		{
			return PlayerPrefs.GetString( key, defaultValue );
		}

		protected override void OnSaveValue( string value )
		{
			PlayerPrefs.SetString( key, value );
		}
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty class for ints.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefInt : PlayerPrefProperty<int>
	{
		private int defaultValue = 0;

		public PlayerPrefInt( string key ) : base( key )
		{
		}

		public PlayerPrefInt( string key, int defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override int OnRetrieveValue()
		{
			return PlayerPrefs.GetInt( key, defaultValue );
		}

		protected override void OnSaveValue( int value )
		{
			PlayerPrefs.SetInt( key, value );
		}
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty class for floats.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefFloat : PlayerPrefProperty<float>
	{
		private float defaultValue = 0.0f;

		public PlayerPrefFloat( string key ) : base( key )
		{
		}

		public PlayerPrefFloat( string key, float defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override float OnRetrieveValue()
		{
			return PlayerPrefs.GetFloat( key, defaultValue );
		}

		protected override void OnSaveValue( float value )
		{
			PlayerPrefs.SetFloat( key, value );
		}
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty class for bool.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefBool : PlayerPrefProperty<bool>
	{
		private bool defaultValue = false;

		public PlayerPrefBool( string key ) : base( key )
		{
		}

		public PlayerPrefBool( string key, bool defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override bool OnRetrieveValue()
		{
			return PlayerPrefs.GetInt( key, defaultValue ? 1 : 0 ) != 0;
		}

		protected override void OnSaveValue( bool value )
		{
			PlayerPrefs.SetInt( key, value ? 1 : 0 );
		}
	}
}
