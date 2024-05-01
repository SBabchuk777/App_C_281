using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views
{
    public class ElementPuzzleView : MonoBehaviour, IBeginDragHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler,IEndDragHandler
    {
        private Vector3 mousePositionOffest;
        private Vector3 startPos;


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
            Debug.LogError("Drag");

        }
        public void OnEndDrag(PointerEventData eventData)
        {
            float x = Mathf.Round(transform.position.x);
            float y = Mathf.Round(transform.position.y);
            Vector3 pos = new Vector3(x, y, 0);
            transform.position = pos;
            Debug.LogError("End Drag");
        }
    }
}
