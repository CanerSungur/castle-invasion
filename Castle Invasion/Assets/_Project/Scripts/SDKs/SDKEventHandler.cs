using GameAnalyticsSDK;
using System;
using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class SDKEventHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            GameAnalyticsEvent.OnGameStart += HandleGameStart;
            GameAnalyticsEvent.OnLevelFail += HandleLevelFail;
            GameAnalyticsEvent.OnLevelSuccess += HandleLevelSuccess;

            GameAnalyticsEvent.OnUpgradeStamina += HandleStaminaUpgrade;
            GameAnalyticsEvent.OnUpgradeIncome += HandleIncomeUpgrade;
            GameAnalyticsEvent.OnUpgradeSize += HandleSizeUpgrade;
        }

        private void OnDisable()
        {
            GameAnalyticsEvent.OnGameStart -= HandleGameStart;
            GameAnalyticsEvent.OnLevelFail -= HandleLevelFail;
            GameAnalyticsEvent.OnLevelSuccess -= HandleLevelSuccess;

            GameAnalyticsEvent.OnUpgradeStamina -= HandleStaminaUpgrade;
            GameAnalyticsEvent.OnUpgradeIncome -= HandleIncomeUpgrade;
            GameAnalyticsEvent.OnUpgradeSize -= HandleSizeUpgrade;
        }

        private void HandleGameStart()
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LevelProgress");
        }

        private void HandleLevelFail()
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "LevelProgress");
        }

        private void HandleLevelSuccess()
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LevelProgress");
        }

        private void HandleStaminaUpgrade(int upgradeLevel)
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Stamina", upgradeLevel, "Upgrade", "Upgrade");
        }

        private void HandleIncomeUpgrade(int upgradeLevel)
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Income", upgradeLevel, "Upgrade", "Upgrade");
        }

        private void HandleSizeUpgrade(int upgradeLevel)
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Size", upgradeLevel, "Upgrade", "Upgrade");
        }
    }
}
