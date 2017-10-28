using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Isolates part of the game objects hierarchy by activating the selected
	/// ones and deactivating the others. Behaves similarly to the isolate tool
	/// present in Autodesk Maya. Just select a game object and press Ctrl+E to
	/// isolate it and its children and parents.
	/// </summary>
	public static class Isolate
	{
		private static Dictionary<Transform, bool> savedState = new Dictionary<Transform, bool>();

		static Isolate()
		{
			Deserialize();
		}

		public static void IsolateTransform( Transform transform )
		{
			if( savedState.Count > 0 )
			{
				RestoreTree();
				Serialize();
			}
			else if( transform != null )
			{
				DeactivateTree( transform );
				Serialize();
			}
		}

		[MenuItem( "CONTEXT/Transform/Isolate %E" )]
		private static void ContextMenu( MenuCommand command )
		{
			IsolateTransform( command.context as Transform );
		}

		[MenuItem( "Window/BitStrap/Isolate Selected %E" )]
		private static void SelectionMenu( MenuCommand command )
		{
			IsolateTransform( Selection.activeTransform );
		}

		private static void DeactivateTree( Transform transform )
		{
			for( Transform t = transform; t.parent != null; t = t.parent )
			{
				Transform parent = t.parent;

				foreach( Transform child in parent )
				{
					if( child != t )
					{
						savedState.Add( child, child.gameObject.activeSelf );
						child.gameObject.SetActive( false );
					}
				}
			}
		}

		private static void RestoreTree()
		{
			foreach( var transformState in savedState )
			{
				Transform t = transformState.Key;

				if( t != null )
				{
					t.gameObject.SetActive( transformState.Value );
				}
			}

			savedState.Clear();
		}

		private static void Serialize()
		{
			var sb = new System.Text.StringBuilder();
			foreach( var transformState in savedState )
			{
				if( transformState.Key != null )
				{
					sb.Append( transformState.Key.GetInstanceID() );
					sb.Append( ':' );
					sb.Append( transformState.Value );
					sb.Append( ',' );
				}
			}

			if( sb.Length > 1 )
			{
				sb.Remove( sb.Length - 1, 1 );
			}

			EditorPrefs.SetString( "Isolate", sb.ToString() );
		}

		private static void Deserialize()
		{
			savedState.Clear();

			string[] serializedStates = EditorPrefs.GetString( "Isolate", "" ).Split( ',' );

			foreach( string serializedState in serializedStates )
			{
				TryParseState( serializedState );
			}
		}

		private static void TryParseState( string serializedState )
		{
			string[] keyPair = serializedState.Split( ':' );

			if( keyPair.Length >= 2 )
			{
				int instanceId;
				bool instanceState;

				if( int.TryParse( keyPair[0], out instanceId ) && bool.TryParse( keyPair[1], out instanceState ) )
				{
					Transform t = EditorUtility.InstanceIDToObject( instanceId ) as Transform;

					if( t != null )
					{
						savedState.Add( t, instanceState );
					}
				}
			}
		}
	}
}