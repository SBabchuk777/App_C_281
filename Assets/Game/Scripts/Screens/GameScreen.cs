using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Datas;
using Views;
using Managers;
using Saves;
using TMPro;
using System;

namespace Screens
{
    public class GameScreen : BaseScreen
    {
        public static event Action<int> OnChangeIndex;
        
        public GridLayoutGroup GridLayout => _gridLayout;

        public List<GridView> GridViews => _gridViews;

        [Header("GridLayout")]
        [SerializeField] private GridLayoutGroup grid3x3;
        [SerializeField] private GridLayoutGroup grid4x4;

        [Header("Parents")]
        [SerializeField] private RectTransform elementParents;
        [SerializeField] private RectTransform wrongsParents;
        [SerializeField] private RectTransform allElementsParent;

        [Header("UI")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private TMP_Text levelText;

        private GridLayoutGroup _gridLayout;

        private List<ElementPuzzleView> _elementPuzzleViews = new List<ElementPuzzleView>();
        private List<ElementPuzzleView> _elementPuzzleViewsGrids = new List<ElementPuzzleView>();
        private List<GridView> _gridViews = new List<GridView>();
        private List<WrongView> _wrongViews = new List<WrongView>();

        private const int _countWrongs = 4;
        private int _wrongSpendCount;

        private ElementPuzzleView dragginElement;
        
        [SerializeField] private int _rewardCoin = 100;

        private void Awake()
        {
            pauseButton.onClick.AddListener(OpenPauseScreen);
        }

        private void Start()
        {
            LevelScreen.OnUpdateLevel += UpdateLevelText;
            
            RestartScreen.RestartGameAction += OnRestartGame;
            ExitScreen.ExitGameAction += OnExitGame;
            WinOrLosePopup.NextLevelAction += OnNextLevel;
            WinOrLosePopup.OpenStartScreenAction += OnExitGame;
            TutorialScreen.CloseTutorial += ActivityAllElements;
        }

        private void OnDestroy()
        {
            LevelScreen.OnUpdateLevel -= UpdateLevelText;
            
            RestartScreen.RestartGameAction -= OnRestartGame;
            ExitScreen.ExitGameAction -= OnExitGame;
            WinOrLosePopup.NextLevelAction -= OnNextLevel;
            WinOrLosePopup.OpenStartScreenAction += OnExitGame;
            TutorialScreen.CloseTutorial -= ActivityAllElements;
        }

        public void SetupLevel()
        {
            GameSaves.Instance.SetLevel();

            UpdateLevelText();
            var currentLevel = PrefabsStorage.Instance.LevelsConfig.Levels[GameSaves.Instance.GetLevel()];

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
            _wrongViews[_wrongSpendCount - 1].SetWrong(true);

            if (_wrongSpendCount == 1)
            {
                var loseScreen = UIManager.Instance.GetScreen<WinOrLosePopup>();

                if (loseScreen != null)
                {
                    AudioManager.Instance.LoseGameSound();
                    loseScreen.SetupPanel(false);
                    loseScreen.OpenScreen();
                }

                return;
            }
           
            _wrongSpendCount--;
        }

        public void AddElementForGrid(ElementPuzzleView elementPuzzleView)
        {
            _elementPuzzleViews.Remove(elementPuzzleView);
            _elementPuzzleViewsGrids.Add(elementPuzzleView);

            if(_elementPuzzleViews.Count == 0)
            {
                GameSaves.Instance.SaveLevelIndex();
                GameSaves.Instance.AddStarCoin(_rewardCoin);

                var winScreen =  UIManager.Instance.GetScreen<WinOrLosePopup>();

                if (winScreen != null)
                {
                    AudioManager.Instance.WinGameSound();
                    winScreen.SetupPanel(true);
                    winScreen.OpenScreen();
                }
            }
        }

        public GridView GetGridView(Vector3 position)
        {
            if (_gridViews.Count == 0)
                return null;

            GridView nearestCell = null;
            float minDistance = 5;//Mathf.Infinity;

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

        public void ActivityAllElements(bool active)
        {
            allElementsParent.gameObject.SetActive(active);
        }

        public void SetGrid(bool active)
        {
            grid3x3.gameObject.SetActive(active);
            grid4x4.gameObject.SetActive(!active);

            _gridLayout = active ? grid3x3.gameObject.activeSelf
                ? grid3x3 : grid4x4 : grid4x4.gameObject.activeSelf
                ? grid4x4 : grid3x3;

        }

        private void OnNextLevel()
        {
            AllClearListes();
            UIManager.Instance.OpenScreen<LevelScreen>();
        }

        private void OnRestartGame()
        {
            AllClearListes();
            UIManager.Instance.OpenScreen<LevelScreen>();
        }

        private void OnExitGame()
        {
            AllClearListes();
            CloseScreen();
            CloseScreen();
        }

        private void OpenPauseScreen()
        {
            UIManager.Instance.OpenScreen<PauseScreen>();
        }

        private void UpdateLevelText()
        {
            levelText.text = "Level " + (GameSaves.Instance.GetLevel() + 1).ToString();
        }

        public override void OpenScreen()
        {
            AudioManager.Instance.BackgroundMusicPlay(false);
            AudioManager.Instance.GameMusicPlay(true);
            base.OpenScreen();
        }

        public void SetId(int id)
        {
            OnChangeIndex?.Invoke(id);
        }

        #region SetupViews

        public void SetupGridViews(LevelsConfig.Level currentLevel)
        {
            for (int i = 0; i < currentLevel.ElementsPuzzles.Count; i++)
            {
                var newGridView = Instantiate(PrefabsStorage.Instance.GridView, _gridLayout.transform);
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


                elementView.Subcribe();
                elementView.SetupElement(currentLevel.ElementsPuzzles[i].ElementSprite, currentLevel.ElementsPuzzles[i].ElementIndex, randomPosition);
                _elementPuzzleViews.Add(elementView);
            }
        }

        public void SetupAvailableWrongs()
        {
            for (int i = 0; i < _countWrongs; i++)
            {
                var newWrongView = Instantiate(PrefabsStorage.Instance.WrongView, wrongsParents);
                _wrongViews.Add(newWrongView);
            }

            _wrongSpendCount = _wrongViews.Count;
        }

        #endregion

        #region Clearing Lists

        public void AllClearListes()
        {
            ClearElements();
            ClearGrids();
            ClearWrongsViews();
        }

        public void ClearElements()
        {
            for (int i = 0; i < _elementPuzzleViews.Count; i++)
            {
                Destroy(_elementPuzzleViews[i].gameObject);
            }

            _elementPuzzleViews.Clear();

            for (int i = 0; i < _elementPuzzleViewsGrids.Count; i++)
            {
                Destroy(_elementPuzzleViewsGrids[i].gameObject);
            }

            _elementPuzzleViewsGrids.Clear();
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