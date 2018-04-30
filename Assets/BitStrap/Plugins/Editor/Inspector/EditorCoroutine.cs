using System.Collections;
using UnityEditor;

namespace BitStrap
{
	/// <summary>
	/// Makes it possible to run coroutines while inside an editor script
	/// using the EditorApplication.update callback.
	/// </summary>
	public sealed class EditorCoroutine
	{
		private readonly IEnumerator routine;

		private EditorCoroutine( IEnumerator routine )
		{
			this.routine = routine;
		}

		/// <summary>
		/// Start an editor coroutine.
		/// </summary>
		/// <param name="routine"></param>
		/// <returns></returns>
		public static EditorCoroutine Start( IEnumerator routine )
		{
			EditorCoroutine coroutine = new EditorCoroutine( routine );
			coroutine.Start();
			return coroutine;
		}

		/// <summary>
		/// Stop this editor coroutine.
		/// </summary>
		public void Stop()
		{
			EditorApplication.update -= Update;
		}

		private void Start()
		{
			EditorApplication.update += Update;
		}

		private void Update()
		{
			if( !routine.MoveNext() )
			{
				Stop();
			}
		}
	}
}