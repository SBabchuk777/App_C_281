using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Views
{
    public class SlotView : MonoBehaviour
    {
        public Action CheckKeyAction;

        [SerializeField] private RectTransform mainImage;
        [SerializeField] private RectTransform downPos;

        [SerializeField] private List<Image> slotImages = new List<Image>();
        [SerializeField] private List<RectTransform> startPosition = new List<RectTransform>();

        private bool isSpinning = false;
        private float _spinDuration = 3f;
        private Sequence _spinSequence;

        public string Key { get; private set; }

        private void OnEnable()
        {
            slotImages[0].rectTransform.position = mainImage.position;
        }

        private void OnDisable()
        {
            DefaultPosSlot();
        }

        public void StartSpin()
        {
            if (!isSpinning)
            {
                isSpinning = true;

                _spinSequence = DOTween.Sequence();

                for (int i = 0; i < slotImages.Count; i++)
                {
                    RectTransform image = slotImages[i].rectTransform;
                    Vector3 initialPosition = startPosition[i].localPosition;

                    _spinSequence.Append(image.DOMove(downPos.position, 0.5f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            image.localPosition = initialPosition;
                        }));
                }

                _spinSequence.OnUpdate(() =>
                {
                    if (_spinSequence.Elapsed() >= _spinDuration)
                    {
                        _spinSequence.Kill();
                        var lastImage = slotImages[UnityEngine.Random.Range(0, slotImages.Count - 1)]; 

                        lastImage.rectTransform.DOMove(mainImage.position, 0.5f)
                            .SetEase(Ease.Linear)
                            .OnComplete(() =>
                            {
                                Key = lastImage.sprite.name;
                                isSpinning = false;
                                CheckKeyAction?.Invoke();
                            });
                    }
                });

                _spinSequence.Play();
            }
        }

        public void DefaultPosSlot()
        {
            for (int i = 0; i < slotImages.Count; i++)
            {
                slotImages[i].rectTransform.position = startPosition[i].position;
            }
        }
    }
}