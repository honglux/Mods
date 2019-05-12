namespace ArcherHelper
{
    using System;
    using ModLoader;
    using UnityEngine;
    using System.IO;

    internal class ArcherH : MonoBehaviour
    {
        const string folder_path = "Mods/ArcherHelper/";
        const string file_name = "AHSetting.json";
        const string player_OBJ_str = "Player(Clone)";

        private static ArcherH instance;

        private Transform player_TRANS;
        private Camera camera;

        private AHSetting AHS;
        private float Time1;
        private float Time2;
        private float timer1;
        private float timer2;

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
            //generate_setting_temp();
            load_setting();

            this.Time1 = AHS.SearchPlayerTime;
            this.Time2 = AHS.CheckHandTime;
            this.player_TRANS = null;
            this.camera = null;
            this.timer1 = Time1;
            this.timer2 = Time2;
    }

        // Called every frame. If you do not need to execute code each tick, you should remove this method.
        private void Update()
        {

            timer1 -= Time.deltaTime;
            if(timer1 < 0)
            {
                timer1 = Time1;
                update_camera();
                player();
            }

            timer2 -= Time.deltaTime;
            if(timer2 < 0)
            {
                timer2 = Time2;

                arrow_indicator();
            }
        }

        private void player()
        {
            try
            {
                if(player_TRANS == null || player_TRANS.GetComponent<BS.Player>() == null)
                {
                    player_TRANS = GameObject.Find(player_OBJ_str).transform;
                }
            }
            catch (Exception e)
            {
                //Debug.Log("Player error "+e);
            }
        }

        private void arrow_indicator()
        {
            try
            {
                if (player_TRANS != null && player_TRANS.GetComponent<BS.Player>() != null &&
                    camera != null)
                {
                    BS.Player player = player_TRANS.GetComponent<BS.Player>();
                    if (AHS.UseRightHand &&
                        player.body.handRight.interactor.grabbedObject != null)
                    {
                        Transform Grabbed_TRANS = player.body.handRight.interactor.grabbedObject.transform.parent;
                        if (AHS.ObjectToIndiNames.Contains(Grabbed_TRANS.name))
                        {
                            if (Grabbed_TRANS.GetComponent<ArrowIndicator>() == null)
                            {
                                Grabbed_TRANS.gameObject.AddComponent<ArrowIndicator>();
                                Grabbed_TRANS.GetComponent<ArrowIndicator>().
                                    init_AI(player,player.body.handRight.interactor, 
                                            AHS.TimeExistAfterHand,camera,AHS);
                            }
                            else if (Grabbed_TRANS.GetComponent<ArrowIndicator>().End_flag)
                            {
                                Grabbed_TRANS.GetComponent<ArrowIndicator>().reset();
                            }
                        }
                    }

                    if (AHS.UseLeftHand &&
                        player.body.handLeft.interactor.grabbedObject != null)
                    {
                        Transform Grabbed_TRANS = player.body.handLeft.interactor.grabbedObject.transform.parent;
                        if (AHS.ObjectToIndiNames.Contains(Grabbed_TRANS.name))
                        {
                            if (Grabbed_TRANS.GetComponent<ArrowIndicator>() == null)
                            {
                                Grabbed_TRANS.gameObject.AddComponent<ArrowIndicator>();
                                Grabbed_TRANS.GetComponent<ArrowIndicator>().
                                    init_AI(player, player.body.handRight.interactor,
                                            AHS.TimeExistAfterHand, camera,AHS);
                            }
                            else if (Grabbed_TRANS.GetComponent<ArrowIndicator>().End_flag)
                            {
                                Grabbed_TRANS.GetComponent<ArrowIndicator>().reset();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Debug.Log("AH arrow_indicator error " + e);
            }

        }

        private void update_camera()
        {
            camera = Camera.main;
        }

        private void generate_setting_temp()
        {
            //AHSetting;
            AHSetting AHS = new AHSetting();
            string AHS_json = JsonUtility.ToJson(AHS);
            if (!Directory.Exists(folder_path))
            {
                try
                {
                    Directory.CreateDirectory(folder_path);
                }
                catch (Exception e)
                {
                    Debug.Log("Folder creation failed, " + e);
                }
            }
            File.WriteAllText(folder_path + file_name, AHS_json);
        }

        private void load_setting()
        {
            AHS = new AHSetting();
            try
            {
                if (!Directory.Exists(folder_path))
                {
                    throw new Exception("Path do not exist");
                }
                else
                {
                    string json = File.ReadAllText(folder_path + file_name);
                    AHS = JsonUtility.FromJson<AHSetting>(json);
                }
            }
            catch(Exception e)
            {
                Debug.Log("AH Setting load failed " + e);
            }
        }







    }
}