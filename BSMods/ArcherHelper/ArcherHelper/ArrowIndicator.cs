namespace ArcherHelper
{
    using UnityEngine;
    using System;

    internal class ArrowIndicator :  MonoBehaviour
    {

        public bool Waiting_flag { get; private set; }
        public bool End_flag { get; private set; }

        private AHSetting AHS;

        private float WaitTime;
        private float wait_timer;
        private Transform player_TRANS;
        private BS.Player player;
        private BS.Interactor interactor;
        private bool UsingDynamicSize;
        private Vector3 orig_size;
        private Camera camera;

        private void Awake()
        {
            this.AHS = null;
            this.Waiting_flag = false;
            this.End_flag = false;
            this.wait_timer = WaitTime;
            this.player = null;
            this.interactor = null;
            this.player_TRANS = null;
            this.orig_size = new Vector3();
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
                        Destroy(this);
                    }
                }
                else
                {
                    try
                    {
                        if (interactor.grabbedObject == null || 
                            !GameObject.ReferenceEquals(interactor.grabbedObject.transform.parent.gameObject, gameObject))
                        {
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
                if(!Waiting_flag && interactor.grabbedObject != null &&
                    GameObject.ReferenceEquals(interactor.grabbedObject.transform.parent.gameObject, gameObject))
                {
                    reset();
                }
            }
        }

        public void init_AI(Transform _player_TRANS,float _wait_time)
        {
            player_TRANS = _player_TRANS;
            player = player_TRANS.GetComponent<BS.Player>();
            interactor = player.body.handRight.interactor;
            WaitTime = _wait_time;
            wait_timer = WaitTime;
        }

        public void init_AI(BS.Player _player,BS.Interactor _interactor,float _wait_time)
        {
            player = _player;
            interactor = _interactor;
            player_TRANS = player.transform;
            WaitTime = _wait_time;
            wait_timer = WaitTime;
        }

        public void init_AI(BS.Player _player, BS.Interactor _interactor, float _wait_time,
                            Camera _camera,AHSetting _AHS)
        {
            player = _player;
            interactor = _interactor;
            player_TRANS = player.transform;
            WaitTime = _wait_time;
            wait_timer = WaitTime;
            camera = _camera;
            AHS = _AHS;
            orig_size = AHS.OriginalSize;
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
                Destroy(this);
            }
        }

        private void spawn_indicator(Collision collision)
        {
            ContactPoint[] CPs = new ContactPoint[collision.contactCount];
            collision.GetContacts(CPs);
            foreach (ContactPoint CP in CPs)
            {
                try
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.AddComponent<DeSpawn>();
                    sphere.GetComponent<DeSpawn>().init_despawn(AHS.IndicatorDespawnTime);
                    sphere.GetComponent<SphereCollider>().enabled = false;
                    Destroy(sphere.GetComponent<SphereCollider>());
                    sphere.GetComponent<MeshRenderer>().material.color = AHS.IndicatorColor;
                    sphere.transform.position = CP.point;
                    if(AHS.UseDynamicSize)
                    {
                        sphere.transform.localScale = size_cal(CP.point);
                    }
                    else
                    {
                        sphere.transform.localScale = orig_size;
                    }
                    
                }
                catch (Exception e)
                {
                    Debug.Log("AH spawn_indicator error " + e);
                }
            }
        }
        
        private Vector3 size_cal(Vector3 hit_point)
        {
            float dist = Vector3.Distance(camera.transform.position, hit_point);
            float rad_scale = Mathf.Tan(camera.fieldOfView / 2) * dist;
            return orig_size * rad_scale;
        }
    }
}
