using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public sealed class EventsImporter : AssetPostprocessor
{
	public override int GetPostprocessOrder()
	{
		return 100;
	}

	private void OnPostprocessAnimation(GameObject root, AnimationClip clip)
	{
		var eventsData = GetEventsJson();
		if (eventsData == null)
			return;
		
		foreach (var action in eventsData.ActionsMarkers)
		{
			var clipName = clip.name.Split('|');
			if (clipName[1] == action.Name)
			{
				List<AnimationEvent> allAnimationEvents = new List<AnimationEvent>();
				foreach (var marker in action.Markers)
				{
					AnimationEvent animationEvent = new AnimationEvent();
					animationEvent.functionName = marker.Name;
					var fpsToTime = marker.Frame / clip.frameRate;
					animationEvent.time = fpsToTime;
					allAnimationEvents.Add(animationEvent);
				}
				AnimationUtility.SetAnimationEvents(clip, allAnimationEvents.ToArray());
			}
		}
	}

	private AnimationEventsData GetEventsJson()
	{
		var extension = Path.GetExtension(assetPath);
		var jsonPath = assetPath.Replace(extension, "_events.json");
		if (!File.Exists(jsonPath))
			return null;
		var jsonText = File.ReadAllText(jsonPath);
		var animationData = JsonUtility.FromJson<AnimationEventsData>(jsonText);
		
		return animationData;
	}
}