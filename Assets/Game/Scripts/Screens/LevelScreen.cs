using UnityEngine;
using Datas;
using UnityEngine.UI;
using Saves;
using Managers;
using TMPro;

namespace Screens
{
    public class LevelScreen : BaseScreen
    {
        [SerializeField] private Image levelImage;
        [SerializeField] private Button startButton;
        [SerializeField] private TMP_Text levelText;

        private const string _levelText = "Level "; 

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            AudioManager.Instance.ButtonClickSound();

            var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

            if (GameSaves.Instance.ShowTutorial())
            {
                var tutorialScreen = UIManager.Instance.GetScreen<TutorialScreen>();

                if (tutorialScreen != null)
                {
                    tutorialScreen.OpenScreen();
                    tutorialScreen.ShowTutorial();
                }

                if (gameScreen != null)
                {
                    gameScreen.SetupLevel();
                    gameScreen.ActivityAllElements(false);
                }
            }
            else
            {
                if (gameScreen != null)
                {
                    gameScreen.SetupLevel();
                }
            }

            CloseScreen();
        }

        public override void OpenScreen()
        {
            GameSaves.Instance.SetLevel();

            levelImage.sprite = PrefabsStorage.Instance.LevelsConfig.Levels[GameSaves.Instance.GetLevel()].LevelSprite;
            levelText.text = _levelText + (GameSaves.Instance.GetLevel() + 1).ToString();

            base.OpenScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }
    }
}