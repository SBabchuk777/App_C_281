using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Screens
{
    public class StartScreen : BaseScreen
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button dailyBonusButton;
        [SerializeField] private Button settingsButton;

        private void Awake()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClick);
            dailyBonusButton.onClick.AddListener(OnOpenSpinScreenClick);
            settingsButton.onClick.AddListener(OnOpenSettingsScreenClick);
        }

        private void OnStartGameButtonClick()
        {
            UIManager.Instance.OpenScreen<GameScreen>();
            UIManager.Instance.OpenScreen<LevelScreen>();

            CloseScreen();
        }

        private void OnOpenSpinScreenClick()
        {

        }

        private void OnOpenSettingsScreenClick()
        {
            CloseScreen();

            UIManager.Instance.OpenScreen<SettingsScreen>();
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