using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BLINDED_AM_ME{
	
	public class Mesh_Maker{

		// Mesh Values
		private List<Vector3>   _vertices  = new List<Vector3>();
		private List<Vector3>   _normals   = new List<Vector3>();
		private List<Vector2>   _uvs       = new List<Vector2>();
		private List<Vector4>   _tangents  = new List<Vector4>();
	    private List<List<int>> _subIndices = new List<List<int>>();
        
	    public int VertCount
	    {
	        get { return _vertices.Count; }
	    }

	    public void AddTriangle(
			Vector3[] vertices,
			Vector3[] normals,
			Vector2[] uvs,
			int       submesh){

			int vertCount = _vertices.Count;

			_vertices.Add(vertices[0]);
			_vertices.Add(vertices[1]);
			_vertices.Add(vertices[2]);

			_normals.Add(normals[0]);
			_normals.Add(normals[1]);
			_normals.Add(normals[2]);

			_uvs.Add(uvs[0]);
			_uvs.Add(uvs[1]);
			_uvs.Add(uvs[2]);

	        if (_subIndices.Count < submesh + 1)
	        {
	            for (int i = _subIndices.Count; i < submesh + 1; i++)
	            {
	                _subIndices.Add(new List<int>());
	            }
	        }

	        _subIndices[submesh].Add(vertCount);
			_subIndices[submesh].Add(vertCount+1);
			_subIndices[submesh].Add(vertCount+2);
		}

	    public void AddTriangle(
	        Vector3[] vertices,
	        Vector3[] normals,
	        Vector2[] uvs,
	        Vector4[] tangents,
	        int submesh)
	    {
	        int vertCount = _vertices.Count;

	        _vertices.Add(vertices[0]);
	        _vertices.Add(vertices[1]);
	        _vertices.Add(vertices[2]);

	        _normals.Add(normals[0]);
	        _normals.Add(normals[1]);
	        _normals.Add(normals[2]);

	        _uvs.Add(uvs[0]);
	        _uvs.Add(uvs[1]);
	        _uvs.Add(uvs[2]);

	        _tangents.Add(tangents[0]);
	        _tangents.Add(tangents[1]);
	        _tangents.Add(tangents[2]);

	        if (_subIndices.Count < submesh + 1)
	        {
	            for (int i = _subIndices.Count; i < submesh + 1; i++)
	            {
	                _subIndices.Add(new List<int>());
	            }
	        }

	        _subIndices[submesh].Add(vertCount);
	        _subIndices[submesh].Add(vertCount + 1);
	        _subIndices[submesh].Add(vertCount + 2);
	    }

	    public void RemoveDoubles()
	    {
	        int dubCount = 0;

	        Vector3 vertex = Vector3.zero;
	        Vector3 normal = Vector3.zero;
	        Vector2 uv = Vector2.zero;

	        int i = 0;
	        while (i < VertCount)
	        {
	            vertex = _vertices[i];
	            normal = _normals[i];
	            uv = _uvs[i];

	            // look backward for a match
	            for (int b = i - 1; b >= 0; b--)
	            {
	                if (vertex == _vertices[b] &&
	                    normal == _normals[b] &&
	                    uv == _uvs[b])
	                {
	                    dubCount++;
	                    DoubleFound(b, i);
	                    i--;
	                    break; // there should only be one
	                }
	            }

	            i++;

	        } // while

	        Debug.LogFormat("Doubles found {0}", dubCount);
	    }

	    private void DoubleFound(int first, int duplicate)
	    {
	        // go through all indices an replace them

	        for (int h = 0; h < _subIndices.Count; h++)
	        {
	            for (int i = 0; i < _subIndices[h].Count; i++)
	            {

	                if (_subIndices[h][i] > duplicate) // knock it down
	                    _subIndices[h][i]--;
	                else if (_subIndices[h][i] == duplicate) // replace
	                    _subIndices[h][i] = first;
	            }
	        }

	        _vertices.RemoveAt(duplicate);
	        _normals.RemoveAt(duplicate);
	        _uvs.RemoveAt(duplicate);
	        _tangents.RemoveAt(duplicate);
	    }

	    /// <summary>
		/// Creates and returns a new mesh
		/// </summary>
		public Mesh GetMesh(){
			
			Mesh shape = new Mesh();
			shape.name =  "Generated Mesh";
			shape.SetVertices(_vertices);
			shape.SetNormals(_normals);
			shape.SetUVs(0, _uvs);
			shape.SetUVs(1, _uvs);

			if(_tangents.Count > 1)
				shape.SetTangents(_tangents);

			shape.subMeshCount = _subIndices.Count;

			for(int i=0; i<_subIndices.Count; i++)
				shape.SetTriangles(_subIndices[i], i);

			return shape;
		}

		/// <summary>
		/// Creates and returns a new mesh with generated lightmap uvs
		/// </summary>
		public Mesh GetMesh_GenerateSecondaryUVSet(){

			Mesh shape = GetMesh();

			#if UNITY_EDITOR
			// for light mapping
			UnityEditor.Unwrapping.GenerateSecondaryUVSet(shape);

			#endif

			return shape;
		}

	    /// <summary>
	    /// Creates and returns a new mesh with generated lightmap uvs
	    /// </summary>
	    public Mesh GetMesh_GenerateSecondaryUVSet(UnityEditor.UnwrapParam param)
	    {
	        Mesh shape = GetMesh();

#if UNITY_EDITOR
	        // for light mapping
	        UnityEditor.Unwrapping.GenerateSecondaryUVSet(shape, param);
#endif
	        return shape;
	    }
	}
}