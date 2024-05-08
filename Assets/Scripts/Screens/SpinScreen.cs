using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using System;
using Views;
using Managers;
using Saves;

namespace Screens
{
    public class SpinScreen : BaseScreen
    {
        public static event Action<bool, bool> ScrollSpin;

        [SerializeField] private List<SlotView> slotViews = new List<SlotView>();
        [SerializeField] private Button spinButton;
        [SerializeField] private Button menuButton;

        [SerializeField] private RectTransform firstText;
        [SerializeField] private RectTransform loseText;
        [SerializeField] private RectTransform winText;

        public static bool IsJackPotChecked = false;

        private int _coinJackPot = 500;

        private void Awake()
        {
            spinButton.onClick.AddListener(StartGameSpin);
            menuButton.onClick.AddListener(OnOpenStartScreen);
        }

        private void Start()
        {
            slotViews[2].CheckKeyAction += OnCheckJackPot;
        }

        private void OnOpenStartScreen()
        {
            NotActiveObject();
            CloseScreen();
            UIManager.Instance.OpenScreen<StartScreen>();
        }

        public void ClosePopupAndSpinScreen()
        {
            UIManager.Instance.OpenScreen<StartScreen>();
            CloseScreen();
        }

        private void SetCancelButtonNegative(bool active)
        {
            spinButton.enabled = active;
        }

        private async void OnCheckJackPot()
        {
            if (!IsJackPotChecked)
            {
                var key = slotViews[0].Key;

                if (slotViews.All(slot => slot.Key == key))
                {
                    //GameSaves.AddCoin(_coinJackPot);
                    IsJackPotChecked = true;
                   // AudioManager.Instance.WinSlotSound();
                }
                else
                {
                   // AudioManager.Instance.LoseSlotSound();
                }
            }

            await Task.Delay(700);

            SetTextJackpot(IsJackPotChecked);
            menuButton.gameObject.SetActive(true);
        }

        private void StartGameSpin()
        {
            //GameSaves.ClaimReward(true);
            //AudioManager.Instance.ScrollSlotSound();

            SetCancelButtonNegative(false);

            for (int i = 0; i < slotViews.Count; i++)
            {
                slotViews[i].StartSpin();
            }
        }

        public override void OpenScreen()
        {
            firstText.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(false);
            SetCancelButtonNegative(true);
            base.OpenScreen();
        }

        public void NotActiveObject()
        {
            menuButton.gameObject.SetActive(false);
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
        }

        private void SetTextJackpot(bool win)
        {
            firstText.gameObject.SetActive(false);

            if (win)
                winText.gameObject.SetActive(true);
            else
                loseText.gameObject.SetActive(true);
        }

        public override void CloseScreen()
        {
            IsJackPotChecked = false;
            base.CloseScreen();
        }
    }
}