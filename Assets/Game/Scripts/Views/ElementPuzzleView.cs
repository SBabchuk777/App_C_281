using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;
using Screens;

namespace Views
{
    public class ElementPuzzleView : MonoBehaviour, IBeginDragHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler,IEndDragHandler
    {
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

        public void OnPointerUp(PointerEventData eventData)
        {

        }
        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isDrag)
            {
                _startPos = transform.position;
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1 * (Camera.main.transform.position.z));
                Vector3 mousePositionWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

                _mousePositionOffest = mousePositionWorldPoint - transform.position;
                transform.SetAsLastSibling();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDrag)
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1 * (Camera.main.transform.position.z));
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) - _mousePositionOffest;

                transform.position = objPosition;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDrag)
            {
                var gameScreen = UIManager.Instance.GetScreen<GameScreen>();
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
