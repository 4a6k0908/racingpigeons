﻿using System;
using System.Collections.Generic;
using Core.Aws;
using Core.Aws.Models;
using Core.MainScene;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.Effects.Models
{
    // 處理鴿子資料
    public class PigeonModel
    {
        private readonly SignalBus  signalBus;
        private readonly AwsGraphQL awsGraphQL;

        private List<PigeonStat> pigeonStatsList = new();

        public PigeonModel(SignalBus signalBus, AwsGraphQL awsGraphQL)
        {
            this.signalBus  = signalBus;
            this.awsGraphQL = awsGraphQL;
        }

        public async UniTask GetPigeonList(int queryCount, AwsUserModel awsUserModel)
        {
            var query = "{\"query\":\""                            +
                        "query MyQuery {\\n"                       +
                        "getPigeonList(page: {back: false, take: " + queryCount + "})" + "{\\n" +
                        "items {\\n"                               +
                        "age\\n"                                   +
                        "age_feature\\n"                           +
                        "breed_id\\n"                              +
                        "breed_name\\n"                            +
                        "constitution\\n"                          +
                        "exp\\n"                                   +
                        "father_pigeon_id\\n"                      +
                        "fatigue\\n"                               +
                        "feather_quality\\n"                       +
                        "feather_size\\n"                          +
                        "feature_a\\n"                             +
                        "feature_b\\n"                             +
                        "feature_c\\n"                             +
                        "free_points\\n"                           +
                        "gender\\n"                                +
                        "grand_father_pigeon_id\\n"                +
                        "grand_mother_pigeon_id\\n"                +
                        "iq\\n"                                    +
                        "level\\n"                                 +
                        "max_constitution\\n"                      +
                        "max_feather_quality\\n"                   +
                        "max_feather_size\\n"                      +
                        "max_iq\\n"                                +
                        "max_muscle\\n"                            +
                        "max_speed\\n"                             +
                        "max_vision\\n"                            +
                        "max_vitality\\n"                          +
                        "mother_pigeon_id\\n"                      +
                        "muscle\\n"                                +
                        "pigeon_id\\n"                             +
                        "pigeon_name\\n"                           +
                        "pigeon_pigeonry\\n"                       +
                        "speed\\n"                                 +
                        "vision\\n"                                +
                        "vitality\\n"                              +
                        "}\\n"                                     +
                        "next_cursor\\n"                           +
                        "}\\n"                                     +
                        "}"                                        +
                        "\",\"variables\":{}}";

            var responseContent = await awsGraphQL.Post(query, awsUserModel.accessToken);

            var data = JsonUtility.FromJson<GQL_GetPigeonList>(responseContent);

            Debug.Log($"Pigeon List: \n {JsonUtility.ToJson(data)}");

            pigeonStatsList = data.data.getPigeonList.items;

            signalBus.Fire(new OnPigeonListUpdate(pigeonStatsList));
        }

        public List<PigeonStat> GetPigeonStatsList() => pigeonStatsList;

        [Serializable]
        public class GQL_GetPigeonList
        {
            [Serializable]
            public class Data
            {
                [Serializable]
                public class MyPigeons
                {
                    public List<PigeonStat> items;

                    public string next_cursor;
                }

                public MyPigeons getPigeonList;
            }

            public Data data;
        }
    }
}