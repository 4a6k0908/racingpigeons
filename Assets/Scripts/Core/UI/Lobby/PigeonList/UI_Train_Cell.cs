﻿using System;
using System.Collections.Generic;
using Core.Effects.Models;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using static Core.Effects.Models.EffectModel.GQL_GetEffectList.Data;

namespace Core.UI.Lobby.Train
{
    public class UI_Train_Cell : FancyScrollRectCell<Effect, Context>
    {
        //[SerializeField] private Image           avatarImg; // 鴿子的 Avatar
        [SerializeField] private TextMeshProUGUI nameText;  // 鴿子的名字

        [SerializeField] private Button btnMain;
        [SerializeField] private GameObject selectedObj;

        Animator anim;
        //private UI_Train uiTrain;

        private void Awake()
        {
            // TODO: 待優化
            //uiTrain = FindObjectOfType<UI_Train>();
            anim = this.gameObject.GetComponentInChildren<Animator>();
        }

        public override void Initialize()
        {
            Context.OnCellClicked += OnCellClicked;
        }

        // 更換鴿子資訊
        public override void UpdateContent(Effect eff)
        {
            nameText.text = eff.effect_name;
            btnMain.onClick.RemoveAllListeners();
            btnMain.onClick.AddListener(delegate {
                Context.OnCellClicked?.Invoke(Index);
                if (anim)
                    anim.Play(0);
            });
            btnMain.onClick.AddListener(eff.add_delegates);
            //genderImg.sprite = genderSprites[pigeonStat.gender];

            selectedObj.SetActive(Context.selectedIndex == Index);
        }

        private void OnCellClicked(int clickIndex)
        {
            selectedObj.SetActive(Index == clickIndex);
        }
        
        public void Button_View()
        {
            //uiTrain.ChangeMode(PigeonListViewMode.HalfPigeon);
        }

    }
}