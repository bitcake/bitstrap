using UnityEngine;

namespace BitStrap.Examples
{
	public class EmailHelperExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public string email = "example@email.com";

		[Button]
		public void IsValidEmail()
		{
			Debug.LogFormat( "Email \"{0}\" is valid? {1}", email, EmailHelper.IsEmail( email ) );
		}

		[Button]
		public void CheckEmailTypo()
		{
			string suggestedEmail;
			if( EmailHelper.IsMistyped( email, out suggestedEmail ) )
				Debug.LogFormat( "Email \"{0}\" may be mistyped. Did you mean: \"{1}\"", email, suggestedEmail );
			else
				Debug.LogFormat( "Email \"{0}\" seems legit!", email );
		}
	}
}
