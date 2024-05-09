using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Screens
{
    public class SettingsScreen : BaseScreen
    {
        [SerializeField] private Button privacyPolicyButton;
        [SerializeField] private Button termsOfUseButton;

        [SerializeField] private Button backButton;

        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundToggle;

        private void Awake()
        {
            privacyPolicyButton.onClick.AddListener(PrivacyPolicy);
            termsOfUseButton.onClick.AddListener(TermsUse);
            backButton.onClick.AddListener(OpenStartScreen);
        }

        private void Start()
        {
            musicToggle.isOn = AudioManager.Instance.GetActivityMusic();
            soundToggle.isOn = AudioManager.Instance.GetActivitySound();

            musicToggle.onValueChanged.AddListener(OnMusicToggleChange);
            soundToggle.onValueChanged.AddListener(OnSoundToggleChange);
        }

        public void OnMusicToggleChange(bool isON)
        {
            AudioManager.Instance.ButtonClickSound();
            AudioManager.Instance.SetMusic(isON);
        }

        public void OnSoundToggleChange(bool isON)
        {
            AudioManager.Instance.ButtonClickSound();
            AudioManager.Instance.SetSound(isON);
        }

        private void PrivacyPolicy()
        {
            AudioManager.Instance.ButtonClickSound();
        }
        private void TermsUse()
        {
            AudioManager.Instance.ButtonClickSound();
        }

        private void OpenStartScreen()
        {
            CloseScreen();
            UIManager.Instance.OpenScreen<StartScreen>();
        }


        public override void CloseScreen()
        {
            base.CloseScreen();
        }

        public override void OpenScreen()
        {
            musicToggle.isOn = AudioManager.Instance.GetActivityMusic();
            soundToggle.isOn = AudioManager.Instance.GetActivitySound();

            base.OpenScreen();
        }
    }
}
