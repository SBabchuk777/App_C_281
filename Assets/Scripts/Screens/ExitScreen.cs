using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class ExitScreen : BaseScreen
    {
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private void Awake()
        {
            yesButton.onClick.AddListener(OnOpenStartScreenClick);
            noButton.onClick.AddListener(OnCloseThisScreenClick);
        }

        private void OnOpenStartScreenClick()
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
