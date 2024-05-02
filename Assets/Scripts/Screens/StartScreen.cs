using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        }

        private void OnOpenSpinScreenClick()
        {

        }

        private void OnOpenSettingsScreenClick()
        {

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