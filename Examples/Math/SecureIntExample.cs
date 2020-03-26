using UnityEngine;

namespace BitStrap.Examples
{
	public class SecureIntExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public SecureInt secureInteger = new SecureInt( 10 );

		[Button]
		public void GetValueInMemory()
		{
			if( Application.isPlaying )
			{
				Debug.LogFormat( "Encrypted Value: {0}", secureInteger.EncryptedValue );
			}
			else
			{
				Debug.LogWarning( "In order to see SecureInt working, please enter Play mode." );
			}
		}
	}
}
