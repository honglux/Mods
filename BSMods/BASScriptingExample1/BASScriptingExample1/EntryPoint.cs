namespace BASScriptingExample1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModLoader;
    using ModLoader.Attributes;
    using UnityEngine;

    internal static class EntryPoint
    {
        [ModEntryPoint]
        private static void Main()
        {
            // Called when the mod is loaded.

            // Load the Gong.ogg sound.
            string soundPath = Path.Combine(PathHelper.CallingModPath, "Gong.ogg");
            if (!File.Exists(soundPath))
            {
                Logging.Log("Couldn't find Gong.ogg :'(");
                return;
            }

            AudioClip clip = AssetHelper.GetAudioClipFromFile(soundPath, false);

            // Create a new GameObject with a source we can play the sound on.
            GameObject go = new GameObject();
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();

            // Destroy the GameObject after 10 seconds.
            UnityEngine.Object.Destroy(go, 10f);
        }

        [ModExitPoint]
        private static void OnExit()
        {
            // Called when this mod is "unloaded".
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