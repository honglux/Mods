namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModLoader;
    using UnityEngine;

    internal class MyBehaviour : MonoBehaviour
    {
        private static MyBehaviour instance;

        private BS.Player player;

        private float timer = 2.0f;

        public MyBehaviour()
        {
            MyBehaviour.instance = this;
        }

        public static MyBehaviour Instance
        {
            get { return MyBehaviour.instance; }
            private set { MyBehaviour.instance = value; }
        }

        // Called when this instance is being loaded. Initialization should be done here, not in the constructor.
        private void Awake()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(MyBehaviour)} has woken!");
        }

        // Called every frame. If you do not need to execute code each tick, you should remove this method.
        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 2.0f;
                test1();
                test2();
            }
        }

        private void test1()
        {
            Debug.Log("~~~~~~~~~~~~~~~~~~~~");
            try
            {
                player = FindObjectOfType(typeof(BS.Player)) as BS.Player;
                Debug.Log("player " + player.transform.name + " " + player.transform.tag);
            }
            catch (Exception e) { Debug.Log("Player error " + e); }
        }

        private void test2()
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!");
            try
            {
                if (player != null)
                {
                    if (player.body.handRight.interactor.grabbedObject != null)
                    {
                        Debug.Log("Item " + player.body.handRight.interactor.grabbedObject.transform.parent.name);
                    }
                }
            }
            catch (Exception e) { Debug.Log("Item error " + e); }
        }


        // Called when this instance is being destroyed. Use to clean up.
        private void OnDestroy()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(MyBehaviour)} has been destroyed!");
        }
    }
}