using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

public class GPUPerformanceSceneScanner
{
	private const int lowPolyCount = 500;
	private const int midPolyCount = 2000;
	private const int highPolyCount = 5000;
	private const int largeAmountOfGameObjects = 100000;
	private const int mediumAmountOfGameObjects = 10000;
	private bool show = true;
	private Dictionary<string, bool> showResult = new Dictionary<string, bool>();
	private GPUPerformanceSceneScanResults scanResults;
	private GUIStyle foldoutStyle;

	public GPUPerformanceSceneScanner(GPUPerformanceAnalyserWindow parent)
	{
		Parent = parent;
	}

	public void DrawGUI()
	{
		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		if(foldoutStyle == null)
		{
			foldoutStyle = new GUIStyle(EditorStyles.foldout);
			foldoutStyle.fontStyle = FontStyle.Bold;
		}
		if(!(show = EditorGUILayout.Foldout(show, "Scene performance scanner", true, foldoutStyle)))
		{
			EditorGUILayout.EndVertical();
			return;
		}

		if(GUILayout.Button("Scan current scene"))
			ScanScene();

		DrawScanResults();

		EditorGUILayout.EndVertical();
	}

	private void ScanScene()
	{
		scanResults = new GPUPerformanceSceneScanResults();
		try
		{
			CheckCameras();
			CheckGameObjects();
		}
		catch(Exception e)
		{
			scanResults = null;
			Debug.LogError("Failed scanning!");
			Debug.LogError(e);
			EditorUtility.ClearProgressBar();
		}

		if(scanResults != null)
			EditorUtility.DisplayDialog("Scanning finished", "Scene scanning finished successfully!", "OK");
		else
			EditorUtility.DisplayDialog("Scanning finished", "Scene scanning failed, please check console logs to learn more", "OK");
	}

	private void CheckCameras()
	{
		scanResults.camerasWithoutOcclusionCulling = new List<Camera>();
		Camera[] cams = GameObject.FindObjectsOfType<Camera>();
		foreach(Camera cam in cams)
		{
			if(!cam.useOcclusionCulling && !cam.name.Contains("GUI") && cam.GetComponent<Canvas>() == null)
				scanResults.camerasWithoutOcclusionCulling.Add(cam);
		}
	}

	private void CheckGameObjects()
	{
		scanResults.dynamicRenderers = new List<Renderer>();
		scanResults.dynamicRenderersWithoutInstancing = new List<Renderer>();
		scanResults.highPolyRenderersWithoutLODs = new List<Renderer>();
		scanResults.dynamicColliders = new List<Collider>();
		scanResults.highPolyColliders = new List<MeshCollider>();
		scanResults.gameObjectsCounter = 0;
		scanResults.dynamicLights = new List<Light>();
		scanResults.dynamicShadowCastingLights = new List<Light>();

		EditorUtility.DisplayProgressBar("Checking game objects", "", 0);
		GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
		for(int i = 0; i < gameObjects.Length; i++)
		{
			EditorUtility.DisplayProgressBar("Checking game objects", String.Format("{0}/{1}", i, gameObjects.Length), (float)i / gameObjects.Length);

			GameObject go = gameObjects[i];
			
			CheckRenderer(go);
			CheckCollider(go);
			CheckLight(go);
			scanResults.gameObjectsCounter++;
		}
		EditorUtility.ClearProgressBar();
	}

	private void CheckRenderer(GameObject go)
	{
		Renderer rend = go.GetComponent<Renderer>();
		if(rend == null)
			return;

		MeshFilter mf = go.GetComponent<MeshFilter>();
		if(mf == null || mf.sharedMesh == null)
			return;

		int polyCount = 0;
		for(int i = 0; i < mf.sharedMesh.subMeshCount; i++)
			polyCount += (int)mf.sharedMesh.GetIndexCount(i);
		polyCount /= 3;

		if(polyCount > highPolyCount)
		{
			if(!go.GetComponentInParent<LODGroup>())
				scanResults.highPolyRenderersWithoutLODs.Add(rend);
		}
		
		if(go.isStatic)
			return;

		scanResults.dynamicRenderers.Add(rend);

		if(rend.sharedMaterial && !rend.sharedMaterial.enableInstancing)
			scanResults.dynamicRenderersWithoutInstancing.Add(rend);
	}

