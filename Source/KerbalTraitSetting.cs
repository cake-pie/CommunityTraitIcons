using System;
using UnityEngine;
using UnityEngine.UI;

namespace CommunityTraitIcons
{
	public class KerbalTraitSetting
	{
		private const int UILayer = 5;

		public string Name { get; }
		public Texture2D Icon { get; }
		public Color Color { get; }

		public KerbalTraitSetting(string name, Texture2D icon, Color color)
		{
			Name = name;
			Icon = icon;
			Color = color;
		}

		#region Icon Generation
		// Generate a Sprite of the icon for this trait. Note: not tinted with color.
		public Sprite makeSprite()
		{
			if (Icon == null) return null;
			return Sprite.Create(Icon, new Rect(0, 0, Icon.width, Icon.height), new Vector2(0.5f, 0.5f));
		}

		// Generate a KSP DialogGUIImage of the icon for this trait.
		public DialogGUIImage makeDialogGUIImage(Vector2 s, Vector2 p)
		{
			if (Icon == null) return null;
			return new DialogGUIImage(s,p,Color,Icon);
		}

		// Generate a KSP DialogGUISprite of the icon for this trait.
		public DialogGUISprite makeDialogGUISprite(Vector2 s, Vector2 p)
		{
			if (Icon == null) return null;
			return new DialogGUISprite(s,p,Color,makeSprite());
		}

		// Generate a GameObject with an Image component of the icon for this trait.
		// Also comes with RectTransform and CanvasRenderer.
		// After obtaining the GameObject, you will most likely likely want to:
		//  - Set its parent: go.transform.SetParent(parent, false);
		//  - Make it active: go.SetActive(true);
		//  - Position it manually by manipulating its RectTransform, or automatically using Unity's auto layout system
		public GameObject makeGameObject()
		{
			if (Icon == null) return null;
			GameObject icon = new GameObject("CrewTraitIcon", typeof(RectTransform), typeof(CanvasRenderer));
			icon.layer = UILayer;
			attachImage(icon);
			return icon;
		}

		// Attach an Image component with the icon for this trait to an existing GameObject.
		public bool attachImage(GameObject go)
		{
			if (Icon == null) return false;
			Image i = go.AddComponent<Image>();
			i.sprite = makeSprite();
			i.color = Color;
			return true;
		}
		#endregion Icon Generation
	}
}
