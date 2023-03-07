using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class WebApiExample : MonoBehaviour
	{
		[Header( "This will search the web using DuckDuckGo api." )]
		public WebApi duckDuckGoWebApi;
		public string search = "BitStrap";

		[Button]
		public void Search()
		{
			if( !Application.isPlaying )
			{
				Debug.LogWarning( "In order to see WebApi working, please enter Play mode." );
				return;
			}

			duckDuckGoWebApi.Controller<DuckDuckGoSearchController>().web.Request( search, "json" ).Then( result =>
			{
				result.Match(
					ok: text => Debug.LogFormat( "RESULT: {0}", text ),
					error: error => Debug.LogFormat( "ERROR: {0}", error )
				);
			} );
		}
	}
}