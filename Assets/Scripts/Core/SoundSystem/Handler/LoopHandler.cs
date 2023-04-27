using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.SoundSystem
{
    // 處理循環音效播放
    public class LoopHandler
    {
        private readonly SoundView view;

        private AudioClip currentClip;

        public LoopHandler(SoundView view)
        {
            this.view = view;
        }

        public async void Play(AssetReferenceT<AudioClip> clipAsset, float volume)
        {
            var sound = view.GetLoopSound();

            sound.Stop();
            sound.clip = null;

            if (currentClip != null)
            {
                Addressables.Release(currentClip);
                currentClip = null;
            }

            if (clipAsset == null)
                return;

            currentClip = await Addressables.LoadAssetAsync<AudioClip>(clipAsset).Task;

            sound.clip   = currentClip;
            sound.volume = volume;

            sound.Play();
        }
    }
}