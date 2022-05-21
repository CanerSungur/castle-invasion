using UnityEngine;

namespace ZestGames
{
    public class DataManager : MonoBehaviour
    {
        public bool DeleteAllData = false;

        public static float TotalMoney { get; private set; }
        public static int StaminaLevel { get; private set; }
        public static int StaminaForCurrentLevel { get; private set; }
        public static int SizeLevel { get; private set; }
        public static int SizeForCurrentLevel { get; private set; }
        public static int IncomeLevel { get; private set; }
        public static float MoneyValue { get; private set; }
        public static float MaxStamina { get; private set; }
        public static float CurrentSize { get; private set; }
        public static int CurrentDamage => (int)(CurrentSize * _coreDamage);

        // \[ Price = BaseCost \times Multiplier ^{(\#\:Owned)} \]
        public static int StaminaCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, StaminaLevel));
        public static int IncomeCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, IncomeLevel));
        public static int SizeCost => (int)(_upgradeCost * Mathf.Pow(_upgradeCostIncreaseRate, SizeLevel));

        // Core data
        private readonly float _coreMoney = 3f;
        private readonly float _coreStamina = 100f;
        private readonly int _coreSize = 5;
        private static readonly int _coreDamage = 1;

        // Increase data
        private static readonly float _upgradeCost = 20;
        private static readonly float _upgradeCostIncreaseRate = 1.2f;
        private readonly float _moneyValueIncreaseRate = 1.5f;
        private readonly float _staminaIncreaseRate = 40f;
        private readonly int _sizeIncreaseRate = 1;

        [Header("-- STRUGGLE LIMIT --")]
        [SerializeField, Range(0f, 1f)] private float struggleRate = 0.5f;
        [SerializeField] private int pullStaminaCost = 10;
        public static int PullStaminaCost { get; private set; }
        public static int StruggleLimit { get; private set; }

        public void Init(GameManager gameManager)
        {
            LoadData();

            UpdateMoneyValue();
            UpdateStamina();
            UpdateSize();

            // Calculate StruggleLimit
            PullStaminaCost = pullStaminaCost;
            StruggleLimit = (int)(MaxStamina * struggleRate);

            UpgradeEvents.OnUpgradeIncome += IncomeUpgrade;
            UpgradeEvents.OnUpgradeStamina += StaminaUpgrade;
            UpgradeEvents.OnUpgradeSize += SizeUpgrade;

            CollectableEvents.OnCollect += IncreaseTotalMoney;
        }

        private void OnDisable()
        {
            UpgradeEvents.OnUpgradeIncome -= IncomeUpgrade;
            UpgradeEvents.OnUpgradeStamina += StaminaUpgrade;
            UpgradeEvents.OnUpgradeSize -= SizeUpgrade;

            CollectableEvents.OnCollect -= IncreaseTotalMoney;

            SaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void StaminaUpgrade()
        {
            IncreaseStaminaLevel();
            UpdateStamina();
        }

        private void IncomeUpgrade()
        {
            IncreaseIncomeLevel();
            UpdateMoneyValue();
        }

        private void SizeUpgrade()
        {
            IncreaseSizeLevel();
            UpdateSize();
        }

        private void UpdateMoneyValue()
        {
            MoneyValue = _coreMoney + _moneyValueIncreaseRate * (IncomeLevel - 1);
            //Debug.Log("Money Value: " + MoneyValue);
        }

        private void UpdateStamina()
        {
            MaxStamina = _coreStamina + _staminaIncreaseRate * (StaminaForCurrentLevel - 1);
            PlayerEvents.OnSetCurrentStamina?.Invoke();
            Debug.Log("Stamina For CurrentLevel: " + StaminaForCurrentLevel);
            Debug.Log("Max Stamina: " + MaxStamina);
        }

        private void UpdateSize()
        {
            CurrentSize = _coreSize + _sizeIncreaseRate * (SizeForCurrentLevel - 1);
            PlayerEvents.OnSetCurrentSize?.Invoke();
            Debug.Log("Size: " + CurrentSize);
        }

        private void IncreaseTotalMoney(float amount) => TotalMoney += amount;

        private void IncreaseStaminaLevel()
        {
            if (TotalMoney >= StaminaCost)
            {
                TotalMoney -= StaminaCost;
                StaminaLevel++;
                StaminaForCurrentLevel++;
                UiEvents.OnUpdateStaminaText?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(0); // ignore parameter
            }
        }

        private void IncreaseIncomeLevel()
        {
            if (TotalMoney >= IncomeCost)
            {
                TotalMoney -= IncomeCost;
                IncomeLevel++;
                UiEvents.OnUpdateIncomeText?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(0); // ignore parameter
            }
        }

        private void IncreaseSizeLevel()
        {
            if (TotalMoney >= SizeCost)
            {
                TotalMoney -= SizeCost;
                SizeLevel++;
                SizeForCurrentLevel++;
                UiEvents.OnUpdateSizeText?.Invoke();
                UiEvents.OnUpdateCollectableText?.Invoke(0); // ignore parameter
            }
        }

        private void LoadData()
        {
            if (PlayerPrefs.GetInt("ResetStaminaForNewLevel") == 1)
            {
                StaminaForCurrentLevel = 1;
                SizeForCurrentLevel = 1;

                PlayerPrefs.SetInt("ResetStaminaForNewLevel", 0);
                PlayerPrefs.Save();
            }
            else
            {
                StaminaForCurrentLevel = PlayerPrefs.GetInt("StaminaForCurrentLevel", 1);
                SizeForCurrentLevel = PlayerPrefs.GetInt("SizeForCurrentLevel", 1);
            }

            TotalMoney = PlayerPrefs.GetFloat("TotalMoney", 0);
            StaminaLevel = PlayerPrefs.GetInt("StaminaLevel", 1);
            IncomeLevel = PlayerPrefs.GetInt("IncomeLevel", 1);
            SizeLevel = PlayerPrefs.GetInt("SizeLevel", 1);
        }

        private void SaveData()
        {
            if (DeleteAllData)
            {
                PlayerPrefs.DeleteAll();
                return;
            }

            PlayerPrefs.SetFloat("TotalMoney", TotalMoney);
            PlayerPrefs.SetInt("StaminaLevel", StaminaLevel);
            PlayerPrefs.SetInt("IncomeLevel", IncomeLevel);
            PlayerPrefs.SetInt("SizeLevel", SizeLevel);
            PlayerPrefs.SetInt("StaminaForCurrentLevel", StaminaForCurrentLevel);
            PlayerPrefs.SetInt("SizeForCurrentLevel", SizeForCurrentLevel);

            PlayerPrefs.Save();
        }
    }
}
