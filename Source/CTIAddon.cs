using System;
using System.Collections.Generic;
using UnityEngine;
using Experience;

namespace CommunityTraitIcons
{
	// CTIAddon runs once when game first loads into the main menu scene, reads trait icon settings from config file, and makes them available for lookup.
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class CTIAddon : MonoBehaviour
	{
		// have the trait icon settings been loaded from file yet?
		public static bool Loaded { get; private set; }

		private static Dictionary<string, KerbalTraitSetting> traitSettings = new Dictionary<string, KerbalTraitSetting>();

		internal static void log(string s, params object[] m)
		{
			Debug.Log(string.Format("[Community Trait Icons] " + s, m));
		}

		private void Awake()
		{
			if (!Loaded)
			{
				LoadIcons();
				GameEvents.OnGameDatabaseLoaded.Add(LoadIcons);
			}

			// it's safe to self-destruct as soon as we're done
			// this also disposes of any spurious instances (see: https://forum.kerbalspaceprogram.com/index.php?/topic/7542-x/&do=findComment&comment=3574980)
			Destroy(gameObject);
		}

		// reads trait icon settings from config file
		private void LoadIcons()
		{
			Loaded = true;

			ExperienceSystemConfig esc = GameDatabase.Instance.ExperienceConfigs;

			traitSettings.Clear();
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
					if (!esc.TraitNames.Contains(traitName) && !traitName.Equals("Unknown"))
					{
						log("Unused Trait - " + traitName + " - load skipped");
						continue;
					}

					Texture2D icon = GameDatabase.Instance.GetTexture(nodes[i].GetValue("icon"), false);
					if (icon==null) {
						log("Texture load failed: " + nodes[i].GetValue("icon"));
						icon = GameDatabase.Instance.GetTexture("CommunityTraitIcons/Icons/errorIcon", false);
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

		// lookup trait icon settings by trait name (protoCrew.experienceTrait.Config.Name)
		public static KerbalTraitSetting getTrait(string traitName)
		{
			if (!Loaded) return null;
			if (traitSettings.ContainsKey(traitName))
				return traitSettings[traitName];
			else
				return traitSettings["Unknown"];
		}

	}
}
