using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Screens
{
    public class SettingsScreen : BaseScreen
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button privacyButon;
        [SerializeField] private Button termsOfUseButton;

        private void Awake()
        {
            backButton.onClick.AddListener(OnOpenStartScreen);
            privacyButon.onClick.AddListener(OnPrivacyClick);
            termsOfUseButton.onClick.AddListener(OnTermsOfUseClick);
        }

        private void OnPrivacyClick()
        {

        }

        private void OnTermsOfUseClick()
        {

        }

        private void OnOpenStartScreen()
        {
            CloseScreen();

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
