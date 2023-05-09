using System;
using UnityEngine.Serialization;

namespace Core.Pigeon.Models
{
    // 大廳、比賽場景中的鴿子的資料
    [Serializable]
    public class PigeonData
    {
        public float moveSpeed;

        public BackgroundShowPercent backgroundShowPercent;
        public HoverShowPercent      hoverShowPercentPercent;
        public RoofShowPercent       roofShowPercent;

        // 獨立事件
        [Serializable]
        public class BackgroundShowPercent
        {
            public int _01_background_left_to_right = 30;
            public int _02_none                     = 40;
            public int _03_background_right_to_left = 30;
        }

        // 屋頂
        [Serializable]
        public class RoofShowPercent
        {
            public int _01_roof_walk;
        }

        // 盤旋
        [Serializable]
        public class HoverShowPercent
        {
            
        }
        
        // 地面
        [Serializable]
        public class CageOutGroundShowPercent
        {
            
        }
        
        // 
        [Serializable]
        public class CageInPaddleShowPercent
        {
            
        }
        
        
    }
}