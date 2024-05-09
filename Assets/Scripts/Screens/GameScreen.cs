using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Datas;
using Views;
using Managers;
using Saves;
using TMPro;
using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;

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

        [Header("UI")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private TMP_Text levelText;

        [Header("Tutorial")]
        [SerializeField] private Image backTutorial;
        [SerializeField] private Image handTutorial;
        [SerializeField] private Image blockClickBackground;
        [SerializeField] private Vector3 offestHands;
        [SerializeField] private float animationTime;
        [SerializeField] private float fadeTime;

        private GridLayoutGroup _gridLayout;

        private List<ElementPuzzleView> _elementPuzzleViews = new List<ElementPuzzleView>();
        private List<ElementPuzzleView> _elementPuzzleViewsGrids = new List<ElementPuzzleView>();
        private List<GridView> _gridViews = new List<GridView>();
        private List<WrongView> _wrongViews = new List<WrongView>();

        private int _countWrongs = 4;
        private int _wrongSpendCount;
        private int _rewardCoin = 100;

        private void Awake()
        {
            pauseButton.onClick.AddListener(OpenPauseScreen);
        }

        private void Start()
        {
            RestartScreen.RestartGameAction += OnRestartGame;
            ExitScreen.ExitGameAction += OnExitGame;
            WinOrLosePopup.NextLevelAction += OnNextLevel;
            WinOrLosePopup.OpenStartScreenAction += OnExitGame;
        }

        private void OnDestroy()
        {
            RestartScreen.RestartGameAction -= OnRestartGame;
            ExitScreen.ExitGameAction -= OnExitGame;
            WinOrLosePopup.NextLevelAction -= OnNextLevel;
            WinOrLosePopup.OpenStartScreenAction += OnExitGame;
        }

        public void SetupLevel()
        {
            GameSaves.Instance.SetLevel();

            UpdateLevelText();
            var currentLevel = levelsConfig.Levels[GameSaves.Instance.GetLevel()];

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

            ShowTutorial();
        }

        public async void ShowTutorial()
        {
            if (GameSaves.Instance.ShowTutorial())
            {
                ActiveTutorialObjects(true);
                await Task.Delay(500);

                var firstCard = _elementPuzzleViews.FirstOrDefault();

                var cardPosition = firstCard.transform.position;

                var cardTarget = _gridViews.FirstOrDefault(item => item.ElementPuzzleIndex == firstCard.ElementIndex);

                handTutorial.transform.DOMove(firstCard.transform.position + offestHands, 0);
                
                handTutorial.DOFade(1, fadeTime).OnComplete(() =>
                {
                    firstCard.transform.SetAsLastSibling();
                    handTutorial.transform.DOMove(cardTarget.transform.position + offestHands, animationTime);
                    firstCard.transform.DOMove(cardTarget.transform.position, animationTime).OnComplete(() =>
                    {
                        CallbackTutorial(firstCard, cardPosition);
                    });
                });         
                
            }
        }

        public async void CallbackTutorial(ElementPuzzleView elementPuzzleView, Vector3 position)
        {
            await Task.Delay(300);

            elementPuzzleView.transform.position = position;

            backTutorial.DOFade(0,fadeTime);
            handTutorial.DOFade(0, fadeTime).OnComplete(() =>
            {
                ActiveTutorialObjects(false);
            });
        }

        public void ActiveTutorialObjects(bool active)
        {
            backTutorial.gameObject.SetActive(active);
            handTutorial.gameObject.SetActive(active);
            blockClickBackground.gameObject.SetActive(active);
        }

        public void SpendAttemp()
        {
            _wrongViews[_wrongSpendCount - 1].SetWrong(true);

            if (_wrongSpendCount == 1)
            {
                var winScreen = UIManager.Instance.GetScreen<WinOrLosePopup>();

                if (winScreen != null)
                {
                    winScreen.SetupPanel(false);
                    winScreen.OpenScreen();
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