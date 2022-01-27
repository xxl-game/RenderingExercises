using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using GPUPerformanceAnalyser;

namespace UnityEditor
{
	public class GPUPerformanceAnalyserWindow : EditorWindow
	{
		private bool initialized = false;
		private int selectedOverlay = -1;
		private CameraClearFlags lastCamClearFlags;
		private Color lastCamBackgroundColor;
		private GUIStyle buttonStyle;
		private Texture2D overlayBcgTex;
		private bool visibleWireframe;
		private GPUPerformanceSceneScanner sceneScanner;
		private Vector2 scrollPosition;

		[MenuItem("Window/GPU Performance Analyser")]
		static void Init()
		{
			var window = (GPUPerformanceAnalyserWindow)EditorWindow.GetWindow(typeof(GPUPerformanceAnalyserWindow));
			window.titleContent = new GUIContent("GPU Analyser");
			window.Initialize();
			window.Show();
		}

		private void Initialize()
		{
			if(initialized)
				return;

			selectedOverlay = -1;
			ClearOverlay();

			sceneScanner = new GPUPerformanceSceneScanner(this);

			initialized = true;
		}

		protected void OnEnable()
		{
			if(!initialized)
				Initialize();

			Camera.onPreRender += OnCameraPreRender;
		}

		protected void OnDisable()
		{
			Camera.onPreRender -= OnCameraPreRender;
		}

