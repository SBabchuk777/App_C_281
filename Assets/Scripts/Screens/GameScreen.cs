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

        public List<GridView> GridViews => _gridViews;

        [Header("GridLayout")]
        [SerializeField] private GridLayoutGroup grid3x3;
        [SerializeField] private GridLayoutGroup grid4x4;
        [SerializeField] private RectTransform elementParents;

        [Header("Config")]
        [SerializeField] private LevelsConfig levelsConfig;

        [Header("Prefab")]
        [SerializeField] private ElementPuzzleView puzzleView;
        [SerializeField] private GridView gridView;

        private GridLayoutGroup _gridLayout;

        private List<ElementPuzzleView> _elementPuzzleViews = new List<ElementPuzzleView>();
        private List<GridView> _gridViews = new List<GridView>();

        private void Start()
        {
            SetupLevel();
        }

        public void SetupLevel()
        {
            var currentLevel = levelsConfig.Levels[3];
           
            if (currentLevel.CountElements <= 9)
            {
                SetGrid(true);
            }
            else
            {
                SetGrid(false);
            }

            SetupGridViews(currentLevel);
            SetupElemets(currentLevel);
        }

        public void SetupGridViews(LevelsConfig.Level currentLevel)
        {
            for (int i = 0; i < currentLevel.ElementsPuzzles.Count; i++)
            {
                var newGridView = Instantiate(gridView, _gridLayout.transform);
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
                var elementView = Instantiate(puzzleView, elementParents);
                Vector2 randomPosition = new Vector2(Random.Range(parentMin.x, parentMax.x),
                                                    Random.Range(parentMin.y, parentMax.y));


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