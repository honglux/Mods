namespace IQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModLoader;
    using UnityEngine;
    using System.IO;

    internal class IQ : MonoBehaviour
    {
        private static IQ instance;

        private Vector3 position = new Vector3();
        private float timer = 0.0f;

        GameObject WSL_OBJ = null;

        public IQ()
        {
            IQ.instance = this;
        }

        public static IQ Instance
        {
            get { return IQ.instance; }
            private set { IQ.instance = value; }
        }

        // Called when this instance is being loaded. Initialization should be done here, not in the constructor.
        private void Awake()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(IQ)} has woken!");
        }

        // Called every frame. If you do not need to execute code each tick, you should remove this method.
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > 2.0f)
            {
                quiver();
                //player();
                //sword();
                //debug2();
                timer = 0.0f;
            }



            Logging.Log($"[{Time.fixedTime}] {nameof(IQ)} ticked!");
        }

        // Called when this instance is being destroyed. Use to clean up.
        private void OnDestroy()
        {
            Logging.Log($"[{Time.fixedTime}] {nameof(IQ)} has been destroyed!");
        }

        private void debug()
        {
            int counter = 0;

            foreach (BS.Quiver finded in FindObjectsOfType<BS.Quiver>())
            {
                //Debug.Log("Quiver " + finded.GetType().FullName + " counter " + counter);
                //Debug.Log("Arrows " + finded.GetArrowCount());
                counter++;
                System.Reflection.PropertyInfo[] p = finded.GetType().GetProperties();
                foreach (var _p in p)
                {
                    Debug.Log("QP " + _p.Name + " " + _p.GetValue(finded).ToString() + " " + _p.PropertyType);
                    //Debug.Log("QP " + _p.Name + " " + _p.GetValue(finded).ToString());
                    if (_p.Name == "quiverHolder")
                    {
                        Debug.Log("-------");
                        System.Reflection.PropertyInfo[] pp = _p.PropertyType.GetProperties();
                        foreach (var _pp in pp)
                        {
                            Debug.Log("QPP~ " + _pp.Name + " " + _pp.PropertyType);
                        }
                    }
                }

                //log_object("", finded);
            }

        }

        private void debug2()
        {
            Debug.Log("=============================");

            //Debug.Log("Player pos " + position);
            //GameObject[] finds = GameObject.FindGameObjectsWithTag("Untagged");

            //foreach (GameObject finded in finds)
            //{
            //    if (finded.name == "ABC")
            //    {
            //        try
            //        {
            //            Debug.Log("finded " + finded.transform.position);
            //            Debug.Log("finded activeSelf " + finded.activeSelf);
            //            Debug.Log("finded MeshRenderer " + finded.GetComponent<MeshRenderer>().enabled);
            //        }
            //        catch (Exception e) { Debug.Log(e); }
            //    }

            //}


            UnityEngine.Object[] GO = FindObjectsOfType(typeof(GameObject));
            HashSet<string> names = new HashSet<string>();
            foreach (UnityEngine.Object OBJ in GO)
            {
                names.Add(OBJ.name);
            }

            foreach(string name in names)
            {
                Debug.Log("OBJ " + name);
            }



        }

        private void quiver()
        {
            foreach (BS.Quiver finded in FindObjectsOfType<BS.Quiver>())
            {
                while (finded.quiverHolder.holdObjects.Count < 10)
                {
                    BS.InteractiveObject A_OBJ = finded.quiverHolder.holdObjects[0].item.Instantiate();
                    finded.quiverHolder.Snap(A_OBJ);
                }
            }

        }

        private void sword()
        {
            Debug.Log("@@@@@@@@@@@@@@@@@@");
            if(WSL_OBJ == null)
            {
                Debug.Log("FIND");
                WSL_OBJ = GameObject.Find("LongswordT2");
            }
            else
            {
                Debug.Log("Instantiate");
                try
                {
                    BS.InteractiveObject IO = WSL_OBJ.GetComponent<BS.InteractiveObject>().item.Instantiate();
                    IO.transform.position = position + new Vector3(0.0f, 0.3f, 0.3f);
                    if (IO != null)
                    {
                        Debug.Log("IO pos " + IO.transform.position);
                        Debug.Log("Origin pos " + WSL_OBJ.transform.position);
                    }

                }
                catch (Exception e) { Debug.Log(e); }

                //Debug.Log("Move");
                //WSL_OBJ.transform.position = position + new Vector3(0.0f, 0.3f, 0.3f);
                Debug.Log("Origin pos " + WSL_OBJ.transform.position);
                Debug.Log("Player pos " + position);
                GameObject SP = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                SP.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                SP.transform.position = WSL_OBJ.transform.position;
            }
            
        }


        private void player()
        {
            Debug.Log("~~~~~~~~~~~~~~~~~");
            foreach (BS.Player finded in FindObjectsOfType<BS.Player>())
            {
                position = finded.transform.position;

            }
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = position;
        }

        //private void log_object(string path,object obj)
        //{
        //    StreamWriter file;
        //    try
        //    {
        //        // create log file if it does not already exist. Otherwise open it for appending new trial
        //        if (!File.Exists(path))
        //        {
        //            file = new StreamWriter(path);

        //            ObjectDumper.Dumper.Dump(item, "Object Dumper", writer);

        //            file.Close();

        //        }


        //    }
        //    catch (System.Exception e)
        //    {
        //        Debug.Log("Error in accessing file: " + e);
        //    }
        //}
    }
}