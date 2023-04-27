using AnimeTask;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.SoundSystem
{
    // 處理背景音樂
    public class BGMHandler
    {
        private readonly SoundView view;

        private AudioClip currentClip;

        public BGMHandler(SoundView view)
        {
            this.view = view;
        }

        // 播放背景音樂
        public async void Play(AssetReferenceT<AudioClip> clipAsset, float volume = 1)
        {
            if (clipAsset == null)
            {
                Change(null, volume);
            }
            else
            {
                AudioClip clip = await Addressables.LoadAssetAsync<AudioClip>(clipAsset).Task;
                Change(clip, volume);
            }
        }

        // 更改背景音樂的實作
        private async void Change(AudioClip clip, float volume)
        {
            var sound = view.GetBgmSound();

            await Easing.Create<Linear>(1, 0, 0.25f).ToAction(delta => sound.volume = delta);

            sound.Stop();
            sound.clip = clip;

            if (currentClip != null)
                Addressables.Release(currentClip);

            currentClip = clip;

            if (clip == null)
                return;

            sound.Play();

            await Easing.Create<Linear>(0, volume, 0.25f).ToAction(delta => sound.volume = delta);
        }
    }
}