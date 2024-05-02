using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class LevelScreen : BaseScreen
    {
        [SerializeField] private Image levelImage;
        [SerializeField] private Button startButton;

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