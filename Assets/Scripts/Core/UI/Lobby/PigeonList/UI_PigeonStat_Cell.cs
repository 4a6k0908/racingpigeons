using System;
using System.Collections.Generic;
using Core.Effects.Models;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Core.UI.Lobby.PigeonList
{
    public class UI_PigeonStat_Cell : FancyScrollRectCell<PigeonStat, Context>
    {
        [SerializeField] private Image           avatarImg; // 鴿子的 Avatar
        [SerializeField] private TextMeshProUGUI nameText;  // 鴿子的名字

        [SerializeField] private Button     selectBtn;
        [SerializeField] private GameObject selectedObj;

        [FoldoutGroup("鴿子狀態")] [SerializeField] private Image    favoriteImg; // 最愛
        [FoldoutGroup("鴿子狀態")] [SerializeField] private Sprite[] favoriteSprites;
        [FoldoutGroup("鴿子狀態")] [SerializeField] private Image    genderImg; // 性別
        [FoldoutGroup("鴿子狀態")] [SerializeField] private Sprite[] genderSprites;
        [FoldoutGroup("鴿子狀態")] [SerializeField] private Image    statusImg; // 狀態
        [FoldoutGroup("鴿子狀態")] [SerializeField] private Sprite[] statusSprites;

        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData iqAbility;             // 智力
        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData visionAbility;         // 眼睛
        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData speedAbility;          // 體型
        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData featherSizeAbility;    // 羽型
        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData vitalityAbility;       // 心肺
        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData muscleAbility;         // 肌力
        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData constitutionAbility;   // 腸胃
        [FoldoutGroup("鴿子屬性")] [SerializeField] private AbilityData featherQualityAbility; // 羽質

        [SerializeField] private Sprite[] gemSprites; // 寶石的 Sprites

        [SerializeField] private Image fatigueBarImg; // 消耗條
        [SerializeField] private Image tiredBarImg;   // 疲勞條
        [SerializeField] private Image expBarImg;     // 經驗質

        private UI_PigeonList uiPigeonList;

        private void Awake()
        {
            // TODO: 待優化
            uiPigeonList = FindObjectOfType<UI_PigeonList>();
        }

        public override void Initialize()
        {
            Context.OnCellClicked += OnCellClicked;
            
            selectBtn.onClick.AddListener(() =>
            {
                Context.OnCellClicked?.Invoke(Index);
            });
        }

        // 更換鴿子資訊
        public override void UpdateContent(PigeonStat pigeonStat)
        {
            nameText.text = $"鴿名：{pigeonStat.pigeon_name}";

            genderImg.sprite = genderSprites[pigeonStat.gender];

            iqAbility.SetCurrentLevel(pigeonStat.iq, pigeonStat.max_iq, gemSprites);
            visionAbility.SetCurrentLevel(pigeonStat.vision, pigeonStat.max_vision, gemSprites);
            speedAbility.SetCurrentLevel(pigeonStat.speed, pigeonStat.max_speed, gemSprites);
            featherSizeAbility.SetCurrentLevel(pigeonStat.feather_size, pigeonStat.max_feather_size, gemSprites);
            vitalityAbility.SetCurrentLevel(pigeonStat.vitality, pigeonStat.max_vitality, gemSprites);
            muscleAbility.SetCurrentLevel(pigeonStat.muscle, pigeonStat.max_muscle, gemSprites);
            constitutionAbility.SetCurrentLevel(pigeonStat.constitution, pigeonStat.max_constitution, gemSprites);
            featherQualityAbility.SetCurrentLevel(pigeonStat.feather_quality, pigeonStat.max_feather_quality, gemSprites);

            // TODO: 確認上限多少，現在假定 100
            fatigueBarImg.fillAmount = pigeonStat.fatigue / 100.0f;
            expBarImg.fillAmount     = pigeonStat.exp     / 100.0f;

            selectedObj.SetActive(Context.selectedIndex == Index);
        }

        private void OnCellClicked(int clickIndex)
        {
            selectedObj.SetActive(Index == clickIndex);
        }
        
        public void Button_View()
        {
            uiPigeonList.ChangeMode(PigeonListViewMode.HalfPigeon);
        }

        [Serializable]
        private class AbilityData
        {
            [SerializeField] private Image           gemImg;
            [SerializeField] private TextMeshProUGUI levelText;

            // 設定寶石圖片以及等級文字資訊
            public void SetCurrentLevel(int stat, int statMax, IReadOnlyList<Sprite> gemSprites)
            {
                gemImg.sprite = statMax switch {
                    0                => gemSprites[0],
                    < 200            => gemSprites[1],
                    >= 200 and < 300 => gemSprites[2],
                    >= 300 and < 400 => gemSprites[3],
                    >= 400 and < 500 => gemSprites[4],
                    _                => gemSprites[5]
                };

                levelText.text = stat switch {
                    0      => "?",
                    <= 60  => "C",
                    <= 148 => "B",
                    <= 232 => "A",
                    <= 316 => "S",
                    _      => "SS",
                };
            }
        }
    }
}