using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( AnimationParameter ), true )]
	public sealed class AnimationParameterDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			var behaviour = property.serializedObject.targetObject as MonoBehaviour;

			Animator animator = null;
			var nameProperty = property.GetMemberProperty<AnimationParameter>( p => p.name );

			if( behaviour != null )
			{
				AnimatorFieldAttribute animatorField;
				if( fieldInfo.GetAttribute<AnimatorFieldAttribute>( false ).TryGet( out animatorField ) )
				{
					var animatorProperty = property.serializedObject.FindProperty( animatorField.animatorFieldName );
					if( animatorProperty != null )
						animator = animatorProperty.objectReferenceValue as Animator;
				}
				else
				{
					animator = behaviour.GetComponent<Animator>();
				}
			}

			if( animator == null || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null )
			{
				EditorGUI.PropertyField( position, nameProperty, label );
			}
			else
			{
				AnimatorControllerParameter[] parameters = FilterParameters( property, animator );
				var popupOptions = parameters.Select( x => new GUIContent( x.name ) ).ToArray();
				int currentIndex = Array.FindIndex( parameters, x => x.name == nameProperty.stringValue );

				EditorGUI.BeginChangeCheck();
				currentIndex = EditorGUI.Popup( position, label, currentIndex, popupOptions );

				if( EditorGUI.EndChangeCheck() )
				{
					nameProperty.stringValue = parameters[currentIndex].name;
					property.serializedObject.ApplyModifiedProperties();
				}
			}
		}

		private AnimatorControllerParameter[] FilterParameters( SerializedProperty property, Animator animator )
		{
			var t = property.type;
			if( typeof( BoolAnimationParameter ).Name == t )
				return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Bool ).ToArray();
			if( typeof( IntAnimationParameter ).Name == t )
				return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Int ).ToArray();
			if( typeof( FloatAnimationParameter ).Name == t )
				return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Float ).ToArray();
			if( typeof( TriggerAnimationParameter ).Name == t )
				return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Trigger ).ToArray();
			return animator.parameters;
		}
	}
}