using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private LevelsConfig levelsConfig;
        [SerializeField] private Image levelImage;
        [SerializeField] private Button startButton;
        [SerializeField] private TMP_Text levelText;

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            AudioManager.Instance.ButtonClickSound();
            var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

            if(gameScreen != null)
            {
                gameScreen.SetupLevel();
            }

            CloseScreen();
        }

        public override void OpenScreen()
        {
            GameSaves.Instance.SetLevel();

            levelImage.sprite = levelsConfig.Levels[GameSaves.Instance.GetLevel()].LevelSprite;
            levelText.text = "Level " + (GameSaves.Instance.GetLevel() + 1).ToString();

            base.OpenScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }
    }
}