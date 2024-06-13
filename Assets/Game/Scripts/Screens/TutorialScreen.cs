using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Views;
using Saves;
using Datas;
using Managers;
using System;

namespace Screens
{
    public class TutorialScreen : BaseScreen
    {
        public static event Action<bool> CloseTutorial;

        [SerializeField] private Button closeTutorial;
        
        [SerializeField] private GridLayoutGroup grid3x3;
        [SerializeField] private RectTransform elementParents;
        [SerializeField] private RectTransform firstPuzzleParent;

        [Header("Tutorial")]
        [SerializeField] private Image handTutorial;
        [SerializeField] private Vector3 offestHands;
        [SerializeField] private float animationTime;
        [SerializeField] private float fadeTime;

        private List<ElementPuzzleView> _elementPuzzleViews = new List<ElementPuzzleView>();
        private List<GridView> _gridViews = new List<GridView>();

        private bool isEndShowTutorial;

        private ElementPuzzleView _firstCard;
        private Vector3 _cardPosition;

        public async void ShowTutorial()
        {
            var currentLevel = PrefabsStorage.Instance.LevelsConfig.Levels[GameSaves.Instance.GetLevel()];

            SetupGridViews(currentLevel);
            SetupElemets(currentLevel);

            ActiveTutorialObjects(true);
            await Task.Delay(500);

            var firstCard = _elementPuzzleViews.FirstOrDefault();

            var cardPosition = firstCard.transform.position;

            var cardTarget = _gridViews.FirstOrDefault(item => item.ElementPuzzleIndex == firstCard.ElementIndex);

            handTutorial.transform.DOMove(firstCard.transform.position + offestHands, 0);

            handTutorial.DOFade(1, fadeTime).OnComplete(() =>
            {
                firstCard.transform.SetParent(firstPuzzleParent);
                handTutorial.transform.DOMove(cardTarget.transform.position + offestHands, animationTime);
                firstCard.transform.DOMove(cardTarget.transform.position, animationTime).OnComplete(() =>
                {
                    _firstCard = firstCard;
                    _cardPosition = cardPosition;
                    isEndShowTutorial = true;
                    //CallbackTutorial(firstCard, cardPosition);
                });
            });
        }

        public async void CallbackTutorial(ElementPuzzleView elementPuzzleView, Vector3 position)
        {
            await Task.Delay(300);

            //elementPuzzleView.transform.position = position;

            handTutorial.DOFade(0, fadeTime).OnComplete(() =>
            {
                isEndShowTutorial = true;
                 CloseTutorial?.Invoke(true);
                 ActiveTutorialObjects(false);
                 CloseScreen();
            });
        }

        public void OnCloseTutorial()
        {
            if (isEndShowTutorial)
            {
                GameSaves.Instance.ShowTutorial();
                CallbackTutorial(_firstCard, _cardPosition);
            }
        }

        public void ActiveTutorialObjects(bool active)
        {
            handTutorial.gameObject.SetActive(active);
        }

        public void SetupGridViews(LevelsConfig.Level currentLevel)
        {
            for (int i = 0; i < currentLevel.ElementsPuzzles.Count; i++)
            {
                var newGridView = Instantiate(PrefabsStorage.Instance.GridView, grid3x3.transform);
                newGridView.SetupIndex(currentLevel.ElementsPuzzles[i].ElementIndex);
                _gridViews.Add(newGridView);
            }
        }

        public void SetupElemets(LevelsConfig.Level currentLevel)
        {
            Vector2 parentMin = elementParents.rect.min;
            Vector2 parentMax = elementParents.rect.max;

            for (int i = 0; i < currentLevel.ElementsPuzzles.Count; i++)
            {
                var elementView = Instantiate(PrefabsStorage.Instance.PuzzleView, elementParents);
                Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(parentMin.x, parentMax.x),
                                                    UnityEngine.Random.Range(parentMin.y, parentMax.y));


                elementView.SetupElement(currentLevel.ElementsPuzzles[i].ElementSprite, currentLevel.ElementsPuzzles[i].ElementIndex, randomPosition);
                _elementPuzzleViews.Add(elementView);
            }
        }

        public void ClearElements()
        {
            for (int i = 0; i < _elementPuzzleViews.Count; i++)
            {
                Destroy(_elementPuzzleViews[i].gameObject);
            }

            _elementPuzzleViews.Clear();
        }

        public void ClearGrids()
        {
            for (int i = 0; i < _gridViews.Count; i++)
            {
                Destroy(_gridViews[i].gameObject);
            }

            _gridViews.Clear();
        }

        public override void OpenScreen()
        {
            GameSaves.Instance.DeleteKeyTutorial();   
            base.OpenScreen();
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
            ClearGrids();
            ClearElements();
        }
    }
}
