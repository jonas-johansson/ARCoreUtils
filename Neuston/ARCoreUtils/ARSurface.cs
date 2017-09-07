using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class ARSurface : MonoBehaviour
{
	TrackedPlane m_trackedPlane;
	MeshCollider m_meshCollider;
	MeshFilter m_meshFilter;
	MeshRenderer m_meshRenderer;
	List<Vector3> m_points = new List<Vector3>();
	Mesh m_mesh;

	void Awake()
	{
		m_meshCollider = gameObject.AddComponent<MeshCollider>();
		m_meshFilter = gameObject.AddComponent<MeshFilter>();
		m_meshRenderer = gameObject.AddComponent<MeshRenderer>();

		m_mesh = new Mesh();
		m_meshFilter.mesh = m_mesh;
		m_meshCollider.sharedMesh = m_mesh;
	}

	public void SetTrackedPlane(TrackedPlane plane, Material material)
	{
		m_trackedPlane = plane;
		m_meshRenderer.material = material;
		UpdateMesh();
	}

	void Update()
	{
		if (m_trackedPlane == null)
		{
			return;
		}
		else if (m_trackedPlane.SubsumedBy != null)
		{
			Destroy(gameObject);
			return;
		}
		else if (!m_trackedPlane.IsValid || Frame.TrackingState != FrameTrackingState.Tracking)
		{
			m_meshRenderer.enabled = false;
			m_meshCollider.enabled = false;
			return;
		}

		m_meshRenderer.enabled = true;
		m_meshCollider.enabled = true;

		if (m_trackedPlane.IsUpdated)
		{
			UpdateMesh();
		}
	}

	void UpdateMesh()
	{
		m_trackedPlane.GetBoundaryPolygon(ref m_points);

		int[] indices = TriangulatorXZ.Triangulate(m_points);

		m_mesh.Clear();
		m_mesh.SetVertices(m_points);
		m_mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		m_mesh.RecalculateBounds();

		m_meshCollider.sharedMesh = null;
		m_meshCollider.sharedMesh = m_mesh;
	}
}