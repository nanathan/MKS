using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USITools;
using UnityEngine;

namespace KolonyTools
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class RecyclePartToggle : MonoBehaviour
    {
        private IButton button;

        public static bool RecycleEnabled
        {
            get
            {
                Initialize();
                return recycleEnabled;
            }
        }
        private static bool initialized;
        private static bool recycleEnabled;

        private static void Initialize()
        {
            if(!initialized)
            {
                ConfigNode[] nodes = GameDatabase.Instance.GetConfigNodes("KOLONIZATION_SETTINGS");
                if(nodes != null && nodes.Length > 0)
                {
                    string v = nodes[0].GetValue("RecyclePartDefaultEnabled");
                    recycleEnabled = v == null || !(string.Compare(v, "off", true) == 0) && !(string.Compare(v, "false", true) == 0);
                }
                Debug.Log("MKS part recycling defaulting to " + recycleEnabled);
                initialized = true;
            }
        }

        public void Start()
        {
            if(ToolbarManager.ToolbarAvailable)
            {
                button = ToolbarManager.Instance.add("UKS", "recyclePartToggle");
                button.TexturePath = "UmbraSpaceIndustries/Recycle24";
                button.ToolTip = "USI Part Recycling";
                button.Enabled = true;
                button.OnClick += (e) => ToggleRecycling();
            }
        }

        public void OnDestroy()
        {
            if(button != null)
            {
                button.Destroy();
                button = null;
            }
        }
        
        private void ToggleRecycling()
        {
            Initialize();
            recycleEnabled = !recycleEnabled;
            string message = string.Format("Part recycling is now {0}", (recycleEnabled ? "enabled" : "disabled"));
            ScreenMessages.PostScreenMessage(message, 6f, ScreenMessageStyle.UPPER_CENTER);
        }
    }
}
