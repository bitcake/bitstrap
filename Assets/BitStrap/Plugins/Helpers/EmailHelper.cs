using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BitStrap
{
	/// <summary>
	/// Bunch of helper methods to work with email strings.
	/// </summary>
	public static class EmailHelper
	{
		private const string emailPattern =
			@"^([0-9a-zA-Z]" + //Start with a digit or alphabate
			@"([\+\-_\.][0-9a-zA-Z]+)*" + // No continues or ending +-_. chars in email
			@")+" +
			@"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";

		private static ICollection<string> emailProviders = new HashSet<string>();

		static EmailHelper()
		{
			emailProviders.Add( "gmail.com" );
			emailProviders.Add( "hotmail.com" );
			emailProviders.Add( "yahoo.com" );
			emailProviders.Add( "live.com" );
			emailProviders.Add( "icloud.com" );
			emailProviders.Add( "me.com" );
			emailProviders.Add( "outlook.com" );
		}

		/// <summary>
		/// Check if text is a valid email.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsEmail( string text )
		{
			return Regex.IsMatch( text, emailPattern );
		}

		/// <summary>
		/// Tries to check if the email string was mistyped and, if so, it suggests the correct one.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="correctEmail"></param>
		/// <returns></returns>
		public static bool IsMistyped( string email, out string correctEmail )
		{
			if( !IsEmail( email ) )
			{
				correctEmail = "";
				return true;
			}

			correctEmail = email;

			int emailProviderIndex = email.IndexOf( '@' ) + 1;
			int emailProviderLength = email.IndexOf( '.', emailProviderIndex );

			int secondDotIndex = email.IndexOf( '.', emailProviderLength + 1 );

			if( secondDotIndex < 0 )
			{
				secondDotIndex = email.Length;
			}

			emailProviderLength = secondDotIndex - emailProviderIndex;

			string emailProvider = email.Substring( emailProviderIndex, emailProviderLength );

			Option<string> mostCorrectProvider = Functional.None;
			int mostCorrectDistance = 6;

			foreach( string provider in emailProviders )
			{
				int distance = emailProvider.Distance( provider );

				if( distance == 0 )
				{
					return false;
				}
				else if( distance < mostCorrectDistance )
				{
					mostCorrectDistance = distance;
					mostCorrectProvider = provider;
				}
			}

			string correctProvider;
			if( mostCorrectProvider.TryGet( out correctProvider ) )
			{
				correctEmail = email.Remove( emailProviderIndex, emailProviderLength ).Insert( emailProviderIndex, correctProvider );
				return true;
			}

			return false;
		}
	}
}
