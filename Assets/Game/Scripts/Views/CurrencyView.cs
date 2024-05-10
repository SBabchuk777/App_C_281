using UnityEngine;
using Saves;
using TMPro;

namespace Views
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;

        private void OnEnable()
        {
            UpdateCoinText();
        }

        public void UpdateCoinText()
        {
            coinText.text = GameSaves.Instance.GetCoin().ToString();
        }
    }
}