	private void CheckCollider(GameObject go)
	{
		Collider collider = go.GetComponent<Collider>();
		if(collider == null)
			return;
		
		if(!go.isStatic)
			scanResults.dynamicColliders.Add(collider);

		MeshCollider meshCollider = go.GetComponent<MeshCollider>();
		if(meshCollider == null || meshCollider.sharedMesh == null)
			return;

		int polyCount = 0;
		for(int i = 0; i < meshCollider.sharedMesh.subMeshCount; i++)
			polyCount += (int)meshCollider.sharedMesh.GetIndexCount(i);
		polyCount /= 3;

		if(polyCount > lowPolyCount)
		{
			scanResults.highPolyColliders.Add(meshCollider);
		}
	}

	private void CheckLight(GameObject go)
	{
		Light light = go.GetComponent<Light>();
		if(light == null)
			return;

		if(go.isStatic)
			return;

		scanResults.dynamicLights.Add(light);

		if(light.shadows != LightShadows.None)
			scanResults.dynamicShadowCastingLights.Add(light);
	}

	private void DrawScanResults()
	{
		if(scanResults == null)
			return;

		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		EditorGUILayout.LabelField("Results:", EditorStyles.boldLabel);

		Color colorTmp = GUI.color;
		if(scanResults.gameObjectsCounter > largeAmountOfGameObjects)
			GUI.color = Color.red;
		else if(scanResults.gameObjectsCounter > mediumAmountOfGameObjects)
			GUI.color = Color.yellow;
		else
			GUI.color = Color.green;
		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		EditorGUILayout.LabelField("Game Objects count: " + scanResults.gameObjectsCounter);
		EditorGUILayout.EndVertical();
		GUI.color = colorTmp;

		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		EditorGUILayout.LabelField("Cameras");
		DrawList<Camera>(scanResults.camerasWithoutOcclusionCulling, "Cameras without occlusion culling",
			"HINT: It's good to have occlusion culling enabled in gameplay camera to greatly reduce amount of drawcalls when objects are hidden behind another one.");
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		EditorGUILayout.LabelField("Renderers");
		DrawList<Renderer>(scanResults.dynamicRenderers, "Dynamic renderers",
			"HINT: Try reduce amount of dynamic renderes and replace them with static objects. You will reduce draw calls count by that.");
		DrawList<Renderer>(scanResults.dynamicRenderersWithoutInstancing, "Dynamic renderers without instancing",
			"HINT: If you use many the same dynamic renderers, check if enabling instancing for their materials can improve game performance.");
		DrawList<Renderer>(scanResults.highPolyRenderersWithoutLODs, "High poly renderers without LODs",
			"HINT: High poly models are not well visible from distance but they still require a lot form GPU (because of polygons count). Add LOD Groups and LOD levels for them to improve performance.");
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		EditorGUILayout.LabelField("Colliders");
		DrawList<Collider>(scanResults.dynamicColliders, "Dynamic colliders",
			"HINT: Dynamic colliders are expensive because they are ready to move. Try change them into static");
		DrawList<MeshCollider>(scanResults.highPolyColliders, "High poly colliders",
			"HINT: Colliders with large amount of polygons are expensive for physics. Try replace them with boxes or lower poly and convex version of them. In most cases simillar shapes are enough to cover collisions.");
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		EditorGUILayout.LabelField("Lights");
		DrawList<Light>(scanResults.dynamicLights, "Dynamic lights",
			"HINT: Try reduce dynamic lights and use static ones because they are cheaper and require less work for GPU. Consider also use of LightProbes.");
		DrawList<Light>(scanResults.dynamicShadowCastingLights, "Dynamic shadow casting lights",
			"HINT: Shadow casting is very expensive thing, highly recommended only 1 shadow casting light.");
		EditorGUILayout.EndVertical();

		EditorGUILayout.EndVertical();
	}

	private void DrawList<T>(List<T> list, string title, string hint) where T : Component
	{
		Color cachedColor = GUI.color;
		bool show = false;
		showResult.TryGetValue(title, out show);
		if(show)
			GUI.color = new Color(1f, 0.9f, 0.8f);

		if(show = EditorGUILayout.Foldout(show, title, true))
		{
			EditorGUI.indentLevel++;

			if(list.Count > 0)
				EditorGUILayout.HelpBox(hint, MessageType.Info, true);
			
			GUI.color = new Color(1f, 0.8f, 0.8f);
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			foreach(T element in list)
			{
				EditorGUILayout.ObjectField(element, typeof(T), true);
			}
			EditorGUILayout.EndVertical();

			EditorGUI.indentLevel--;
		}
		showResult[title] = show;

		GUI.color = cachedColor;
	}

	public GPUPerformanceAnalyserWindow Parent { get; private set; }
}
