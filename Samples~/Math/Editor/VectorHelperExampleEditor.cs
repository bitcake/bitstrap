using UnityEditor;
using UnityEngine;

namespace BitStrap.Examples
{
	[CustomEditor( typeof( VectorHelperExample ) )]
	public class VectorHelperExampleEditor : Editor
	{
#if UNITY_5 || UNITY_2017
		private static readonly ColorPickerHDRConfig hdrConfig = new ColorPickerHDRConfig( 0.0f, 1.0f, 0.0f, 1.0f );
#endif

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var self = target as VectorHelperExample;

			using( DisabledGroup.Do( true ) )
			{
				EditorGUILayout.ToggleLeft( string.Format( "Vector is zero?" ), self.IsVectorZero() );
				bool betweenVectors = self.IsVectorCBetweenVectorsAAndB();
				EditorGUILayout.ToggleLeft( string.Format( "Vector C between vectors A and B?" ), betweenVectors );
				bool onVectorSide = self.IsVectorCOnTheSameSideAsVectorBInRelationToA();
				EditorGUILayout.ToggleLeft( string.Format( "Vector C on the same side as vectors B in relation to A?" ), onVectorSide );
			}

			var rect = EditorGUILayout.GetControlRect( false, 200.0f );

			float vectorSize = Mathf.Min( rect.width, rect.height ) * 0.5f;
			Vector2 vectorScale = new Vector2( vectorSize, -vectorSize );
			Vector2 center = rect.center;

			Handles.color = Color.red;
			Handles.DrawLine( center, center + Vector2.Scale( self.vectorA, vectorScale ) );

			Handles.color = Color.green;
			Handles.DrawLine( center, center + Vector2.Scale( self.vectorB, vectorScale ) );

			Handles.color = Color.blue;
			Handles.DrawLine( center, center + Vector2.Scale( self.vectorC, vectorScale ) );

			using( DisabledGroup.Do( true ) )
			{
#if UNITY_5 || UNITY_2017
				EditorGUILayout.ColorField( new GUIContent( "Vector A" ), Color.red, false, false, false, hdrConfig );
				EditorGUILayout.ColorField( new GUIContent( "Vector B" ), Color.green, false, false, false, hdrConfig );
				EditorGUILayout.ColorField( new GUIContent( "Vector C" ), Color.blue, false, false, false, hdrConfig );
#else
				EditorGUILayout.ColorField( new GUIContent( "Vector A" ), Color.red, false, false, false );
				EditorGUILayout.ColorField( new GUIContent( "Vector B" ), Color.green, false, false, false );
				EditorGUILayout.ColorField( new GUIContent( "Vector C" ), Color.blue, false, false, false );
#endif
			}
		}
	}
}
