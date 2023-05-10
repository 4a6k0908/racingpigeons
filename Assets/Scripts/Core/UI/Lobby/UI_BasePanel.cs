using Core.LobbyScene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Lobby
{
    public class UI_BasePanel : MonoBehaviour
    {
        private SignalBus signalBus;

        [SerializeField] private Image    bgImg;
        [SerializeField] private Sprite[] bgSprites;

        [Inject]
        public void Inject(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnLobbyStateChange>(OnLobbyStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnLobbyStateChange>(OnLobbyStateChange);
        }

        private void OnLobbyStateChange(OnLobbyStateChange e)
        {
            bgImg.color = e.currentState == LobbyState.Lobby ? Color.clear : Color.white;

            bgImg.sprite = bgSprites[(int)e.currentState];
        }
    }
}