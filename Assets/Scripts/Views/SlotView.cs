using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Views
{
    public class SlotView : MonoBehaviour
    {
        public Action SetRandomSlotsAction;
        public Action CheckKeyAction;

        public Image SpinImage => spinImage;
        public List<Sprite> SpinSprites => spinSprites;

        [SerializeField] private RectTransform startPos;
        [SerializeField] private RectTransform allImages;
        [SerializeField] private RectTransform downPos;

        [SerializeField] private Image spinImage;

        [SerializeField] private List<Image> slotImages = new List<Image>();
        [SerializeField] private List<Sprite> spinSprites = new List<Sprite>();
        [SerializeField] private List<RectTransform> startPosition = new List<RectTransform>();

        private bool isSpinning = false;

        public string Key { get; private set; }

        private void OnEnable()
        {
            allImages.position = startPos.position;
            SetImage();
        }

        private void OnDisable()
        {
            allImages.position = startPos.position;
        }

        public void SetImage()
        {
            for (int i = 0; i < slotImages.Count; i++)
            {
                slotImages[i].sprite = spinSprites[UnityEngine.Random.Range(0, spinSprites.Count)];
            }
        }

        public void StartSpin()
        {
            if (!isSpinning)
            {
                SetRandomSlotsAction?.Invoke();
                isSpinning = true;

                allImages.transform.DOMove(downPos.position,2f).OnComplete (() =>
                {
                    isSpinning = false;
                    CheckKeyAction?.Invoke();
                });

            }
        }

        public int GetRandomIndex()
        {
            return UnityEngine.Random.Range(0, spinSprites.Count);
        }
    }
}