namespace BASScriptingExample3
{
    using System.Collections;
    using System.IO;
    using BS;
    using ModLoader;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    // This script will spawn a creature when a level is loaded, with a simple brain that makes the character stand still, and leave when injured.
    internal class MyBehavior : MonoBehaviour
    {
        private static MyBehavior instance;
        private Coroutine coroutine;

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
        }

        // Called on the frame when a script is enabled just before any of the Update methods are called the first time.
        void Start()
        {
            SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
        }

        // Called every frame, if the MonoBehaviour is enabled.
        void Update()
        {
        }

        // Called when this script is being destroyed.
        void OnDestroy()
        {
            SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;

            // Coroutines are automatically stopped when a script they're running on is destroyed,
            // but just for good measure..
            this.StopAllCoroutines();
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "Master" ||
                newScene.name == GameManager.characterSelectionLevel ||
                newScene.name == GameManager.homeLevel)
            {
                // Don't do anything when "Master" is loaded. This is the loading screen.
                // Don't spawn a creature in the character selection or home levels either.
                return;
            }

            if (this.coroutine != null)
            {
                // Old coroutine still running. Stop it.
                this.StopCoroutine(this.coroutine);
            }

            this.coroutine = this.StartCoroutine(this.HandleCreatureSpawn());
        }

        private IEnumerator HandleCreatureSpawn()
        {
            // Wait a tiny bit.
            yield return new WaitForSeconds(0.1f);

            // Wait until the level is fully loaded.
            yield return new WaitUntil(() => GameManager.IsLevelLoaded(GameManager.GetCurrentLevel()));

            // Wait a little more.
            yield return new WaitForSeconds(2f);

            // Spawn a creature with a custom brain.
            this.SpawnTestCreature();
        }

        private Creature SpawnTestCreature()
        {
            Vector3 spawnPosition;
            Vector3 forward;

            // Calculate a spawn position.
            Player player = Player.local;
            forward = player.body.headBone.transform.forward;
            forward.y = 0;
            forward.Normalize();
            spawnPosition = player.body.transform.position + forward * 3f;
            spawnPosition.y -= 0.3f;

            // Get the "Barbarian" creature from the JSON definition.
            CreatureData creatureData = Catalog.current.GetData<CreatureData>("Barbarian");

            // Clone it so we can alter it a little.
            creatureData = creatureData.Clone();
            creatureData.brainId = null; // Do not install a brain. We'll install our own that isn't in the Catalog.
            creatureData.containerID = "Northern1H";

            // Spawn the creature.
            Creature prefab = Resources.Load("Creatures/UMANPC", typeof(Creature)) as Creature;
            Creature creature = UnityEngine.Object.Instantiate<Creature>(prefab, spawnPosition, Quaternion.LookRotation(-forward));
            creature.data = creatureData;
            creature.name = "TestCreature";
            creature.isSleeping = false;
            creature.spawnTime = Time.time;

            // Install our test brain.
            creature.brain = new TestBrain().Instantiate(creature);

            return creature;
        }
    }
}