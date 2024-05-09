using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class WrongView : MonoBehaviour
    {
        [SerializeField] private Image activeImage;
        [SerializeField] private Image notActiveImage;

        public void SetWrong(bool active)
        {
            activeImage.gameObject.SetActive(!active);
            notActiveImage.gameObject.SetActive(active);
        }
    }
}