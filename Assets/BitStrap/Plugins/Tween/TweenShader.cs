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
	public class TweenShader : MonoBehaviour
	{
		/// <summary>
		/// The tween duration.
		/// </summary>
		public Timer duration = new Timer( 1.0f );

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
			duration.Stop();
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

			duration.Stop();
			enabled = false;
		}

		private void Play( bool forward )
		{
			playingForward = forward;
			duration.Start();
			enabled = true;
		}

		private void OnTimer()
		{
			SampleAt( playingForward ? 1.0f : 0.0f );
			enabled = false;
		}

		private void Init()
		{
#if UNITY_EDITOR
			lastEditorTimestep = UnityEditor.EditorApplication.timeSinceStartup;
#endif

			if( propertyBlock == null )
			{
				propertyBlock = new MaterialPropertyBlock();
				duration.onTimer.UnregisterAll();
				duration.onTimer.Register( OnTimer );
			}
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
			duration.OnUpdate( GetEditorDeltaTime() );
#else
			duration.OnUpdate();
#endif

			float t = duration.Progress;
			t = playingForward ? t : 1.0f - t;
			SampleAt( t );
		}
	}
}