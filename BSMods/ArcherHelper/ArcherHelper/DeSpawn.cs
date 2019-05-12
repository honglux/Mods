namespace ArcherHelper
{
    using UnityEngine;

    internal class DeSpawn : MonoBehaviour
    {
        private float timer;

        private void Awake()
        {
            this.timer = 2.0f;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                timer = float.MaxValue;
                Destroy(gameObject);
            }
        }

        public void init_despawn(float time)
        {
            timer = time;
        }
    }
}
