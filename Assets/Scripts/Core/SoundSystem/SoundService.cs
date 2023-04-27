using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Core.SoundSystem
{
    public class SoundService : ISoundService
    {
        private readonly BGMHandler    bgmHandler; // 處理背景音樂播放
        private readonly EffectHandler effectHandler; // 處理音效播放
        private readonly LoopHandler   loopHandler; // 處理循環音效播放

        public SoundService(BGMHandler bgmHandler, EffectHandler effectHandler, LoopHandler loopHandler)
        {
            this.bgmHandler    = bgmHandler;
            this.effectHandler = effectHandler;
            this.loopHandler   = loopHandler;
        }

        public void DoPlayBGM(AssetReferenceT<AudioClip> clip, float volume = 1) => bgmHandler.Play(clip, volume);
        public void DoPlaySound(AssetReferenceT<AudioClip> clip, float volume = 1, float pitch = 1) => effectHandler.Play(clip, volume, pitch);
        public void DoPlaySound3D(AssetReferenceT<AudioClip> clip, Vector3 position, float volume = 1) => effectHandler.Play3D(clip, position, volume);
        public void DoPlayLoop(AssetReferenceT<AudioClip> clip, float volume = 1) => loopHandler.Play(clip, volume);
    }
}