using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// A shader property that can be interpolated.
	/// </summary>
	[System.Serializable]
	public sealed class TweenShaderProperty
	{
		/// <summary>
		/// The shader property type.
		/// </summary>
		public enum Type
		{
			Float,
			Vector,
			Color
		}

		/// <summary>
		/// The property's name.
		/// </summary>
		public string name;

		/// <summary>
		/// The property's tween curve.
		/// </summary>
		public AnimationCurve curve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );

		/// <summary>
		/// Tween origin values.
		/// </summary>
		public Vector4 from;

		/// <summary>
		/// Tween target values.
		/// </summary>
		public Vector4 to;

		/// <summary>
		/// The property's type.
		/// </summary>
		public Type type = Type.Float;

		private bool hasId = false;
		private int id = 0;

		/// <summary>
		/// The cached property Id for faster access.
		/// </summary>
		public int Id
		{
			get
			{
				if( !hasId )
					id = Shader.PropertyToID( name );

				return id;
			}
		}

		/// <summary>
		/// Evaluates the property at "t" and stores its value in the MaterialPropertyBlock.
		/// </summary>
		/// <param name="block"></param>
		/// <param name="t"></param>
		public void Evaluate( MaterialPropertyBlock block, float t )
		{
			t = curve.Evaluate( t );

			switch( type )
			{
			case Type.Float:
				block.SetFloat( Id, Mathf.Lerp( from.x, to.x, t ) );
				break;

			case Type.Vector:
				block.SetVector( Id, Vector4.Lerp( from, to, t ) );
				break;

			case Type.Color:
				block.SetColor( Id, Vector4.Lerp( from, to, t ) );
				break;
			}
		}
	}
}