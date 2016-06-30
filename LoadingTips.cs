/* 
LoadingTipsPlus by JPLRepo is released under the following license:
Copyright © Jamie Leighton, 2016 All Rights Reserved
For more info visit the KSP Forum Thread
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace LoadingTipsPlus
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class LoadingTips : MonoBehaviour
    {
        private ConfigNode globalNode = new ConfigNode();
        private LTconfig LTconfigvals = new LTconfig();
        private List<LoadingScreen.LoadingScreenState> LoadingScreens = new List<LoadingScreen.LoadingScreenState>();
        private List<string> NewToolTipsList = new List<string>();
        private bool processedToolTips = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            try
            {
                string DataPath = Path.Combine(KSPUtil.ApplicationRootPath, "GameData");
                string[] files = Directory.GetFiles(DataPath,"*.ltp",SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    globalNode = ConfigNode.Load(files[i]);
                    LTconfigvals.Load(globalNode);
                }
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Debug.Log(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Debug.Log(PathEx.Message);
            }
            if (LTconfigvals.ToolTips.Count == 0)
            {
                processedToolTips = true;
            }
        }

        private void Update()
        {
            if (!processedToolTips && LoadingScreen.Instance != null)
            {
                LoadingScreens = LoadingScreen.Instance.Screens;
                foreach (var screen in LoadingScreens)
                {
                    NewToolTipsList.Clear();
                    if (!LTconfigvals.Overwrite)
                    {
                        for (int i = 0; i < screen.tips.Length; i++)
                        {
                            NewToolTipsList.Add(screen.tips[i]);
                        }
                    }
                    
                    for (int i = 0; i < LTconfigvals.ToolTips.Count; i++)
                    {
                        NewToolTipsList.Add(LTconfigvals.ToolTips[i]);
                    }

                    
                    string[] NewToolTips = new string[NewToolTipsList.Count];
                    NewToolTips = NewToolTipsList.ToArray();
                    screen.tips = NewToolTips;
                }
                processedToolTips = true;
            }
            if (HighLogic.LoadedScene == GameScenes.MAINMENU)
            {
                DestroyMe();
            }
        }

        private void DestroyMe()
        {
            DestroyObject(this);
        }

        /// <summary>
        /// Name of the Assembly that is running this MonoBehaviour
        /// </summary>
        internal static String AssemblyName
        { get { return Assembly.GetExecutingAssembly().GetName().Name; } }

        /// <summary>
        /// Full Path of the executing Assembly
        /// </summary>
        internal static String AssemblyLocation
        { get { return Assembly.GetExecutingAssembly().Location.Replace("\\", "/"); } }

        /// <summary>
        /// Folder containing the executing Assembly
        /// </summary>
        internal static String AssemblyFolder
        { get { return Path.GetDirectoryName(AssemblyLocation).Replace("\\", "/"); } }
    }
}
