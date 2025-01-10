using UnityEngine;

namespace BitStrap
{
	public readonly struct GUILayoutChart
	{
		public readonly float[] samples;

		public GUILayoutChart(int sampleCount)
		{
			samples = new float[sampleCount];
		}

		public void AddSample(float sample)
		{
			for (int i = 0; i < samples.Length - 1; i++)
			{
				samples[i] = samples[i + 1];
			}
			samples[samples.Length - 1] = sample;
		}

		public void Draw(params GUILayoutOption[] layoutOptions)
		{
			Color backgroundColor = new Color(0.2f, 0.2f, 0.2f);
			Color foregroundColor = new Color(0.5f, 0.5f, 0.5f);

			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, layoutOptions);

			GUI.DrawTexture(
				rect,
				Texture2D.whiteTexture,
				ScaleMode.StretchToFill,
				/* alphaBlend */ false,
				rect.width / rect.height,
				backgroundColor,
				/* borderWidth */ 0.0f,
				/* borderRadius */ 0.0f
			);

			float sampleMax = 0.0f;
			foreach (float sample in samples)
			{
				sampleMax = Mathf.Max(sampleMax, sample);
			}

			float sampleRectWidth = rect.width / samples.Length;
			for (int i = 0; i < samples.Length; i++)
			{
				float sample = samples[i];
				float percentage = sample / sampleMax;

				Rect sampleRect = rect;
				sampleRect.x += i * sampleRectWidth;
				sampleRect.width = sampleRectWidth;
				sampleRect.yMin += sampleRect.height * Mathf.Clamp01(1.0f - percentage);

				GUI.DrawTexture(
					sampleRect,
					Texture2D.whiteTexture,
					ScaleMode.StretchToFill,
					/* alphaBlend */ false,
					sampleRect.width / sampleRect.height,
					foregroundColor,
					/* borderWidth */ 0.0f,
					/* borderRadius */ 0.0f
				);
			}
		}
	}
}
