using UnityEngine;
using UnityEngine.UI;
using System;
using Managers;

namespace Screens
{
    public class ExitScreen : BaseScreen
    {
        public static Action ExitGameAction;

        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private void Awake()
        {
            yesButton.onClick.AddListener(OnOpenStartScreenClick);
            noButton.onClick.AddListener(CloseScreen);
        }

        private void OnOpenStartScreenClick()
        {
            CloseScreen();
            ExitGameAction?.Invoke();
            UIManager.Instance.OpenScreen<StartScreen>();
        }

        public override void OpenScreen()
        {
            base.OpenScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }
    }
}
