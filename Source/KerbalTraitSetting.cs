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
			Color = color;

			if (icon == null) return;

			CTIAddon.log("Tinting icon for {0}.", name);
			Icon = new Texture2D(icon.width, icon.height, TextureFormat.ARGB32, false);  // disable mipmap!
			Color32[] tex;
			try
			{
				// readable textures
				tex = icon.GetPixels32();
			}
			catch (UnityException e)
			{
				// workaround for unreadable textures
				RenderTexture bak = RenderTexture.active;
				RenderTexture tmp = RenderTexture.GetTemporary(icon.width, icon.height, 0);
				Graphics.Blit(icon, tmp);
				RenderTexture.active = tmp;
				Icon.ReadPixels(new Rect(0, 0, icon.width, icon.height), 0, 0);
				RenderTexture.active = bak;
				RenderTexture.ReleaseTemporary(tmp);
				tex = Icon.GetPixels32();
			}
			for (int i = 0; i < tex.Length; i++)
				tex[i] = tex[i] * Color;
			Icon.SetPixels32(tex);
			Icon.Apply(false,true);
		}

		#region Icon Generation
		// Generate a Sprite of the icon for this trait.
		public Sprite makeSprite()
		{
			if (Icon == null) return null;
			return Sprite.Create(Icon, new Rect(0, 0, Icon.width, Icon.height), new Vector2(0.5f, 0.5f));
		}

		// Generate a KSP DialogGUIImage of the icon for this trait.
		public DialogGUIImage makeDialogGUIImage(Vector2 s, Vector2 p)
		{
			if (Icon == null) return null;
			return new DialogGUIImage(s,p,Color.white,Icon);
		}

		// Generate a KSP DialogGUISprite of the icon for this trait.
		public DialogGUISprite makeDialogGUISprite(Vector2 s, Vector2 p)
		{
			if (Icon == null) return null;
			return new DialogGUISprite(s,p,Color.white,makeSprite());
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
			return true;
		}
		#endregion Icon Generation
	}
}
