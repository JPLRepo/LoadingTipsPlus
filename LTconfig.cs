/* 
LoadingTipsPlus by JPLRepo is released under the following license:
Copyright © Jamie Leighton, 2016 All Rights Reserved
For more info visit the KSP Forum Thread
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingTipsPlus
{
    class LTconfig
    {
        private const string configNodeName = "LOADINGTOOLTIPSPLUS";

        public bool Overwrite;
        public List<string> ToolTips;

        public LTconfig()
        {
            Overwrite = false;
            ToolTips = new List<string>();
        }

        public void Load(ConfigNode node)
        {
            if (node.HasNode(configNodeName)) //Load SFS config nodes
            {
                ConfigNode LTsettingsNode = node.GetNode(configNodeName);

                LTsettingsNode.TryGetValue("Overwrite", ref Overwrite);
                string[] values = LTsettingsNode.GetValuesStartsWith("ToolTip");
                for (int i = 0; i < values.Length; i++)
                {
                    ToolTips.Add(values[i]);
                }
            }
        }
    }
}
