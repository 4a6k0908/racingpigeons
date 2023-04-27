using UnityEngine;

namespace Core.SoundSystem
{
    // 取得 Unity 播放器物件
    public class SoundView : MonoBehaviour
    {
        [SerializeField] private AudioSource   bgmSound; // 取得背景音樂物件
        [SerializeField] private AudioSource   loopSound; // 取得循環音效物件
        [SerializeField] private AudioSource[] effectSound; // 取得音效物件

        private int effectSoundIndex = 0; // 要用哪一個音效物件

        public AudioSource GetBgmSound() => bgmSound; // 取得背景音效播放器
        public AudioSource GetLoopSound() => loopSound; // 取得循環音效播放器
        public AudioSource GetEffectSound() // 取得音效播放器
        {
            AudioSource sound = effectSound[effectSoundIndex];

            effectSoundIndex++;

            if (effectSoundIndex >= effectSound.Length)
                effectSoundIndex = 0;

            return sound;
        }

        public void SetBGMSound(AudioSource sound) => bgmSound = sound;
        public void SetEffectSound(AudioSource[] sound) => effectSound = sound;
        public void SetLoopSound(AudioSource sound) => loopSound = sound;
    }
}