using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class WinOrLosePopup : BaseScreen
    {
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

        }


        private void OnRestartLevelClick()
        {

        }


        private void OnOpenStartScreenClick()
        {
        
        }

        public void SetupPanel(bool win)
        {
            winPanel.gameObject.SetActive(win);
            losePanel.gameObject.SetActive(!win);
        }


        public override void CloseScreen()
        {
            base.CloseScreen();
        }

        public override void OpenScreen()
        {
            base.OpenScreen();
        }
    }
}