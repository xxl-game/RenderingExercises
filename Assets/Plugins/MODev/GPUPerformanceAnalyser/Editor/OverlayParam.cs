using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUPerformanceAnalyser
{
	[System.Serializable]
	public class OverlayParam
	{
		public string name;
		public float minValue;
		public float maxValue;
		public float currentValue;

		public OverlayParam() { }

		public OverlayParam(string name, float minValue, float maxValue, float defaultValue)
		{
			this.name = name;
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.currentValue = defaultValue;
		}
	}
}