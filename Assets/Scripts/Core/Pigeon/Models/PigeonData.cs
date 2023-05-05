using System;
using UnityEngine.Serialization;

namespace Core.Pigeon.Models
{
    // 大廳、比賽場景中的鴿子的資料
    [Serializable]
    public class PigeonData
    {
        public float moveSpeed;

        public IndividualShowPercent individualShowPercent;
        public CageInShowPercent     cageInShowPercentPercent;
        public CageOutRoofPercent    cageOutRoofPercent;

        // 獨立事件
        [Serializable]
        public class IndividualShowPercent
        {
            public int _01_background_left_to_right = 30;
            public int _02_none                     = 40;
            public int _03_background_right_to_left = 30;
        }

        // 鴿舍外
        [Serializable]
        public class CageOutRoofPercent
        {
            public int _01_roof_to_ground;
        }

        // 鴿舍內
        [Serializable]
        public class CageInShowPercent { }
    }
}