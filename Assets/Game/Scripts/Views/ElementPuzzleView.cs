using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;
using Screens;
using System;
using Unity.VisualScripting;

namespace Views
{
    public class ElementPuzzleView : MonoBehaviour, IBeginDragHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler,IEndDragHandler
    {
        public event Action<ElementPuzzleView,bool> StateDragginAction;
        public int ElementIndex => _elementIndex;
        
        [SerializeField] private RectTransform rectElement;
        [SerializeField] private Image elementImage;
        [SerializeField] private float maxDistance;

        private Vector3 _mousePositionOffest;
        private Vector3 _startPos;

        private Vector2 _gridSize;
        private GridLayoutGroup _gridLayoutGroup;

        private int _elementIndex;

        private bool isDrag = true;
        private bool isBusy = false;

        public void SetupElement(Sprite sprite, int index,Vector2 vector2)
        {
            elementImage.sprite = sprite;
            _elementIndex = index;

            rectElement.anchoredPosition = vector2;
        }

        private void OnEnable()
        {
            var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

            if (gameScreen != null && gameScreen.GridLayout != null)
            {
                _gridLayoutGroup = gameScreen.GridLayout;
                _gridSize = _gridLayoutGroup.cellSize;

                rectElement.sizeDelta = _gridSize;
            }
        }

        private void OnDestroy()
        {
            UnSubcribe();
        }

        public void Subcribe()
        {
            GameScreen.OnChangeIndex += SetRaycastPuzzle;
        }

        private void UnSubcribe()
        {
            GameScreen.OnChangeIndex -= SetRaycastPuzzle;
        }

        private void SetRaycastPuzzle(int index)
        {
            isDrag = index == _elementIndex || index == -1;
            
            elementImage.raycastTarget = index == _elementIndex || index == -1;
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(CheckPauseScreen())
                return;
            
            if (isDrag && !isBusy)
            {
                var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

                if (gameScreen != null && gameScreen.Content.gameObject.activeSelf)
                {
                    gameScreen.SetId(_elementIndex);
                }
                
                _startPos = transform.position;
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1 * (Camera.main.transform.position.z));
                Vector3 mousePositionWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

                _mousePositionOffest = mousePositionWorldPoint - transform.position;
                transform.SetAsLastSibling();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CheckPauseScreen())
            {
                transform.position = _startPos;
                return;
            }
            
            if (isDrag && !isBusy)
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1 * (Camera.main.transform.position.z));
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) - _mousePositionOffest;

                transform.position = objPosition;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (CheckPauseScreen())
            {
                transform.position = _startPos;
                return;
            }
            
            if (isDrag && !isBusy)
            {
                var gameScreen = UIManager.Instance.GetScreen<GameScreen>();
                
                if(gameScreen !=null)
                     gameScreen.SetId(-1);
                
                Vector3 currentPosition = transform.position;

                GridView nearestCell = FindNearestCellPosition(currentPosition);

                float distance = Vector2.Distance(transform.position, nearestCell.transform.position);

                if (distance > maxDistance)
                {
                    AudioManager.Instance.ErrorPutCardSound();
                    isDrag = true;
                    transform.position = _startPos;
                }
                else
                {
                    if(_elementIndex == nearestCell.ElementPuzzleIndex)
                    {
                        AudioManager.Instance.PutCardSound();
                        transform.position = nearestCell.transform.position;
                        isDrag = false;
                        isBusy = true;
                        
                        nearestCell.SetBusy();
                        
                        if (gameScreen != null)
                        {
                            gameScreen.AddElementForGrid(this);
                        }
                    }
                    else if(!nearestCell.IsBusy) 
                    {
                        isDrag = true;
                        transform.position = _startPos;
                        AudioManager.Instance.ErrorPutCardSound();

                        if (gameScreen != null)
                        {
                            gameScreen.SpendAttemp();
                        }
                    }
                    else
                    {
                        isDrag = true;
                        transform.position = _startPos;
                        AudioManager.Instance.ErrorPutCardSound();
                    }
                   
                }
            }
        }

        private bool CheckPauseScreen()
        {
            var pauseScreen = UIManager.Instance.GetScreen<PauseScreen>();

            if (pauseScreen != null && pauseScreen.Content.gameObject.activeSelf)
                return true;

            return false;
        }

        private GridView FindNearestCellPosition(Vector3 position)
        {
            var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

            if (gameScreen != null)
            {
                return gameScreen.GetGridView(position);
            }

            return null;
        }
    }
}
