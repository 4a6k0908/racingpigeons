using System;
using Core.Title;
using UnityEngine;
using Zenject;

namespace UI.Title
{
    public class UI_Title : MonoBehaviour
    {
        private SignalBus    signalBus;
        private StateHandler stateHandler;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        private void Inject(SignalBus signalBus, StateHandler stateHandler)
        {
            this.signalBus    = signalBus;
            this.stateHandler = stateHandler;
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnStateChange>(OnStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnStateChange>(OnStateChange);
        }

        public void Button_To_Login()
        {
            stateHandler.ChangeState(State.Login);
        }

        private async void OnStateChange(OnStateChange e)
        {
            if (e.preState == State.Login && e.state != State.Title)
            {
                
                
                canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
            }
        }
    }
}