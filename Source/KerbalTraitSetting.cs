using System;
using UnityEngine;

namespace CommunityTraitIcons
{
	public class KerbalTraitSetting
	{
		public string Name { get; set; }
		public Texture2D Icon { get; set; }
		public Color Color { get; set; }

		public KerbalTraitSetting() {}

		public KerbalTraitSetting(string name, Texture2D icon, Color color)
		{
			Name = name;
			Icon = icon;
			Color = color;
		}
	}
}
