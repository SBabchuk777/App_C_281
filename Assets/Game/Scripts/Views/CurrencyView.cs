using System.Globalization;
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
            int coin = GameSaves.Instance.GetCoin();
            coinText.text = FormatNumberWithSpaces(coin);
        }

        private string FormatNumberWithSpaces(int number)
        {
            // Используем культуру инвариантной культуры для стандартного формата
            string formattedNumber = number.ToString("N0", CultureInfo.InvariantCulture);
        
            // Заменяем запятые на пробелы
            formattedNumber = formattedNumber.Replace(',', ' ');
        
            return formattedNumber;
        }
    }
}