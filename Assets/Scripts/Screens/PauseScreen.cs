using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class PauseScreen : BaseScreen
    {
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

        }

        private void OnRestartGameClick()
        {

        }

        private void OnOpenStartScreenClick()
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
