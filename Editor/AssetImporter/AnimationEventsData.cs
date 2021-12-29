using System;
using System.Collections.Generic;

[Serializable]
public class AnimationEventsData
{
	public int FPS;
	public string Character;
	public FrameMarker[] TimelineMarkers;

	public ActionToAnimationMarker[] ActionsMarkers;
}

[Serializable]
public class ActionToAnimationMarker
{
	public string Name;
	public FrameMarker[] Markers;
}

[Serializable]
public class FrameMarker
{
	public string Name;
	public int Frame;
}