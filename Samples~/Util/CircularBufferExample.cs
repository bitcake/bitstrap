using System.Text;
using UnityEngine;

namespace BitStrap.Examples
{
	public class CircularBufferExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!\nCircular Buffer capacity: 4" )]
		public int value = 10;

		private CircularBuffer<int> buffer = new CircularBuffer<int>( 4 );

		[Button]
		public void Add()
		{
			if( !Application.isPlaying )
			{
				Debug.LogWarning( "In order to see CircularBuffer working, please enter Play mode." );
				return;
			}

			buffer.Add( value++ );
			Print();
		}

		[Button]
		public void Print()
		{
			if( !Application.isPlaying )
			{
				Debug.LogWarning( "In order to see CircularBuffer working, please enter Play mode." );
				return;
			}

			StringBuilder sb = new StringBuilder();
			foreach( int element in buffer )
			{
				sb.Append( element );
				sb.Append( ", " );
			}

			Debug.Log( sb );
		}
	}
}
