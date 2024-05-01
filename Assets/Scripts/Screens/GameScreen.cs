using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Datas;
using Views;

namespace Screens
{
    public class GameScreen : BaseScreen
    {
        public GridLayoutGroup GridLayout => _gridLayout;

        [Header("GridLayout")]
        [SerializeField] private GridLayoutGroup grid3x3;
        [SerializeField] private GridLayoutGroup grid4x4;
        [SerializeField] private RectTransform elementParents;

        [Header("Config")]
        [SerializeField] private LevelsConfig levelsConfig;

        [Header("Prefab")]
        [SerializeField] private ElementPuzzleView puzzleView;

        private GridLayoutGroup _gridLayout;
        private List<ElementPuzzleView> _elementPuzzleViews = new List<ElementPuzzleView>();

        private void Start()
        {
            SetupLevel();
        }

        public void SetupLevel()
        {
            var currentLevel = levelsConfig.Levels[0];
            Vector2 parentMin = elementParents.rect.min;
            Vector2 parentMax = elementParents.rect.max;

            if (currentLevel.CountElements <= 9)
            {
                SetGrid(true);
            }
            else
            {
                SetGrid(false);
            }

            for (int i = 0; i < currentLevel.ElementsPuzzles.Count; i++)
            {
                var elementView = Instantiate(puzzleView, elementParents);
                elementView.SetupElement(currentLevel.ElementsPuzzles[i].ElementSprite, currentLevel.ElementsPuzzles[i].ElementIndex);

                float randomX = UnityEngine.Random.Range(parentMin.x, parentMax.x);
                float randomY = UnityEngine.Random.Range(parentMin.y, parentMax.y);

                // Устанавливаем позицию элемента пазла
                RectTransform elementRect = elementView.GetComponent<RectTransform>();
                elementRect.anchoredPosition = new Vector2(randomX, randomY);

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

        public void SetGrid(bool active)
        {
            grid3x3.gameObject.SetActive(active);
            grid4x4.gameObject.SetActive(!active);

            _gridLayout = active ? grid3x3.gameObject.activeSelf
                ? grid3x3 : grid4x4 : grid4x4.gameObject.activeSelf
                ? grid4x4 : grid3x3;

        }
    }
}