		private void OnCameraPreRender(Camera cam)
		{
			if(selectedOverlay == -1)
				return;

			if(CurrentSceneView.camera != cam)
				return;

			if(overlayBcgTex == null)
			{
				overlayBcgTex = new Texture2D(2, 2);
				overlayBcgTex.SetPixels(new Color[] { Color.black, Color.black, Color.black, Color.black });
				overlayBcgTex.Apply();
			}

			Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), overlayBcgTex);
			UpdateCamera();
		}

		protected void Update()
		{
			UpdateCamera();

			Repaint();
		}

		public void OnPreSceneGUI()
		{
			UpdateCamera();
		}

		private void UpdateCamera()
		{
			if(selectedOverlay == -1)
				return;

			if(CurrentSceneView == null)
			{
				selectedOverlay = -1;
				return;
			}
			
			if(CurrentSceneView.camera.clearFlags != CameraClearFlags.SolidColor)
				CurrentSceneView.camera.clearFlags = CameraClearFlags.SolidColor;

			if(CurrentSceneView.camera.backgroundColor != Color.black)
				CurrentSceneView.camera.backgroundColor = Color.black;

			if(CurrentSceneView.camera.allowHDR != false)
				CurrentSceneView.camera.allowHDR = false;
		}

		protected void OnGUI()
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

			DrawOverlays();

			if(sceneScanner == null)
				sceneScanner = new GPUPerformanceSceneScanner(this);
			sceneScanner.DrawGUI();

			EditorGUILayout.EndScrollView();
		}

		protected void DrawOverlays()
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("Select overlay:", EditorStyles.boldLabel);

			if(buttonStyle == null)
			{
				buttonStyle = new GUIStyle(EditorStyles.miniButton);
				buttonStyle.alignment = TextAnchor.MiddleLeft;
				buttonStyle.fontSize = 13;
			}

			var overlays = OverlaysList.overlays;
			for(int i = 0; i < overlays.Count; i++)
			{
				if(GUILayout.Button(overlays[i].name, buttonStyle))
					SetOverlay(i);
			}

			if(GUILayout.Button("None (clear)", buttonStyle))
				ClearOverlay();

			EditorGUILayout.EndVertical();

			if(selectedOverlay == -1)
			{
				return;
			}

			EditorGUI.BeginChangeCheck();

			var currentOverlay = overlays[selectedOverlay];
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField(currentOverlay.name + ":", EditorStyles.boldLabel);
			EditorGUILayout.LabelField(currentOverlay.description, EditorStyles.helpBox);
			for(int i = 0; i < currentOverlay.overlayParams.Count; i++)
			{
				var param = currentOverlay.overlayParams[i];
				param.currentValue = EditorGUILayout.Slider(param.name, param.currentValue, param.minValue, param.maxValue);

				if(i == 0)
				{
					Shader.SetGlobalFloat("_OverlayParam", param.currentValue);
				}
				else
				{
					Shader.SetGlobalFloat("_OverlayParam" + (i + 1), param.currentValue);
				}
			}

			visibleWireframe = EditorGUILayout.Toggle("Show wireframe", visibleWireframe);

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			Vector2 colorPickerPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			Color pickedColor = InternalEditorUtility.ReadScreenPixel(colorPickerPos, 1, 1)[0];
			
			EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
			GUIStyle textStyle = new GUIStyle(EditorStyles.label);
			textStyle.normal.textColor = pickedColor;
			EditorGUILayout.LabelField("Color value under cursor: ", textStyle);
			EditorGUILayout.LabelField(pickedColor.ToString().Replace("RGBA", "RGB").Replace(", 1.000)", ")"));
			EditorGUILayout.EndHorizontal();

			if(currentOverlay.overlayParams.Count > 0 && currentOverlay.overlayParams[0].name.Contains("Count to color"))
			{
				int colorValToCount = Mathf.RoundToInt(pickedColor.r / currentOverlay.overlayParams[0].currentValue);
				EditorGUILayout.IntField("Value to count", colorValToCount);
				EditorGUILayout.HelpBox("Hint:\nTo make color picker pick only overdraw values: disable grids, wireframes and everything what change colors under mouse pointer."
										+ "\nLower color scaling makes wider range for value to count compute. Count is limited to 255.", MessageType.Info);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();

			if(EditorGUI.EndChangeCheck())
			{
				CurrentSceneView.renderMode = visibleWireframe ? DrawCameraMode.TexturedWire : DrawCameraMode.Textured;
				CurrentSceneView.Repaint();
				return;
			}
		}

		public void ClearOverlay()
		{
			if(selectedOverlay != -1)
				SetSavedPreOverlayCameraSetup();
			selectedOverlay = -1;

			if(CurrentSceneView)
			{
				CurrentSceneView.SetSceneViewShaderReplace(null, null);
				CurrentSceneView.renderMode = DrawCameraMode.Textured;
				CurrentSceneView.Repaint();
			}
		}

		public void SetOverlay(int overlayId)
		{
			if(overlayId < 0)
			{
				ClearOverlay();
				return;
			}

			if(selectedOverlay == -1)
			{
				SavePreOverlayCameraSetup();
			}

			selectedOverlay = overlayId;
			CurrentSceneView.SetSceneViewShaderReplace(OverlaysList.overlays[overlayId].Shader, "RenderType");
			CurrentSceneView.Repaint();
		}

		private void SavePreOverlayCameraSetup()
		{
			lastCamClearFlags = CurrentSceneView.camera.clearFlags;
			lastCamBackgroundColor = CurrentSceneView.camera.backgroundColor;
		}

		private void SetSavedPreOverlayCameraSetup()
		{
			CurrentSceneView.camera.clearFlags = lastCamClearFlags;
			CurrentSceneView.camera.backgroundColor = lastCamBackgroundColor;
		}

		public SceneView CurrentSceneView
		{
			get
			{
				SceneView currentSceneView = SceneView.currentDrawingSceneView;

				if(currentSceneView == null)
					currentSceneView = SceneView.lastActiveSceneView;

				if(currentSceneView == null && SceneView.sceneViews.Count > 0)
					currentSceneView = SceneView.sceneViews[0] as SceneView;

				if(currentSceneView == null)
					Debug.LogWarning("Missing scene view, enable one to use overlays");

				return currentSceneView;
			}
		}
	}
}