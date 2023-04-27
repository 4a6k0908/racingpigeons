using System.Net.Mime;
using Core.Pigeon.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Core.UI.Lobby.PigeonList
{
    public class UI_PigeonStat_Cell : FancyScrollRectCell<PigeonStat, Context>
    {
        [SerializeField] private Image           avatarImg; // 鴿子的 Avatar
        
        [SerializeField] private TextMeshProUGUI nameText; // 鴿子的名字
        
        [SerializeField] private Image    favoriteImg; // 最愛
        [SerializeField] private Sprite[] favoriteSprites;
        [SerializeField] private Image    genderImg; // 性別
        [SerializeField] private Sprite[] genderSprites;
        [SerializeField] private Image    statusImg; // 狀態
        [SerializeField] private Sprite[] statusSprites;

        [SerializeField] private Image           iqGemImg; // 智力
        [SerializeField] private TextMeshProUGUI iqLevelText;
        [SerializeField] private Image           visionGemImg; // 眼睛
        [SerializeField] private TextMeshProUGUI visionLevelText;
        [SerializeField] private Image           speedGemImg; // 體型
        [SerializeField] private TextMeshProUGUI speedLevelText;
        [SerializeField] private Image           featherSizeGemImg; // 羽型
        [SerializeField] private TextMeshProUGUI featherSizeLevelText;
        [SerializeField] private Image           vitalityGemImg; // 心肺
        [SerializeField] private TextMeshProUGUI vitalityLevelText;
        [SerializeField] private Image           muscleGemImg; // 肌力
        [SerializeField] private TextMeshProUGUI muscleLevelText;
        [SerializeField] private Image           constitutionGemImg; // 腸胃
        [SerializeField] private TextMeshProUGUI constitutionLevelText;
        [SerializeField] private Image           featherQualityGemImg; // 羽質
        [SerializeField] private TextMeshProUGUI featherQualityLevelText;
        [SerializeField] private Sprite[]        gemSprites; // 寶石的 Sprites

        [SerializeField] private Image consumeBarImg; // 消耗條
        [SerializeField] private Image tiredBarImg; // 疲勞條
        [SerializeField] private Image expBarImg; // 經驗質

        public override void UpdateContent(PigeonStat itemData)
        {
            
        }

        public void Button_View()
        {
            
        }
    }
}