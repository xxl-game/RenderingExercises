using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUPerformanceAnalyser
{
	public static class OverlaysList
	{
		public static List<OverlayData> overlays = new List<OverlayData>()
		{
			new OverlayData("[Opaque] Overdraw", "Hidden/MODev/GpuPA/PolygonCountOverlay", true, 
							"Shows count of all opaque polygons on scene per pixel (disabled front and back culling).\n",
							// + "If you set scaling to lowest value, the value picker will show you true count of polygons under cursor.", 
							new List<OverlayParam>(){ new OverlayParam("Count to color scaling", 1/255f, 0.5f, 0.1f)}),

			new OverlayData("[Opaque] Visible polygons", "Hidden/MODev/GpuPA/VisiblePolygonCountOverlay", true,
							"Shows count of visible opaque polygons on scene (those turned front to camera). In short shows overdraw of opaque objects.\n",
							// + "If you set scaling to lowest value, the value picker will show you true count of polygons under cursor.",
							new List<OverlayParam>(){ new OverlayParam("Count to color scaling", 1/255f, 0.5f, 0.1f)}),

			new OverlayData("[Opaque] Wasted invisible polygons", "Hidden/MODev/GpuPA/WastedPolygonsCountOverlay", true,
							"Shows count of invisible opaque polygons on scene (those turned back to camera but still needed to be culled and kept in memory).\n",
							// + "If you set scaling to lowest value, the value picker will show you true count of polygons under cursor.",
							new List<OverlayParam>(){ new OverlayParam("Count to color scaling", 1/255f, 0.5f, 0.1f)}),

			new OverlayData("[Opaque] Wasted invisible polygons textured", "Hidden/MODev/GpuPA/WastedPolygonsTexturedOverlay", false,
							"Shows invisible opaque polygons on scene (those turned back to camera but still needed to be culled and kept in memory).\n",
							new List<OverlayParam>(){ new OverlayParam("Visibility", 1/255f, 1f, 0.5f)}),

			new OverlayData("[Transparency] Overdraw", "Hidden/MODev/GpuPA/TransparencyOverdrawOverlay", true,
							"Shows overdraw of transparency on scene. Culling is disabled for better effect.",
							// + "If you set scaling to lowest value, the value picker will show you true count of polygons under cursor.",
							new List<OverlayParam>(){ new OverlayParam("Count to color scaling", 1/255f, 0.5f, 0.1f)}),

			/*new OverlayData("[Transparency] Textured wasted alpha blending", "Hidden/MODev/GpuPA/TransparencyTexturedOverlay", false,
							"Shows all transparent polygons with wasted alpha blending (wasted alpha blending become more redish).",
							new List<OverlayParam>(){ new OverlayParam("Visibility", 1/255f, 1f, 0.5f)}),*/

			new OverlayData("[Transparency] Wasted alpha", "Hidden/MODev/GpuPA/TransparencyWastedOverlay", false,
							"Shows where transparency is wasted. More redish fields on screen means more GPU cost for rendering. Culling is disabled for better effect.",
							new List<OverlayParam>()
							{
								new OverlayParam("Wasted alpha visibility", 1/255f, 1f, 0.5f),
								new OverlayParam("Texture visibility", 0f, 1f, 0.4f),
							}),
		};
	}
}