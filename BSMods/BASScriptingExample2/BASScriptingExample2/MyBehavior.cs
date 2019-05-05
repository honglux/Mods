namespace BASScriptingExample2
{
    using System.IO;
    using BS;
    using ModLoader;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    // This script plays a gong every time a new map is loaded.
    internal class MyBehavior : MonoBehaviour
    {
        private static MyBehavior instance;
        private AudioClip audioClip;
        private AudioSource audioSource;

        internal MyBehavior()
        {
            MyBehavior.instance = this;
        }

        public static MyBehavior Instance
        {
            get { return MyBehavior.instance; }
            private set { MyBehavior.instance = value; }
        }

        // Called when the script instance is being loaded.
        void Awake()
        {
            this.audioClip = AssetHelper.GetAudioClipFromFile(Path.Combine(PathHelper.CallingModPath, "Gong.ogg"), false);
            this.audioSource = this.gameObject.AddComponent<AudioSource>();
            this.audioSource.clip = this.audioClip;
        }

        // Called on the frame when a script is enabled just before any of the Update methods are called the first time.
        void Start()
        {
            SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
        }

        // Called every frame, if the MonoBehaviour is enabled.
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                // Are we in Market?..
                if (GameManager.GetCurrentLevel() == "Market")
                {
                    // .. then load Ruins.
                    GameManager.LoadLevel("Ruins");
                }
                else
                {
                    // .. otherwise load Market.
                    GameManager.LoadLevel("Market");
                }
            }
        }

        // Called when this script is being destroyed.
        void OnDestroy()
        {
            SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            Logging.Log("Scene switched " + oldScene.name + " to " + newScene.name);
            this.audioSource.Play();
        }
    }
}