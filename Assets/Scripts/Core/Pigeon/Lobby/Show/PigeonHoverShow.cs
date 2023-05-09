using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Pigeon.Lobby.Show
{
    public class PigeonHoverShow : MonoBehaviour
    {
        [SerializeField] private Animator pathAnim; // 路徑 Animator
        [SerializeField] private Animator pigeonAnim; // 鴿子 Animator

        private void Awake()
        {
            pathAnim.SetFloat("Speed", Random.Range(1, 2.0f)); // 路徑動畫速度
            pigeonAnim.SetBool("IsFlying", true); // 鴿子播放飛行動畫
        }
    }
}