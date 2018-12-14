using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech
{
	public static class ButtonExtension
	{
		public static void SetNormalColor (this Button button, Color color)
		{
			ColorBlock colorBlock = button.colors;
			colorBlock.normalColor = color;
			button.colors = colorBlock;
		}
	}
}