using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// A simple tween class for shader param interpolation.
	/// It uses the MaterialPropertyBlock class to modify the renderer's material properties.
	/// It is better to use it than directly setting a material property because it does not
	/// create a new Material instance and, thus, does not generates a new drawcall.
	/// </summary>
	public sealed class TweenShader : MonoBehaviour
	{
		/// <summary>
		/// The tween duration.
		/// </summary>
		public Timer.Duration duration = new Timer.Duration( 1.0f );
		public Timer timer = new Timer();

		/// <summary>
		/// List of shader properties to be interpolated.
		/// </summary>
		public List<TweenShaderProperty> shaderProperties;

		/// <summary>
		/// The current renderer to apply the MaterialPropertyBlock.
		/// </summary>
		public Renderer targetRenderer;

		private MaterialPropertyBlock propertyBlock;
		private bool playingForward;

		/// <summary>
		/// Evaluates the tween and, consequently, all the shader properties at "t" [0, 1].
		/// </summary>
		/// <param name="t"></param>
		public void SampleAt( float t )
		{
			propertyBlock.Clear();
			targetRenderer.GetPropertyBlock( propertyBlock );

			foreach( TweenShaderProperty p in shaderProperties )
				p.Evaluate( propertyBlock, t );

			targetRenderer.SetPropertyBlock( propertyBlock );
		}

		/// <summary>
		/// Play the tween forward interpolating all the shader properties.
		/// </summary>
		public void PlayForward()
		{
			SampleAt( 0.0f );
			Play( true );
		}

		/// <summary>
		/// Play the tween backward interpolating all the shader properties.
		/// </summary>
		public void PlayBackward()
		{
			SampleAt( 1.0f );
			Play( false );
		}

		/// <summary>
		/// Stop the tween.
		/// Use Clear() to reset to the original state.
		/// </summary>
		public void Stop()
		{
			timer.Stop();
			SampleAt( 0.0f );
			enabled = false;
		}

		/// <summary>
		/// Stops and clear the cached MaterialPropertyBlock.
		/// </summary>
		public void Clear()
		{
			propertyBlock.Clear();
			targetRenderer.SetPropertyBlock( propertyBlock );

			timer.Stop();
			enabled = false;
		}

		private void Play( bool forward )
		{
			playingForward = forward;
			timer.Start();
			enabled = true;
		}

		private void Init()
		{
#if UNITY_EDITOR
			lastEditorTimestep = UnityEditor.EditorApplication.timeSinceStartup;
#endif

			if( propertyBlock == null )
				propertyBlock = new MaterialPropertyBlock();
		}

		private void Reset()
		{
			targetRenderer = GetComponent<Renderer>();
			enabled = false;
		}

		private void Awake()
		{
			Init();

			if( enabled )
				PlayForward();
		}

#if UNITY_EDITOR
		private double lastEditorTimestep = -1.0;

		private float GetEditorDeltaTime()
		{
			if( UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode )
				return Time.unscaledDeltaTime;

			float diff = ( float ) ( UnityEditor.EditorApplication.timeSinceStartup - lastEditorTimestep );
			lastEditorTimestep = UnityEditor.EditorApplication.timeSinceStartup;

			return diff;
		}
#endif

		private void Update()
		{
#if UNITY_EDITOR
			if( timer.Update( duration, GetEditorDeltaTime() ) )
#else
			if( timer.Update( duration ) )
#endif
			{
				SampleAt( playingForward ? 1.0f : 0.0f );
				enabled = false;
			}

			float t = timer.GetProgress( duration );
			t = playingForward ? t : 1.0f - t;
			SampleAt( t );
		}
	}
}