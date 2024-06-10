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

        [SerializeField] private Button bonusButton;

        [Header("TextRects")]
        [SerializeField] private RectTransform firstText;
        [SerializeField] private RectTransform loseText;
        [SerializeField] private RectTransform winText;

        [Header("Images")]
        [SerializeField] private Sprite claimRewardSprite;
        [SerializeField] private Sprite menuSprite;
        [SerializeField] private Sprite spinSprite;

        public  bool isJackPotChecked = false;
        private bool isSpin = false;

        private int _coinJackPot = 100;    

        private void Awake()
        {
            bonusButton.onClick.AddListener(StartGameSpin);
        }

        private void Start()
        {
            slotViews[2].CheckKeyAction += OnCheckJackPot;
            slotViews[2].SetRandomSlotsAction += OnSetRandomImage;
        }

        private void OnDestroy()
        {
            slotViews[2].CheckKeyAction -= OnCheckJackPot;
            slotViews[2].SetRandomSlotsAction -= OnSetRandomImage;
        }

        private void OnSetRandomImage()
        {
            var randomIndex = UnityEngine.Random.Range(0,100);

            if(randomIndex % 2 == 0)
            {
                var randomImage = slotViews[2].GetRandomIndex();

                for (int i = 0; i < slotViews.Count; i++)
                {
                    slotViews[i].SpinImage.sprite = slotViews[i].SpinSprites[randomImage];
                }
            }
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
            bonusButton.interactable = active;
        }

        private async void OnCheckJackPot()
        {
            if (!isJackPotChecked)
            {
                var key = slotViews[0].SpinImage.sprite.name;

                if (slotViews.All(slot => slot.SpinImage.sprite.name == key))
                {
                    isJackPotChecked = true;
                    AudioManager.Instance.WinSlotSound();
                }
                else
                {
                    AudioManager.Instance.LoseSlotSound();
                }
            }

            await Task.Delay(700);

            SetCancelButtonNegative(true);
            SetTextJackpot(isJackPotChecked);
            SetButtons();
        }

        private void StartGameSpin()
        {
            GameSaves.Instance.ClaimReward(true);

            SetCancelButtonNegative(false);  

            if (!isSpin)
            {
                StartSpin();
            }
            else if(isSpin && isJackPotChecked)
            {
                ClaimReward();
            }
            else if (isSpin && !isJackPotChecked)
            {
                GoToMenu();
            }
        }

        public void StartSpin()
        {
            AudioManager.Instance.ScrollSlotSound();

            for (int i = 0; i < slotViews.Count; i++)
            {
                slotViews[i].StartSpin();
            }

            isSpin = true;
        }

        public void ClaimReward()
        {
            AudioManager.Instance.ClaimRewardSound();
            isJackPotChecked = false;
            GameSaves.Instance.AddStarCoin(_coinJackPot);
            SetButtons();
            SetCancelButtonNegative(true);
        }

        public void GoToMenu()
        {
            AudioManager.Instance.ButtonClickSound();
            OnOpenStartScreen();
            SetButtons();
        }

        public void SetButtons()
        {
            if (!isSpin)
            {
                bonusButton.image.sprite = spinSprite;
            }
            else if (isSpin && isJackPotChecked)
                bonusButton.image.sprite = claimRewardSprite;
            else if (isSpin && !isJackPotChecked)
            {
                bonusButton.image.sprite = menuSprite;
            }
        }

        public override void OpenScreen()
        {
            SetButtons();
            firstText.gameObject.SetActive(true);
            SetCancelButtonNegative(true);
            base.OpenScreen();
        }

        public void NotActiveObject()
        {
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
            SetCancelButtonNegative(true);
            isJackPotChecked = false;
            isSpin = false;
            base.CloseScreen();
        }
    }
}