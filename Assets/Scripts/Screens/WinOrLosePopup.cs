using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Managers;

namespace Screens
{
    public class WinOrLosePopup : BaseScreen
    {
        public static Action NextLevelAction;
        public static Action OpenStartScreenAction;

        [SerializeField] private RectTransform winPanel;
        [SerializeField] private RectTransform losePanel;

        [SerializeField] private Button nextLevel;
        [SerializeField] private Button restartLevel;
        [SerializeField] private Button homeLose;
        [SerializeField] private Button homeWin;

        private void Awake()
        {
            nextLevel.onClick.AddListener(OnNextLevelClick);
            restartLevel.onClick.AddListener(OnRestartLevelClick);
            homeLose.onClick.AddListener(OnOpenStartScreenClick);
            homeWin.onClick.AddListener(OnOpenStartScreenClick);
        }

        private void OnNextLevelClick()
        {
            CloseScreen();
            NextLevelAction?.Invoke();
        }


        private void OnRestartLevelClick()
        {
            CloseScreen();
            NextLevelAction?.Invoke();
        }


        private void OnOpenStartScreenClick()
        {
            CloseScreen();
            OpenStartScreenAction?.Invoke();
            UIManager.Instance.OpenScreen<StartScreen>();
        }

        public void SetupPanel(bool win)
        {
            winPanel.gameObject.SetActive(win);
            losePanel.gameObject.SetActive(!win);
        }


        public override void CloseScreen()
        {
            base.CloseScreen();

            winPanel.gameObject.SetActive(false);
            losePanel.gameObject.SetActive(false);
        }

        public override void OpenScreen()
        {
            base.OpenScreen();
        }
    }
}