using UnityEngine;
using TMPro;

namespace ZestGames
{
    public class Hud : MonoBehaviour
    {
        [Header("-- TEXT --")]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI coinText;

        public static Transform CollectableHUDTransform { get; private set; }

        public void Init(UiManager uiManager)
        {
            UiEvents.OnUpdateLevelText += UpdateLevelText;
            UiEvents.OnUpdateMoneyText += UpdateMoneyText;
            UiEvents.OnUpdateCoinText += UpdateCoinText;

            CollectableHUDTransform = moneyText.transform.parent;
        }

        private void OnDisable()
        {
            UiEvents.OnUpdateLevelText -= UpdateLevelText;
            UiEvents.OnUpdateMoneyText -= UpdateMoneyText;
            UiEvents.OnUpdateCoinText -= UpdateCoinText;
        }

        private void UpdateLevelText(int level) => levelText.text = $"Level {level}";
        private void UpdateMoneyText(int money) => moneyText.text = money.ToString();
        private void UpdateCoinText(int coin) => coinText.text = coin.ToString();
    }
}
