using UnityEngine;

namespace BitStrap.Examples
{
	public class VectorHelperExample : MonoBehaviour
	{
		[Header( "Edit the fields and look at the checkboxes below!", order = 1 )]
		public Vector2 vector = new Vector2( 0.00001f, 0.0f );

		[Range( 0.0f, 360.0f )]
		public float vectorAAngle = 0.0f;

		[Range( 0.0f, 360.0f )]
		public float vectorBAngle = 90.0f;

		[Range( 0.0f, 360.0f )]
		public float vectorCAngle = 45.0f;

		[ReadOnly]
		public Vector2 vectorA = Vector2.zero;

		[ReadOnly]
		public Vector2 vectorB = Vector2.zero;

		[ReadOnly]
		public Vector2 vectorC = Vector2.zero;

		// Used in the Editor (check VectorHelperExampleEditor)
		public bool IsVectorZero()
		{
			return vector.IsZero();
		}

		// Used in the Editor (check VectorHelperExampleEditor)
		public bool IsVectorCBetweenVectorsAAndB()
		{
			return VectorHelper.IsBetweenVectors( vectorC, vectorA, vectorB );
		}

		// Used in the Editor (check VectorHelperExampleEditor)
		public bool IsVectorCOnTheSameSideAsVectorBInRelationToA()
		{
			return VectorHelper.IsOnVectorSide( vectorC, vectorA, vectorB );
		}

		public void OnValidate()
		{
			SetVector( ref vectorA, vectorAAngle );
			SetVector( ref vectorB, vectorBAngle );
			SetVector( ref vectorC, vectorCAngle );
		}

		public void Reset()
		{
			OnValidate();
		}

		private void SetVector( ref Vector2 vector, float degrees )
		{
			float radians = degrees * Mathf.Deg2Rad;
			vector = new Vector2( Mathf.Cos( radians ), Mathf.Sin( radians ) );
		}
	}
}
