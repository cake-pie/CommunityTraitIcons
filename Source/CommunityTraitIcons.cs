﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Experience;

namespace CommunityTraitIcons
{
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class CommunityTraitIcons : MonoBehaviour
	{
		private static bool loaded = false;
		private static Dictionary<string, KerbalTraitSetting> traitSettings = new Dictionary<string, KerbalTraitSetting>();

		internal static void log(string s, params object[] m)
		{
			Debug.Log(string.Format("[Community Trait Icons] " + s, m));
		}

		private void Start()
		{
			if (!loaded)
			{
				loaded = true;

				ExperienceSystemConfig esc = new ExperienceSystemConfig();
				esc.LoadTraitConfigs();

				ConfigNode node = GameDatabase.Instance.GetConfigNode("CommunityTraitIcons/CommunityTraitIcons/CommunityTraitIcons");
				if (node != null)
				{
					var nodes = node.GetNodes("Trait");

					for (int i = 0; i < nodes.Length; i++)
					{
						if (!nodes[i].HasValue("name") || !nodes[i].HasValue("icon") || !nodes[i].HasValue("color"))
						{
							log("Invalid Trait node format - load failed - skipped");
							continue;
						}

						string traitName = nodes[i].GetValue("name");
						if (!esc.TraitNames.Contains(traitName))
						{
							log("Unused Trait - " + traitName + " - load skipped");
							continue;
						}

						Texture2D icon = GameDatabase.Instance.GetTexture(nodes[i].GetValue("icon"), false);
						if (icon==null) {
							log("Texture load failed: " + nodes[i].GetValue("icon"));
							// TODO create substitute texture for cases of failure to load
							icon = GameDatabase.Instance.GetTexture("CommunityTraitIcons/Icons/questionIcon", false);
						}

						var newTrait = new KerbalTraitSetting(
							traitName,
							icon,
							parseColor(nodes[i], "color", XKCDColors.White)
						);
						try
						{
							traitSettings.Add(traitName, newTrait);
						}
						catch (Exception ex)
						{
							log("Error: {0}", ex.Message);
						}
					}
				}
				else
				{
					log("Could not find trait settings config - using defaults");
					traitSettings.Add("Pilot", new KerbalTraitSetting("Pilot", GameDatabase.Instance.GetTexture("CommunityTraitIcons/Icons/pilotIcon", false), XKCDColors.PastelRed));
					traitSettings.Add("Engineer", new KerbalTraitSetting("Engineer", GameDatabase.Instance.GetTexture("CommunityTraitIcons/Icons/engineerIcon", false), XKCDColors.DarkYellow));
					traitSettings.Add("Scientist", new KerbalTraitSetting("Scientist", GameDatabase.Instance.GetTexture("CommunityTraitIcons/Icons/scientistIcon", false), XKCDColors.DirtyBlue));
					traitSettings.Add("Tourist", new KerbalTraitSetting("Tourist", GameDatabase.Instance.GetTexture("CommunityTraitIcons/Icons/touristIcon", false), XKCDColors.SapGreen));
				}

				if (!traitSettings.ContainsKey("Unknown"))
					traitSettings.Add("Unknown", new KerbalTraitSetting("Unknown", GameDatabase.Instance.GetTexture("CommunityTraitIcons/Icons/questionIcon", false), XKCDColors.White));
			}
		}

		private Color parseColor(ConfigNode node, string name, Color c)
		{
			try
			{
				return ConfigNode.ParseColor(node.GetValue(name));
			}
			catch
			{
				return c;
			}
		}

		public static KerbalTraitSetting getTrait(string traitName)
		{
			KerbalTraitSetting result;
			if (traitSettings.TryGetValue(traitName, out result))
				return result;
			else
				return null;
		}

	}
}
