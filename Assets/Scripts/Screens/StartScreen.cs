using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using Views;

namespace Screens
{
    public class StartScreen : BaseScreen
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button dailyBonusButton;
        [SerializeField] private Button settingsButton;

        [SerializeField] private CurrencyView currencyView;

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
            CloseScreen();
            UIManager.Instance.OpenScreen<SpinScreen>();
        }

        private void OnOpenSettingsScreenClick()
        {
            CloseScreen();

            UIManager.Instance.OpenScreen<SettingsScreen>();
        }

        public override void OpenScreen()
        {
            currencyView.UpdateCoinText();
            base.OpenScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }
    }
}