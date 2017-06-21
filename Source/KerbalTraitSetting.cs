using System;
using UnityEngine;

namespace CommunityTraitIcons
{
	public class KerbalTraitSetting
	{
		public string Name { get; }
		public Texture2D Icon { get; }
		public Color Color { get; }

		public KerbalTraitSetting(string name, Texture2D icon, Color color)
		{
			Name = name;
			Icon = icon;
			Color = color;
		}
	}
}
