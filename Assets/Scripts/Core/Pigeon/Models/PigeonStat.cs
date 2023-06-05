using System;

namespace Core.Effects.Models
{
    // 鴿子的狀態
    [Serializable]
    public class PigeonStat
    {
        public int    age;         //年紀
        public int    age_feature; //年齡特性
        public string breed_id;    //品種ID
        public string breed_name;  //品種名稱

        public int constitution; //體質
        public int exp;          //經驗值

        public string father_pigeon_id; //鴿父ID
        public int    fatigue;          //疲勞度
        public int    feather_quality;  //羽質
        public int    feather_size;     //羽型
        public int    feature_a;        //特性A
        public int    feature_b;        //特性B
        public int    feature_c;        //特性C
        public int    free_points;      //自由點數

        public int    gender;                 //性別(0公 1母)
        public string grand_father_pigeon_id; //鴿爺爺ID
        public string grand_mother_pigeon_id; //鴿奶奶ID
        public int    iq;                     //智商
        public int    level;                  //等級

        public int    max_constitution;    //體質上限
        public int    max_feather_quality; //羽質上限
        public int    max_feather_size;    //羽型上限
        public int    max_iq;              //智商上限
        public int    max_muscle;          //肌力上限
        public int    max_speed;           //體型上限
        public int    max_vision;          //眼睛上限
        public int    max_vitality;        //心肺上限
        public string mother_pigeon_id;    //鴿母ID 
        public int    muscle;              //肌力

        public string pigeon_id;   //鴿子ID
        public string pigeon_name; //暱稱
        public string pigeon_pigeonry;
        public int    speed;    //體型
        public int    vision;   //眼睛
        public int    vitality; //心肺
    }
}