using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;
using Screens;

namespace Views
{
    public class ElementPuzzleView : MonoBehaviour, IBeginDragHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler,IEndDragHandler
    {
        [SerializeField] private RectTransform rectElement;
        [SerializeField] private Image elementImage;

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

            if (gameScreen != null)
            {
                _gridLayoutGroup = gameScreen.GridLayout;
                _gridSize = _gridLayoutGroup.cellSize;

                rectElement.sizeDelta = _gridSize; //- new Vector2(2.5f,2.5f);
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
                Vector3 currentPosition = transform.position;

                GridView nearestCell = FindNearestCellPosition(currentPosition);

                float distance = Vector2.Distance(transform.position, nearestCell.transform.position);

                if (distance > 0.6)
                {
                    isDrag = true;
                    transform.position = _startPos;
                }
                else
                {
                    if(_elementIndex == nearestCell.ElementPuzzleIndex)
                    {
                        transform.position = nearestCell.transform.position;
                        isDrag = false;
                    }
                    else
                    {
                        // TODO: для ресета позиции и траты ошибки...

                        isDrag = true;
                        transform.position = _startPos;

                        var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

                        if(gameScreen != null)
                        {
                            gameScreen.SpendAttemp();
                        }
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
