using UnityEngine;

namespace BitStrap.Examples
{
	public class SafeActionExample : MonoBehaviour
	{
		private SafeAction action = new SafeAction();

		[Button]
		public void CallAction()
		{
			if( Application.isPlaying )
			{
				action.Call();
			}
			else
			{
				Debug.LogWarning( "In order to see SafeAction working, please enter Play mode." );
			}
		}

		private void ActionCallback()
		{
			Debug.Log( "Action callback was executed." );
		}

		private void Awake()
		{
			action.Register( ActionCallback );
		}
	}
}
