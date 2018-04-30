using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( ParticleController ) )]
	public sealed class ParticleControllerDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			SerializedProperty root = property.GetMemberProperty<ParticleController>( p => p.RootParticleSystem );
			EditorGUI.PropertyField( position, root, label );
		}
	}
}
