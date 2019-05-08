namespace ArcherHelper
{
    using BS;
    using ModLoader;
    using ModLoader.Attributes;

    internal static class EntryPoint
    {
        [ModEntryPoint]
        internal static void OnStart()
        {
            GameManager.local.gameObject.AddComponent<ArcherH>();


            Logging.Log("ArcherHelper has loaded.");
        }

        [ModExitPoint]
        internal static void OnExit()
        {
            ArcherH script = ArcherH.Instance;
            if (script)
            {
                UnityEngine.Object.Destroy(script);
            }

            

            Logging.Log("ArcherHelper has unloaded.");
        }


    }
}