using UnityEditor;

namespace BitStrap
{
	/// <summary>
	/// Helper to work with scripting define symbols.
	/// You can check which ones are currently being used by going to
	/// "Player Settings" > "Other Settings" > "Scripting Define Symbols".
	/// </summary>
	public static class ScriptDefinesHelper
	{
		/// <summary>
		/// Given a BuildTarget, checks whether symbol is defined or not.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="symbol"></param>
		/// <returns></returns>
		public static bool IsDefined( BuildTarget target, string symbol )
		{
			return IsDefined( GetGroupFromBuildTarget( target ), symbol );
		}

		/// <summary>
		/// Given a BuildTarget, sets a symbol to be defined or not.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="symbol"></param>
		/// <param name="defined"></param>
		public static void SetDefined( BuildTarget target, string symbol, bool defined )
		{
			SetDefined( GetGroupFromBuildTarget( target ), symbol, defined );
		}

		/// <summary>
		/// Given a BuildTargetGroup, checks whether symbol is defined or not.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="symbol"></param>
		/// <returns></returns>
		public static bool IsDefined( BuildTargetGroup group, string symbol )
		{
			string definesText = PlayerSettings.GetScriptingDefineSymbolsForGroup( group );
			return System.Array.Exists( definesText.Trim().Split( ';' ), d => d.Equals( symbol ) );
		}

		/// <summary>
		/// Given a BuildTargetGroup, sets a symbol to be defined or not.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="symbol"></param>
		/// <param name="defined"></param>
		public static void SetDefined( BuildTargetGroup group, string symbol, bool defined )
		{
			string definesText = PlayerSettings.GetScriptingDefineSymbolsForGroup( group );
			string[] defines = definesText.Trim().Split( ';' );

			if( !defined )
			{
				ArrayUtility.Remove( ref defines, symbol );
			}
			else if( System.Array.IndexOf( defines, symbol ) < 0 )
			{
				ArrayUtility.Add( ref defines, symbol );
			}

			definesText = string.Join( ";", defines );
			PlayerSettings.SetScriptingDefineSymbolsForGroup( group, definesText );
		}

		private static BuildTargetGroup GetGroupFromBuildTarget( BuildTarget target )
		{
			string targetName = target.ToString();

			foreach( BuildTargetGroup group in System.Enum.GetValues( typeof( BuildTargetGroup ) ) )
			{
				if( targetName.StartsWith( group.ToString() ) )
				{
					return group;
				}
			}

			return BuildTargetGroup.Unknown;
		}
	}
}
