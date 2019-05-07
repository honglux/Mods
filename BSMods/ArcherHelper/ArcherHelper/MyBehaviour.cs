namespace ArcherHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModLoader;
    using UnityEngine;

    internal class ArcherH : MonoBehaviour
    {
        private static ArcherH instance;

        private Transform player_TRANS;

        private float timer;
        private float timer2;
        private float timer3 = 0.0f;

        private int debug_counter = 0;

        public ArcherH()
        {
            ArcherH.instance = this;
        }

        public static ArcherH Instance
        {
            get { return ArcherH.instance; }
            private set { ArcherH.instance = value; }
        }

        // Called when this instance is being loaded. Initialization should be done here, not in the constructor.
        private void Awake()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(ArcherH)} has woken!");
            player_TRANS = null;
            debug_counter = 0;
            timer = 2.0f;
            timer2 = 0.1f;
    }

        // Called every frame. If you do not need to execute code each tick, you should remove this method.
        private void Update()
        {

            timer -= Time.deltaTime;
            if(timer < 0)
            {
                timer = 2.0f;

                player();

            }

            timer2 -= Time.deltaTime;
            if(timer2 < 0)
            {
                timer2 = 0.1f;

                arrow_indicator();
            }

            //debug();
        }

        private void debug()
        {
            timer3 += Time.deltaTime;
            Debug.Log("timer3 " + timer3.ToString("F2"));
        }

        // Called when this instance is being destroyed. Use to clean up.
        private void OnDestroy()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(ArcherH)} has been destroyed!");
        }

        private void player()
        {
            Debug.Log("~~~~~~~~~~~~~~~~~");

            //if(player_TRANS == null)
            //{
            //    player_TRANS = 
            //}

            try
            {
                if(player_TRANS == null || player_TRANS.GetComponent<BS.Player>() == null)
                {
                    BS.Player player = FindObjectOfType(typeof(BS.Player)) as BS.Player;
                    player_TRANS = player.transform;
                }
            }
            catch (Exception e) { Debug.Log("Player error "+e); }
            

            //Debug.Log("Player num " + );

            //foreach (BS.Player finded in FindObjectsOfType<BS.Player>())
            //{
            //position = finded.transform.position;

            //}
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = position;
        }
        private void arrow_indicator()
        {
            try
            {
                if (player_TRANS != null && player_TRANS.GetComponent<BS.Player>() != null)
                {
                    BS.Player player = player_TRANS.GetComponent<BS.Player>();
                    if (player.body.handRight.interactor.grabbedObject != null)
                    {
                        Transform Grabbed_TRANS = player.body.handRight.interactor.grabbedObject.transform.parent;
                        if (Grabbed_TRANS.name == "Pool_Arrow1")
                        {
                            if (Grabbed_TRANS.GetComponent<ArrowIndicator>() == null)
                            {
                                Grabbed_TRANS.gameObject.AddComponent<ArrowIndicator>();
                                Grabbed_TRANS.GetComponent<ArrowIndicator>().
                                    init_AI(player_TRANS, 5.0f);
                            }
                            else if (Grabbed_TRANS.GetComponent<ArrowIndicator>().End_flag)
                            {
                                Grabbed_TRANS.GetComponent<ArrowIndicator>().reset();
                            }
                        }
                    }
                }
            }
            catch { }

        }
    }


}