using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUPerformanceSceneScanResults
{
	public List<Camera> camerasWithoutOcclusionCulling;
	public List<Renderer> dynamicRenderers;
	public List<Collider> dynamicColliders;
	public List<MeshCollider> highPolyColliders;
	public List<Renderer> dynamicRenderersWithoutInstancing;
	public int gameObjectsCounter;
	public List<Renderer> highPolyRenderersWithoutLODs;
	public List<Light> dynamicLights;
	public List<Light> dynamicShadowCastingLights;
}
