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
        private Vector3 mousePositionOffest;
        private Vector3 startPos;

        private Vector2 gridSize;
        private GridLayoutGroup gridLayoutGroup;

        private void OnEnable()
        {
            var gameScreen = UIManager.Instance.GetScreen<GameScreen>();

            if (gameScreen != null)
            {
                gridLayoutGroup = gameScreen.gridLayout;
                gridSize = gridLayoutGroup.cellSize;
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
            startPos = transform.position;
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1 * (Camera.main.transform.position.z));
            Vector3 mousePositionWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

            mousePositionOffest = mousePositionWorldPoint - transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1 * (Camera.main.transform.position.z));
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) - mousePositionOffest;

            transform.position = objPosition;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 currentPosition = transform.position;
            Vector3 nearestCellPosition = FindNearestCellPosition(currentPosition);

            float distance = Vector2.Distance(transform.position, nearestCellPosition);

            if (distance > 0.6)
            {
                transform.position = startPos;
            }
            else
            {
                transform.position = nearestCellPosition;
            }
        }

        private Vector3 FindNearestCellPosition(Vector3 position)
        {
            RectTransform[] cells = gridLayoutGroup.GetComponentsInChildren<RectTransform>();

            RectTransform nearestCell = null;
            float minDistance = Mathf.Infinity;

            foreach (RectTransform cell in cells)
            {
                float distance = Vector3.Distance(position, cell.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCell = cell;
                }
            }

            return nearestCell.position;
        }
    }
}
