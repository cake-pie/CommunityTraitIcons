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

		public Sprite makeSprite()
		{
			if (Icon == null) return null;
			return Sprite.Create(Icon, new Rect(0, 0, Icon.width, Icon.height), new Vector2(0.5f, 0.5f));
		}

		public DialogGUIImage makeDialogGUIImage(Vector2 s, Vector2 p)
		{
			if (Icon == null || Color == null) return null;
			return new DialogGUIImage(s,p,Color,Icon);
		}

		public DialogGUISprite makeDialogGUISprite(Vector2 s, Vector2 p)
		{
			if (Icon == null || Color == null) return null;
			return new DialogGUISprite(s,p,Color,makeSprite());
		}
	}
}
