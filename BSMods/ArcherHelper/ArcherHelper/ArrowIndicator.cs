namespace ArcherHelper
{
    using UnityEngine;
    using System;

    internal class ArrowIndicator :  MonoBehaviour
    {

        public bool Waiting_flag { get; private set; }
        public bool End_flag { get; private set; }
        private float WaitTime;
        private float wait_timer;
        private Transform player_TRANS;
        private BS.Player player;
        //private BS.BodyHand hand;
        //private Transform find_cache;

        private void Awake()
        {
            this.Waiting_flag = false;
            this.End_flag = false;
            this.wait_timer = WaitTime;
            this.player = null;
            this.player_TRANS = null;
            //this.hand = null;
        }

        private void Update()
        {
            if(!End_flag)
            {
                if (Waiting_flag)
                {
                    wait_timer -= Time.deltaTime;
                    if (wait_timer < 0)
                    {
                        Waiting_flag = false;
                        End_flag = true;
                        Debug.Log("ArrowIndicator waited !!!!! " + transform.name);
                    }
                }
                else
                {
                    try
                    {
                        if (player.body.handRight.interactor.grabbedObject == null || 
                            !GameObject.ReferenceEquals(player.body.handRight.interactor.grabbedObject.transform.parent.gameObject, gameObject))
                        {
                            Debug.Log("$$$$$$$$$$$$$ ");
                            Debug.Log(player.body.handRight.transform.name);
                            Debug.Log(player.body.handRight.interactor.transform.name);
                            Debug.Log(player.body.handRight.interactor.grabbedObject.transform.name);
                            Debug.Log(player.body.handRight.interactor.grabbedObject.transform.parent.gameObject.name);
                            Debug.Log(gameObject.name);
                            start_wait();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("start_wait error " + e);
                    }
                }
            }
            else
            {
                if(!Waiting_flag && player.body.handRight.interactor.grabbedObject != null &&
                    GameObject.ReferenceEquals(player.body.handRight.interactor.grabbedObject.transform.parent.gameObject, gameObject))
                {
                    reset();
                }
            }
        }

        public void init_AI(Transform _player_TRANS,float _wait_time)
        {
            player_TRANS = _player_TRANS;
            player = player_TRANS.GetComponent<BS.Player>();
            WaitTime = _wait_time;
            wait_timer = WaitTime;
        }

        public void start_wait()
        {
            Waiting_flag = true;
        }

        public void reset()
        {
            Waiting_flag = false;
            End_flag = false;
            wait_timer = WaitTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(Waiting_flag && !End_flag)
            {
                Waiting_flag = false;
                End_flag = true;
                spawn_indicator(collision);
            }
        }

        private void spawn_indicator(Collision collision)
        {
            ContactPoint[] CPs = new ContactPoint[collision.contactCount];
            collision.GetContacts(CPs);
            foreach (ContactPoint CP in CPs)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.GetComponent<SphereCollider>().enabled = false;
                Destroy(sphere.GetComponent<SphereCollider>());
                sphere.GetComponent<MeshRenderer>().material.color = Color.red;
                sphere.transform.position = CP.point;
                sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                sphere.AddComponent<DeSpawn>();
            }
        }
    }
}
