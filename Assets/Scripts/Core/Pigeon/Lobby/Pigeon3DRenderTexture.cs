using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Pigeon.Lobby
{
    public class Pigeon3DRenderTexture : MonoBehaviour
    {
        private MaterialPropertyBlock materialPropertyBlock;

        [SerializeField] private Renderer[]         renders;
        [SerializeField] private BreedTextureData[] breedTextures;

        public void ChangeTexture(string breedID)
        {
            var breedGroupID = breedID == null ? 25 : int.Parse(breedID);

            Debug.Log($"change texture: {breedID}");

            for (int i = 0; i < renders.Length; i++)
            {
                materialPropertyBlock = new MaterialPropertyBlock();

                renders[i].GetPropertyBlock(materialPropertyBlock);

                var textures = breedTextures[breedGroupID].textures;
                
                materialPropertyBlock.SetTexture("_BaseMap", textures[Random.Range(0, textures.Length)]);
                
                renders[i].SetPropertyBlock(materialPropertyBlock);
            }
        }

        [Serializable]
        private class BreedTextureData
        {
            public Texture[] textures;
        }
    }
}