using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Provides cool features to the component context menu such as:
	/// "Fold All", "Move to Top", "Move to Bottom" and "Sort Components".
	/// </summary>
	public sealed class ComponentContextMenu
	{
		private class ComponentComparer : IComparer<Component>
		{
			public static readonly ComponentComparer instance = new ComponentComparer();

			public int Compare( Component a, Component b )
			{
				const int bigReasonableNumber = 999999;

				System.Type monobehaviourType = typeof( MonoBehaviour );
				int monobehaviourOffset = monobehaviourType.IsAssignableFrom( a.GetType() ) ? bigReasonableNumber : 0;
				monobehaviourOffset -= monobehaviourType.IsAssignableFrom( b.GetType() ) ? bigReasonableNumber : 0;

				return string.Compare( a.GetType().Name, b.GetType().Name ) + monobehaviourOffset;
			}
		}

		[MenuItem( "CONTEXT/Component/Fold All" )]
		public static void FoldAll( MenuCommand command )
		{
			ActiveEditorTracker editorTracker = ActiveEditorTracker.sharedTracker;
			Editor[] editors = editorTracker.activeEditors;

			bool areAllFolded = true;
			for( int i = 1; i < editors.Length; i++ )
			{
				if( editorTracker.GetVisible( i ) > 0 )
					areAllFolded = false;
			}

			for( int i = 1; i < editors.Length; i++ )
			{
				if( editorTracker.GetVisible( i ) < 0 )
					continue;

				editorTracker.SetVisible( i, areAllFolded ? 1 : 0 );
				InternalEditorUtility.SetIsInspectorExpanded( editors[i].target, areAllFolded );
			}
		}

		[MenuItem( "CONTEXT/Component/Sort Components" )]
		public static void SortComponents( MenuCommand command )
		{
			var target = ( Component ) command.context;
			Component[] components = target.gameObject.GetComponents<Component>();
			SerializedObject gameObject = new SerializedObject( target.gameObject );

			// Bubble Sort
			for( int i = 0; i < components.Length; i++ )
			{
				for( int j = 1; j < components.Length - i; j++ )
				{
					var up = components[j - 1];
					var down = components[j];

					if( ComponentComparer.instance.Compare( up, down ) > 0 )
					{
						if( ComponentUtility.MoveComponentUp( down ))
						{
							components[j - 1] = down;
							components[j] = up;
						}
					}
				}
			}

			gameObject.ApplyModifiedProperties();
		}
	}
}