using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Datas;
using Views;
using UnityEngine.UIElements;

namespace Screens
{
    public class GameScreen : BaseScreen
    {
        public GridLayoutGroup GridLayout => _gridLayout;

        public List<GridView> GridViews => _gridViews;

        [Header("GridLayout")]
        [SerializeField] private GridLayoutGroup grid3x3;
        [SerializeField] private GridLayoutGroup grid4x4;

        [Header("Parents")]
        [SerializeField] private RectTransform elementParents;
        [SerializeField] private RectTransform wrongsParents;

        [Header("Configs")]
        [SerializeField] private LevelsConfig levelsConfig;

        [Header("Prefabs")]
        [SerializeField] private ElementPuzzleView puzzleView;
        [SerializeField] private GridView gridView;
        [SerializeField] private WrongView wrongView;

        private GridLayoutGroup _gridLayout;

        private List<ElementPuzzleView> _elementPuzzleViews = new List<ElementPuzzleView>();
        private List<GridView> _gridViews = new List<GridView>();
        private List<WrongView> _wrongViews = new List<WrongView>();

        private int _countWrongs = 4;
        private int _wrongSpendCount;

        private void Start()
        {
            SetupLevel();
        }

        public void SetupLevel()
        {
            var currentLevel = levelsConfig.Levels[0];
           
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
            SetupAvailableWrongs();
        }

        public void SpendAttemp()
        {
            if(_wrongSpendCount == 0)
            {
                Debug.LogError("You Lose");
                return;
            }

            //Debug.LogError("Count wrongs: " + _wrongSpendCount);

            _wrongViews[_wrongSpendCount-1].SetWrong(true);
            _wrongSpendCount--;
        }

        public GridView GetGridView(Vector3 position)
        {
            if (_gridViews.Count == 0)
                return null;

            GridView nearestCell = null;
            float minDistance = Mathf.Infinity;

            foreach (GridView cell in _gridViews)
            {
                float distance = Vector3.Distance(position, cell.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCell = cell;
                }
            }

            return nearestCell;
        }

        public void SetGrid(bool active)
        {
            grid3x3.gameObject.SetActive(active);
            grid4x4.gameObject.SetActive(!active);

            _gridLayout = active ? grid3x3.gameObject.activeSelf
                ? grid3x3 : grid4x4 : grid4x4.gameObject.activeSelf
                ? grid4x4 : grid3x3;

        }

        #region SetupViews

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

        public void SetupAvailableWrongs()
        {
            for (int i = 0; i < _countWrongs; i++)
            {
                var newWrongView = Instantiate(wrongView, wrongsParents);
                _wrongViews.Add(newWrongView);
            }

            _wrongSpendCount = _wrongViews.Count;
        }

        #endregion

        #region Clearing Lists

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

        public void ClearWrongsViews()
        {
            for (int i = 0; i < _wrongViews.Count; i++)
            {
                Destroy(_wrongViews[i].gameObject);
            }

            _wrongViews.Clear();
        }

        #endregion
    }
}