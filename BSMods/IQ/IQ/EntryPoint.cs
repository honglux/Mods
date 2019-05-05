namespace IQ
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using BS;
    using Harmony;
    using ModLoader;
    using ModLoader.Attributes;
    using UnityEngine;

    internal static class EntryPoint
    {
        [ModEntryPoint]
        internal static void OnStart()
        {
            GameManager.local.gameObject.AddComponent<IQ>();

			Logging.Log("IQ has loaded.");
        }

        [ModExitPoint]
        internal static void OnExit()
        {
            IQ script = IQ.Instance;
            if (script)
            {
                UnityEngine.Object.Destroy(script);
            }

			Logging.Log("IQ has unloaded.");
        }
    }
}