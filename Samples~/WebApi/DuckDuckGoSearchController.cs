namespace BitStrap.Examples
{
	//http://api.duckduckgo.com/?q=goku&format=json
	[WebUrl( "" )]
	public class DuckDuckGoSearchController : WebController<DuckDuckGoSearchController>
	{
		[WebUrl( "" )]
		[WebAction( WebMethod.GET, "", "format" )]
		public WebAction<string> web;
	}
}