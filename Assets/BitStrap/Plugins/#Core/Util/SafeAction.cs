using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Safe version of System.Action that envelopes each call in a try/catch to prevent
	/// execution flow interruption just because of one bad callback.
	/// </summary>
	public sealed class SafeAction
	{
		private readonly List<Action> actions = new List<Action>();

		/// <summary>
		/// How many actions registered.
		/// </summary>
		public int Count
		{
			get { return actions.Count; }
		}

		/// <summary>
		/// Register a callback.
		/// </summary>
		/// <param name="a"></param>
		public void Register( Action a )
		{
			if( !actions.Contains( a ) )
				actions.Add( a );
		}

		/// <summary>
		/// Unregister a callback.
		/// </summary>
		/// <param name="a"></param>
		public void Unregister( Action a )
		{
			actions.Remove( a );
		}

		/// <summary>
		/// Unregisters all callbacks.
		/// </summary>
		public void UnregisterAll()
		{
			actions.Clear();
		}

		/// <summary>
		/// Trigger all callbacks.
		/// </summary>
		public void Call()
		{
			for( int i = 0; i < actions.Count; i++ )
			{
				try
				{
					if( actions[i] != null )
						actions[i]();
				}
				catch( Exception e )
				{
					Debug.LogException( e );
				}
			}
		}
	}

	/// <summary>
	/// Safe version of System.Action[T] that envelopes each call in a try/catch to prevent
	/// execution flow interuption just because of one bad callback.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class SafeAction<T>
	{
		private List<Action<T>> actions = new List<Action<T>>();

		/// <summary>
		/// How many actions registered.
		/// </summary>
		public int Count
		{
			get { return actions.Count; }
		}

		/// <summary>
		/// Register a callback.
		/// </summary>
		/// <param name="a"></param>
		public void Register( Action<T> a )
		{
			if( !actions.Contains( a ) )
				actions.Add( a );
		}

		/// <summary>
		/// Unregister a callback.
		/// </summary>
		/// <param name="a"></param>
		public void Unregister( Action<T> a )
		{
			actions.Remove( a );
		}

		/// <summary>
		/// Unregisters all callbacks.
		/// </summary>
		public void UnregisterAll()
		{
			actions.Clear();
		}

		/// <summary>
		/// Trigger all callbacks.
		/// </summary>
		/// <param name="p1"></param>
		public void Call( T p1 )
		{
			for( int i = 0; i < actions.Count; i++ )
			{
				try
				{
					if( actions[i] != null )
						actions[i]( p1 );
				}
				catch( Exception e )
				{
					Debug.LogException( e );
				}
			}
		}
	}

	/// <summary>
	/// Safe version of System.Action[T1, T2] that envelopes each call in a try/catch to prevent
	/// execution flow interuption just because of one bad callback.
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	public sealed class SafeAction<T1, T2>
	{
		private List<Action<T1, T2>> actions = new List<Action<T1, T2>>();

		/// <summary>
		/// How many actions registered.
		/// </summary>
		public int Count
		{
			get { return actions.Count; }
		}

		/// <summary>
		/// Register a callback.
		/// </summary>
		/// <param name="a"></param>
		public void Register( Action<T1, T2> a )
		{
			if( !actions.Contains( a ) )
				actions.Add( a );
		}

		/// <summary>
		/// Unregister a callback.
		/// </summary>
		/// <param name="a"></param>
		public void Unregister( Action<T1, T2> a )
		{
			actions.Remove( a );
		}

		/// <summary>
		/// Unregisters all callbacks.
		/// </summary>
		public void UnregisterAll()
		{
			actions.Clear();
		}

		/// <summary>
		/// Trigger all callbacks.
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		public void Call( T1 p1, T2 p2 )
		{
			for( int i = 0; i < actions.Count; i++ )
			{
				try
				{
					if( actions[i] != null )
						actions[i]( p1, p2 );
				}
				catch( Exception e )
				{
					Debug.LogException( e );
				}
			}
		}
	}
}