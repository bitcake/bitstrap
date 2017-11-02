using UnityEngine;

namespace BitStrap
{
	public interface IValidatable
	{
		void Validate();
	}

	/// <summary>
	/// Specialized version of NumberRange for int.
	/// </summary>
	[System.Serializable]
	public sealed class IntRange : NumberRange<int>
	{
		public IntRange() : base()
		{ }

		public IntRange( int min, int max ) : base( min, max )
		{ }

		/// <summary>
		/// Random number inside this bounds.
		/// </summary>
		/// <returns></returns>
		public int RandomInside()
		{
			return Random.Range( min, max );
		}
	}

	/// <summary>
	/// Specialized version of NumberRange for float.
	/// </summary>
	[System.Serializable]
	public sealed class FloatRange : NumberRange<float>
	{
		public FloatRange() : base()
		{ }

		public FloatRange( float min, float max ) : base( min, max )
		{ }

		/// <summary>
		/// Random number inside this bounds.
		/// </summary>
		/// <returns></returns>
		public float RandomInside()
		{
			return Random.Range( min, max );
		}

		/// <summary>
		/// Lerp between its min and max numbers.
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public float Lerp( float t )
		{
			return Mathf.Lerp( min, max, t );
		}
	}

	/// <summary>
	/// Specialized version of NumberRange for double.
	/// </summary>
	[System.Serializable]
	public sealed class DoubleRange : NumberRange<double>
	{
		public DoubleRange() : base()
		{ }

		public DoubleRange( double min, double max ) : base( min, max )
		{ }
	}

	/// <summary>
	/// Represents a number range. Contains a minimum and maximun value.
	/// Also it has a nice inspector with auto validation.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public class NumberRange<T> : IValidatable where T : System.IComparable<T>
	{
		[SerializeField]
		protected T min = default( T );

		[SerializeField]
		protected T max = default( T );

		/// <summary>
		/// Bounds minimum value.
		/// </summary>
		public T Min
		{
			get { return min; }
			set { min = value; ValidateBounds(); }
		}

		/// <summary>
		/// Bounds maximum value.
		/// </summary>
		public T Max
		{
			get { return max; }
			set { max = value; ValidateBounds(); }
		}

		public NumberRange()
		{
		}

		public NumberRange( T min, T max )
		{
			Set( min, max );
		}

		/// <summary>
		/// Set both values at once.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public void Set( T min, T max )
		{
			this.min = min;
			this.max = max;
			ValidateBounds();
		}

		/// <summary>
		/// Clamp a value inside min and max bounds.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public T Clamp( T value )
		{
			value = GetMax( value, min );
			value = GetMin( value, max );
			return value;
		}

		/// <summary>
		/// Returns max if selectMax is true. Returns min otherwise.
		/// </summary>
		/// <param name="selectMax"></param>
		/// <returns></returns>
		public T SelectMax( bool selectMax )
		{
			return selectMax ? max : min;
		}

		/// <summary>
		/// Validates if min < max
		/// And, if not, corrects the values.
		/// </summary>
		void IValidatable.Validate()
		{
			ValidateBounds();
		}

		protected static T GetMin( T a, T b )
		{
			return a.CompareTo( b ) <= 0 ? a : b;
		}

		protected static T GetMax( T a, T b )
		{
			return a.CompareTo( b ) >= 0 ? a : b;
		}

		private void ValidateBounds()
		{
			T tempMin = min;
			T tempMax = max;

			min = GetMin( tempMin, tempMax );
			max = GetMax( tempMin, tempMax );
		}
	}
}
