using UnityEngine;
using TMPro;

namespace CastleInvasion
{
    public class MoneyFeedback : MonoBehaviour
    {
        private TextMeshProUGUI _moneyText;

        private void OnEnable()
        {
            if (!_moneyText)
                _moneyText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

            //_moneyText.text = DataManager.MoneyValue.ToString("#0.0");
        }

        public void SetMoneyText(float lastEarnedMoney) => _moneyText.text = lastEarnedMoney.ToString("#0.0");
    }
}
