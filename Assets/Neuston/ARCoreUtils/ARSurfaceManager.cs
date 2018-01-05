using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class ARSurfaceManager : MonoBehaviour
{
	[SerializeField] Material m_surfaceMaterial;
	List<TrackedPlane> m_newPlanes = new List<TrackedPlane>();

	void Update()
	{
#if UNITY_EDITOR
		return;
#endif

		if (Frame.TrackingState != TrackingState.Tracking)
		{
			return;
		}

		Frame.GetPlanes(m_newPlanes, TrackableQueryFilter.New);

		foreach (var plane in m_newPlanes)
		{
			var surfaceObj = new GameObject("ARSurface");
			surfaceObj.AddComponent<ARSurface>().SetTrackedPlane(plane, m_surfaceMaterial);
		}
	}
}
