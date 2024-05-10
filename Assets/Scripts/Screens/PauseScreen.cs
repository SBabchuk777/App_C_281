using UnityEngine;
using UnityEngine.UI;
using Managers;

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
            exitButton.onClick.AddListener(OnOpenExitScreenClick);           
        }

        private void OnPlayGameClick()
        {
            CloseScreen();
        }

        private void OnRestartGameClick()
        {
            CloseScreen();
            UIManager.Instance.OpenScreen<RestartScreen>();
        }

        private void OnOpenExitScreenClick()
        {
            CloseScreen();
            UIManager.Instance.OpenScreen<ExitScreen>();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }
    }
}
