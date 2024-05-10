using UnityEngine;
using System;
using UnityEngine.UI;

namespace Screens
{
    public class RestartScreen : BaseScreen
    {
        public static Action RestartGameAction;

        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private void Awake()
        {
            yesButton.onClick.AddListener(OnRestartClick);
            noButton.onClick.AddListener(CloseScreen);
        }

        private void OnRestartClick()
        {
            RestartGameAction?.Invoke();
            CloseScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }
    }
}
