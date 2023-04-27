using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.SoundSystem
{
    public interface ISoundService
    {
        void DoPlayBGM(AssetReferenceT<AudioClip> clip, float volume = 1); // 播放 BGM
        void DoPlaySound(AssetReferenceT<AudioClip> clip, float volume = 1, float pitch = 1); // 播放音效
        void DoPlaySound3D(AssetReferenceT<AudioClip> clip, Vector3 position, float volume = 1); // 播放 3D 音效
        void DoPlayLoop(AssetReferenceT<AudioClip> clip, float volume = 1); // 播放循環音效
    }
}