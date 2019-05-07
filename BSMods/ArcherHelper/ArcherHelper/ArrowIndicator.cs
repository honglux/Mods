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
        private BS.Interactor interactor;
        private bool UsingDynamicSize;
        //private float fieldofview;
        private Vector3 orig_size;
        private Camera camera;

        private void Awake()
        {
            this.Waiting_flag = false;
            this.End_flag = false;
            this.wait_timer = WaitTime;
            this.player = null;
            this.interactor = null;
            this.player_TRANS = null;
            //this.fieldofview = 0.0f;
            this.orig_size = new Vector3();
            //this.hand = null;

            orig_size = new Vector3(0.02f, 0.02f, 0.02f);
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
                            //Debug.Log("$$$$$$$$$$$$$ ");
                            //Debug.Log(player.body.handRight.transform.name);
                            //Debug.Log(player.body.handRight.interactor.transform.name);
                            //Debug.Log(player.body.handRight.interactor.grabbedObject.transform.name);
                            //Debug.Log(player.body.handRight.interactor.grabbedObject.transform.parent.gameObject.name);
                            //Debug.Log(gameObject.name);
                            start_wait();
                        }
                        else
                        {
                            //Debug.Log("$$$$$$$$$$$$$ ");
                            //Debug.Log(player.body.handRight.interactor.grabbedObject.transform.name);
                            //Debug.Log(player.body.handRight.interactor.grabbedObject.transform.parent.gameObject.name);
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
                            Camera _camera)
        {
            player = _player;
            interactor = _interactor;
            player_TRANS = player.transform;
            WaitTime = _wait_time;
            wait_timer = WaitTime;
            camera = _camera;
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
                    sphere.GetComponent<SphereCollider>().enabled = false;
                    Destroy(sphere.GetComponent<SphereCollider>());
                    sphere.GetComponent<MeshRenderer>().material.color = Color.red;
                    sphere.transform.position = CP.point;
                    sphere.transform.localScale = size_cal(CP.point);
                }
                catch { }
            }
        }
        
        private Vector3 size_cal(Vector3 hit_point)
        {
            Debug.Log("$$$$$$$$$$$$$ ");
            float dist = Vector3.Distance(camera.transform.position, hit_point);
            //test_cube();
            //Debug.Log("dist " + dist);
            float rad_scale = Mathf.Tan(camera.fieldOfView / 2) * dist;
            //Debug.Log("rad_scale " + rad_scale);
            //Debug.Log("return " + orig_size * rad_scale);
            return orig_size * rad_scale;
        }

        private void test_cube()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = camera.transform.position;
            cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
