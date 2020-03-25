using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Tool that lets you mirror several Transforms in the hierarchy given a reflect normal.
	/// Useful when level designing.
	/// </summary>
	public sealed class MirrorTool : MonoBehaviour
	{
		/// <summary>
		/// The normal vector that will be used to reflect the selected Transforms.
		/// </summary>
		public Vector3 reflectNormal = Vector3.left;

		/// <summary>
		/// The Transforms that will be mirrored.
		/// </summary>
		[Header( "Select things to be mirrored" )]
		public List<Transform> sourceRoots;

		/// <summary>
		/// The copyed Transforms.
		/// </summary>
		[Header( "Do NOT modify this" )]
		[ReadOnly]
		public List<Transform> destinationRoots;

		/// <summary>
		/// Creates the mirror.
		/// Also destroys it if any existed before.
		/// </summary>
		[Button]
		public void CreateMirror()
		{
			if( Application.isPlaying )
				return;

			DestroyMirror();

			foreach( Transform root in sourceRoots )
				destinationRoots.Add( MirrorRoot( root ) );
		}

		/// <summary>
		/// Destroy the mirrored transforms.
		/// </summary>
		[Button]
		public void DestroyMirror()
		{
			if( Application.isPlaying )
				return;

			foreach( Transform root in destinationRoots )
			{
				if( root != null )
					DestroyImmediate( root.gameObject );
			}

			destinationRoots.Clear();
		}

		private Transform MirrorRoot( Transform root )
		{
			Transform mirror = Instantiate( root.gameObject ).transform;
			mirror.name = string.Concat( root.name, "_Mirrored" );
			mirror.parent = root.parent;

			foreach( Transform child in mirror.GetComponentsInChildren<Transform>( true ) )
				child.localPosition = Vector3.Reflect( child.localPosition, reflectNormal );

			foreach( MeshFilter meshFilter in mirror.GetComponentsInChildren<MeshFilter>( true ) )
				meshFilter.sharedMesh = MirrorMesh( meshFilter.sharedMesh );

			foreach( MeshCollider meshCollider in mirror.GetComponentsInChildren<MeshCollider>( true ) )
				meshCollider.sharedMesh = MirrorMesh( meshCollider.sharedMesh );

			return mirror.transform;
		}

		private Mesh MirrorMesh( Mesh srcMesh )
		{
			if( srcMesh == null )
				return null;

			Mesh dstMesh = new Mesh();

			dstMesh.vertices = MirrorArray( srcMesh.vertices );
			dstMesh.triangles = MirrorTriangles( srcMesh.triangles );
			dstMesh.uv = srcMesh.uv;
			dstMesh.uv2 = srcMesh.uv2;
			dstMesh.normals = MirrorArray( srcMesh.normals );
			dstMesh.colors = srcMesh.colors;
			dstMesh.tangents = srcMesh.tangents;

			dstMesh.UploadMeshData( true );

			return dstMesh;
		}

		private Vector3[] MirrorArray( Vector3[] srcArray )
		{
			Vector3[] dstArray = new Vector3[srcArray.Length];

			for( int i = 0; i < srcArray.Length; i++ )
				dstArray[i] = Vector3.Reflect( srcArray[i], reflectNormal );

			return dstArray;
		}

		private int[] MirrorTriangles( int[] srcTriangles )
		{
			int[] dstTriangles = new int[srcTriangles.Length];
			System.Array.Copy( srcTriangles, dstTriangles, srcTriangles.Length );
			System.Array.Reverse( dstTriangles );

			return dstTriangles;
		}
	}
}
