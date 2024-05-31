using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class GridView : MonoBehaviour
    {
        public bool IsBusy => isBusy;
        public int ElementPuzzleIndex => _elementPuzzleIndex;

        [SerializeField] private Image gridImage;

        private int _elementPuzzleIndex;
        private bool isBusy = false;

        public void SetupIndex(int index)
        {
            _elementPuzzleIndex = index;
            isBusy = false;
        }

        public void SetBusy()
        {
            isBusy = true;
        }
    }
}
