namespace BASScriptingExample4
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BS;
    using DigitalRuby.ThunderAndLightning;
    using Harmony;
    using ModLoader;
    using ModLoader.Attributes;
    using UnityEngine;

    internal static class EntryPoint
    {
        private static Harmony.HarmonyInstance harmony;
        private static Texture2D lightningTexture;

        [ModEntryPoint]
        private static void Main()
        {
            // Called when the mod is loaded.

            // Load the texture we need.
            EntryPoint.lightningTexture = AssetHelper.GetTextureFromFile(Path.Combine(PathHelper.CallingModPath, "LightningTexture.png"));

            // Initialize Harmony so we can patch some game methods.
            // The parameter to Create() is an ID that must be unique for each mod.
            EntryPoint.harmony = Harmony.HarmonyInstance.Create("com.mulledk19.example4");
            EntryPoint.harmony.PatchAll();
        }

        // Postfix patch for SpellLightning.Load.
        [HarmonyPatch(typeof(SpellLightning))]
        [HarmonyPatch(nameof(SpellLightning.Load))]
        internal static class LightningLoadPatch
        {
            [HarmonyPostfix]
            internal static void Postfix(SpellLightning __instance)
            {
                // Add many more bolts.
                __instance.countMinRange *= 10;
                __instance.countMaxRange *= 10;

                // Change texture and size.
                __instance.lightningBoltScript.LightningTexture = EntryPoint.lightningTexture;
                __instance.lightningBoltScript.JitterMultiplier *= 0.2f;

                // Make it fly all over the place.
                __instance.ChaosFactor *= 30f;
                __instance.ChaosFactorForks *= 30f;
                __instance.lightningBoltScript.Turbulence *= 2f;
                __instance.lightningBoltScript.TurbulenceVelocity *= 1.5f;

                // More branching in the lightning.
                __instance.Forkedness *= 6f;
            }
        }

        // Postfix patch for SpellLightning.GenerateParameters.
        [HarmonyPatch(typeof(SpellLightning))]
        [HarmonyPatch("GenerateParameters")]
        internal static class LightningColorRandomPatch
        {
            [HarmonyPostfix]
            internal static void Postfix(SpellLightning __instance, LightningBoltParameters __result)
            {
                Color color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
                __instance.lightningBoltScript.GlowTintColor = color;
                __instance.lightningBoltScript.LightningTintColor = color;
                __result.LightParameters.LightColor = color;
            }
        }

        [ModExitPoint]
        private static void OnExit()
        {
            // Called when this mod is "unloaded".

            // Remove our patches.
            EntryPoint.harmony?.UnpatchAll();

            // Unload our texture.
            if (EntryPoint.lightningTexture != null)
            {
                UnityEngine.Object.Destroy(EntryPoint.lightningTexture);
                EntryPoint.lightningTexture = null;
            }

            // Note, assemblies can't actually be unloaded from a process
            // without unloading the entire AppDomain, thus unloading everything else too.
            // And loading mods into separate AppDomains would not work well with Unity.
            // 
            // This method is meant only to make debugging easier.
            // When the loader is in developer mode (See LoaderSettings.json), the loader
            // will clone mod assemblies before loading them, meaning they can be loaded into memory multiple times.
            //
            // You should cleanup certain things your mod does, in this method, if you intend
            // to use the "reload" feature of the loader.
            //
            // For example, if you make a brain and install it into a creature,
            // you don't have to uninstall that brain. The creature can keep using the brain.
            //
            // But if you hook an event for example, or spawn a MonoBehavior; you'll probably want
            // to unhook that event, and destroy the behaviors, so the old instance of the mod
            // doesn't react to for example key presses while the new instance also does.
            //
            // A note on cloned assemblies:
            // Since an assembly can only be loaded into an AppDomain once, the loader
            // clones the mod assembly, giving it a random name every time. This forces the runtime
            // to load the "same" assembly again.
            // However, since it's a clone with a different name, it's not actually the same assembly.
            // So for example, if you launch the mod,
            // and install a brain of type MyNamespace.MyBrain into a creature,
            // then reload your mod, you can't assign that brain to a variable of MyNamespace.MyBrain,
            // because technically, the new instance of your mod is a different assembly, and thus, despite
            // the same name, it's not the same type.
        }
    }
}