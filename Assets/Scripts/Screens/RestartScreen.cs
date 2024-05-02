using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class RestartScreen : BaseScreen
    {
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private void Awake()
        {
            yesButton.onClick.AddListener(OnRestartClick);
            noButton.onClick.AddListener(OnCloseThisScreenClick);
        }

        private void OnRestartClick()
        {

        }

        private void OnCloseThisScreenClick()
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
