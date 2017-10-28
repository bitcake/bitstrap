using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class DuckDuckGoSearchExample : MonoBehaviour
	{
		public WebApi duckDuckGoWebApi;
		public string search = "Goku";

		[Button]
		public void Search()
		{
			if( !Application.isPlaying )
			{
				Debug.LogWarning( "In order to see WebApi working, please enter Play mode." );
				return;
			}

			duckDuckGoWebApi.Controller<DuckDuckGoSearchController>().web.Request( search, "json" ).OnRawResponse( text =>
			{
				Debug.LogFormat( "RESULT: {0}", text );
			} ).OnError( error =>
			{
				Debug.LogFormat( "ERROR: {0}", error );
			} );
		}

		[System.Serializable]
		public struct NodeReference
		{
			public Node Reference { get; set; }
			public int referenceId;
		}

		[System.Serializable]
		public sealed class Node
		{
			public string name;

			public NodeReference input;
		}

		public struct Nodes
		{
			public Node[] nodes;
		}

		[Button]
		public void Test()
		{
			var s0 = new Node();
			s0.name = "s0";

			var s1 = new Node();
			s1.name = "s1";
			s1.input.Reference = s0;

			s0.input.Reference = s1;

			var ss = new Nodes();
			ss.nodes = new Node[] { s0, s1 };

			foreach( var s in ss.nodes )
			{
				s.input.referenceId = System.Array.IndexOf( ss.nodes, s.input.Reference );
			}

			Debug.Log( JsonUtility.ToJson( ss ) );
		}
	}
}