namespace ArcherHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModLoader;
    using UnityEngine;

    internal class ARcherH : MonoBehaviour
    {
        private static ARcherH instance;

        private Transform player_TRANS;

        private float timer = 2.0f;

        public ARcherH()
        {
            ARcherH.instance = this;
        }

        public static ARcherH Instance
        {
            get { return ARcherH.instance; }
            private set { ARcherH.instance = value; }
        }

        // Called when this instance is being loaded. Initialization should be done here, not in the constructor.
        private void Awake()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(ARcherH)} has woken!");
            player_TRANS = null;
        }

        // Called every frame. If you do not need to execute code each tick, you should remove this method.
        private void Update()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(ARcherH)} ticked!");

            timer -= Time.deltaTime;
            if(timer < 0)
            {
                timer = 2.0f;

                player();

            }
        }

        // Called when this instance is being destroyed. Use to clean up.
        private void OnDestroy()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(ARcherH)} has been destroyed!");
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
                if(player_TRANS == null)
                {
                    BS.Player player = FindObjectOfType(typeof(BS.Player)) as BS.Player;
                    Rigidbody G_OBJ = player.body.handRight.playerHand.physicJoint.connectedBody;
                    //Rigidbody G_OBJ2 = player.body.handRight.distantGrabber.joint.connectedBody;
                    Debug.Log("Grib " + G_OBJ.name);

                    //Debug.Log("Player " + player.gameObject.name + " " + player.gameObject.tag);
                    //player_TRANS = player.transform;
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
    }
}