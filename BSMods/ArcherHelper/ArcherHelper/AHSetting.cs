
namespace ArcherHelper
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    class AHSetting
    {
        public List<string> ObjectToIndiNames;
        public float SearchPlayerTime;
        public float CheckHandTime;
        public bool UseRightHand;
        public bool UseLeftHand;
        public float TimeExistAfterHand;
        public Vector3 OriginalSize;
        public bool UseDynamicSize;
        public Color IndicatorColor;
        public float IndicatorDespawnTime;

        public AHSetting()
        {
            this.ObjectToIndiNames = new List<string>{ "Pool_Arrow1"};
            this.SearchPlayerTime = 2.0f;
            this.CheckHandTime = 0.1f;
            this.UseRightHand = true;
            this.UseLeftHand = false;
            this.TimeExistAfterHand = 10.0f;
            this.OriginalSize = new Vector3(0.02f, 0.02f, 0.02f);
            this.UseDynamicSize = true;
            this.IndicatorColor = Color.red;
            this.IndicatorDespawnTime = 2.0f;
        }
    }
}
