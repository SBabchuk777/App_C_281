using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Managers;

namespace Screens
{
    public class PauseScreen : BaseScreen
    {
        public static Action RestartGameAction;
        public static Action ExitGameAction;

        [SerializeField] private Button playButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayGameClick);
            restartButton.onClick.AddListener(OnRestartGameClick);
            exitButton.onClick.AddListener(OnOpenStartScreenClick);           
        }

        private void OnPlayGameClick()
        {
            CloseScreen();
        }

        private void OnRestartGameClick()
        {
            CloseScreen();
            RestartGameAction?.Invoke();
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
