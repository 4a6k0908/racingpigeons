using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Effects.Lobby
{
    public class Pigeon3DRenderTexture : MonoBehaviour
    {
        private MaterialPropertyBlock materialPropertyBlock;
        
        [SerializeField] private Renderer[]         renders; // 鴿子的 Render
        [SerializeField] private BreedTextureData[] breedTextures;

        // 更換貼圖
        public void ChangeTexture(string breedID)
        {
            var breedGroupID = string.IsNullOrEmpty(breedID) ? 25 : int.Parse(breedID);

            for (int i = 0; i < renders.Length; i++)
            {
                materialPropertyBlock = new MaterialPropertyBlock();

                renders[i].GetPropertyBlock(materialPropertyBlock);

                var textures = breedTextures[breedGroupID].textures;
                
                materialPropertyBlock.SetTexture("_BaseMap", textures[Random.Range(0, textures.Length)]);
                
                renders[i].SetPropertyBlock(materialPropertyBlock);
            }
        }

        // 貼圖品種 Group
        [Serializable]
        private class BreedTextureData
        {
            public Texture[] textures;
        }
    }
}