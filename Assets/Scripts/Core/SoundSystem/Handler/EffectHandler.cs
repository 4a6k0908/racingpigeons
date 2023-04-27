using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.SoundSystem
{
    // 處理音效
    public class EffectHandler
    {
        private readonly SoundView view;

        public EffectHandler(SoundView view)
        {
            this.view = view;
        }

        // 播放音效
        public async void Play(AssetReferenceT<AudioClip> clipAsset, float volume = 1, float pitch = 1)
        {
            if (clipAsset == null)
                return;

            AudioClip clip = await Addressables.LoadAssetAsync<AudioClip>(clipAsset).Task;

            var sound = view.GetEffectSound();

            sound.pitch        = pitch;
            sound.spatialBlend = 0;
            sound.PlayOneShot(clip, volume);

            Observable.Timer(TimeSpan.FromSeconds(clip.length + 1))
                      .Subscribe(l => Addressables.Release(clip));
        }

        // 處理 3D 音效
        public async void Play3D(AssetReferenceT<AudioClip> clipAsset, Vector3 position, float volume = 1)
        {
            if (clipAsset == null)
                return;

            AudioClip clip = await Addressables.LoadAssetAsync<AudioClip>(clipAsset).Task;

            var sound = view.GetEffectSound();

            sound.transform.position = position;
            sound.spatialBlend       = 1.0f;
            sound.PlayOneShot(clip, volume);

            Observable.Timer(TimeSpan.FromSeconds(clip.length + 1))
                      .Subscribe(l => Addressables.Release(clip));
        }
    }
}