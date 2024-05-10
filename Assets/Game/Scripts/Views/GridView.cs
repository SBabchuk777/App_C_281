using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class GridView : MonoBehaviour
    {
        public int ElementPuzzleIndex => _elementPuzzleIndex;

        [SerializeField] private Image gridImage;

        private int _elementPuzzleIndex;

        public void SetupIndex(int index)
        {
            _elementPuzzleIndex = index;
        }
    }
}
