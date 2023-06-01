using System;
using System.Collections.Generic;
using Core.Aws;
using Core.Aws.Models;
using Core.MainScene;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using static Core.Effects.Models.EffectModel.GQL_GetEffectList.Data;

namespace Core.Effects.Models
{
    // 處理鴿子資料
    public class EffectModel
    {
        private readonly SignalBus  signalBus;
        private readonly AwsGraphQL awsGraphQL;

        private List<Effect> effectList = new();

        public EffectModel(SignalBus signalBus, AwsGraphQL awsGraphQL)
        {
            this.signalBus  = signalBus;
            this.awsGraphQL = awsGraphQL;
        }

        public enum YourEnumType
        {
            train_effect
        }

        public async UniTask GetEffects(YourEnumType queryType, AwsUserModel awsUserModel)
        {
            var query = "{\"query\":\"" +
                        "query MyQuery {\\n" +
                        "getEffects(effect_type: " + queryType + ")" + "{\\n" +
                        "effect_id\\n" +
                        "effect_name\\n" +
                        "effect_price\\n" +
                        "}\\n" +
                        "}" +
                        "\",\"variables\":{}}";

            var responseContent = await awsGraphQL.Post(query, awsUserModel.accessToken);

            var data = JsonUtility.FromJson<GQL_GetEffectList>(responseContent);

            Debug.Log($"Effect List: \n {JsonUtility.ToJson(data)}");

            effectList = data.data.getEffects;

            signalBus.Fire(new OnEffectsUpdate(effectList));
        }

        public List<Effect> GetEffectsList() => effectList;

        [Serializable]
        public class GQL_GetEffectList
        {
            [Serializable]
            public class Data
            {
                [Serializable]
                public class Effect
                {
                    public string effect_id;

                    public string effect_name;

                    public string effect_price;
                }

                public List<Effect> getEffects;
            }

            public Data data;
        }
    }
}