using UnityEngine;
using TMPro;
using ZestCore.Utility;
using ZestGames;

namespace CastleInvasion
{
    public class UpgradeCanvas : MonoBehaviour
    {
        [Header("-- STAMINA SETUP --")]
        [SerializeField] private TextMeshProUGUI staminaLevelText;
        [SerializeField] private TextMeshProUGUI staminaCostText;
        [SerializeField] private CustomButton staminaUpgradeButton;

        [Header("-- INCOME SETUP --")]
        [SerializeField] private TextMeshProUGUI incomeLevelText;
        [SerializeField] private TextMeshProUGUI incomeCostText;
        [SerializeField] private CustomButton incomeUpgradeButton;

        [Header("-- SPEED SETUP --")]
        [SerializeField] private TextMeshProUGUI sizeLevelText;
        [SerializeField] private TextMeshProUGUI sizeCostText;
        [SerializeField] private CustomButton sizeUpgradeButton;

        private void OnEnable()
        {
            Delayer.DoActionAfterDelay(this, 0.5f, UpdateTexts);

            staminaUpgradeButton.onClick.AddListener(StaminaUpgradeClicked);
            incomeUpgradeButton.onClick.AddListener(IncomeUpgradeClicked);
            sizeUpgradeButton.onClick.AddListener(SpeedUpgradeClicked);

            UiEvents.OnUpdateStaminaText += UpdateTexts;
            UiEvents.OnUpdateIncomeText += UpdateTexts;
            UiEvents.OnUpdateSizeText += UpdateTexts;
        }

        private void OnDisable()
        {
            staminaUpgradeButton.onClick.RemoveListener(StaminaUpgradeClicked);
            incomeUpgradeButton.onClick.RemoveListener(IncomeUpgradeClicked);
            sizeUpgradeButton.onClick.RemoveListener(SpeedUpgradeClicked);

            UiEvents.OnUpdateStaminaText -= UpdateTexts;
            UiEvents.OnUpdateIncomeText -= UpdateTexts;
            UiEvents.OnUpdateSizeText -= UpdateTexts;
        }

        private void UpdateTexts()
        {
            staminaLevelText.text = $"Level {DataManager.StaminaLevel}";
            staminaCostText.text = DataManager.StaminaCost.ToString();

            incomeLevelText.text = $"Level {DataManager.IncomeLevel}";
            incomeCostText.text = DataManager.IncomeCost.ToString();

            sizeLevelText.text = $"Level {DataManager.SizeLevel}";
            sizeCostText.text = DataManager.SizeCost.ToString();

            CheckForMoneySufficiency();
        }

        private void UpgradeStamina() => UpgradeEvents.OnUpgradeStamina?.Invoke();
        private void UpgradeIncome() => UpgradeEvents.OnUpgradeIncome?.Invoke();
        private void UpgradeSpeed() => UpgradeEvents.OnUpgradeSize?.Invoke();
        private void StaminaUpgradeClicked() => staminaUpgradeButton.TriggerClick(UpgradeStamina);
        private void IncomeUpgradeClicked() => incomeUpgradeButton.TriggerClick(UpgradeIncome);
        private void SpeedUpgradeClicked() => sizeUpgradeButton.TriggerClick(UpgradeSpeed);
        private void CheckForMoneySufficiency()
        {
            staminaUpgradeButton.interactable = DataManager.TotalMoney >= DataManager.StaminaCost;
            incomeUpgradeButton.interactable = DataManager.TotalMoney >= DataManager.IncomeCost;
            sizeUpgradeButton.interactable = DataManager.TotalMoney >= DataManager.SizeCost;
        }
    }
}
