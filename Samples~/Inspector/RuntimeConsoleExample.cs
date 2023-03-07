using UnityEngine;

namespace BitStrap.Examples
{
	public class RuntimeConsoleExample : MonoBehaviour
	{
		[Header( "Enter PlayMode and keep pressed \"Shift+C\"" )]
		public bool printLog = true;

		public bool printWarning = true;
		public bool printError = true;

		[Button]
		public void PrintSomeDebugLogs()
		{
			if( printLog )
				Debug.LogFormat( "Some Debug Log Message. Keep pressed \"Shift+C\" to see it in-game" );
			if( printWarning )
				Debug.LogWarning( "Some Debug Log Warning. Keep pressed \"Shift+C\" to see it in-game" );
			if( printError )
				Debug.LogError( "Some Debug Log Error. Keep pressed \"Shift+C\" to see it in-game" );
		}
	}
}
