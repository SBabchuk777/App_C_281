using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Datas;
using UnityEngine.UI;
using Managers;

namespace Screens
{
    public class LevelScreen : BaseScreen
    {
        [SerializeField] private LevelsConfig levelsConfig;
        [SerializeField] private Image levelImage;
        [SerializeField] private Button startButton;

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

            if(gameScreen != null)
            {
                gameScreen.SetupLevel();
            }

            CloseScreen();
        }

        public override void OpenScreen()
        {
            levelImage.sprite = levelsConfig.Levels[3].LevelSprite;

            base.OpenScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }
    }
}