using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUPerformanceAnalyser
{
	[System.Serializable]
	public class OverlayData
	{

		public string name;
		public string shaderPath;
		public bool allowColorPicker;
		public string description;
		public List<OverlayParam> overlayParams;

		private Shader shader;

		public OverlayData() { }

		public OverlayData(string name, string shaderPath, bool allowColorPicker, string description, List<OverlayParam> overlayParams)
		{
			this.name = name;
			this.shaderPath = shaderPath;
			this.description = description;
			this.overlayParams = overlayParams;
		}

		public Shader Shader
		{
			get
			{
				if(shader == null)
					shader = Shader.Find(shaderPath);

				return shader;
			}
		}
	}
}