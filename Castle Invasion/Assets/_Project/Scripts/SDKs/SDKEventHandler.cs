//using GameAnalyticsSDK;
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

            //GameAnalyticsEvent.OnUpgradeStamina += HandleStaminaUpgrade;
            //GameAnalyticsEvent.OnUpgradeIncome += HandleIncomeUpgrade;
            //GameAnalyticsEvent.OnUpgradeSize += HandleSizeUpgrade;
        }

        private void OnDisable()
        {
            GameAnalyticsEvent.OnGameStart -= HandleGameStart;
            GameAnalyticsEvent.OnLevelFail -= HandleLevelFail;
            GameAnalyticsEvent.OnLevelSuccess -= HandleLevelSuccess;

            //GameAnalyticsEvent.OnUpgradeStamina -= HandleStaminaUpgrade;
            //GameAnalyticsEvent.OnUpgradeIncome -= HandleIncomeUpgrade;
            //GameAnalyticsEvent.OnUpgradeSize -= HandleSizeUpgrade;
        }

        private void HandleGameStart(int level)
        {
            // TODO: Level starts here.



            #region Game Analytics Event
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LevelProgress");
            #endregion
        }

        private void HandleLevelFail(int level)
        {
            // TODO: Level fails here.



            #region Game Analytics Event
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "LevelProgress");

            #endregion
        }

        private void HandleLevelSuccess(int level)
        {
            // TODO: Level successes here.



            #region Game Analytics Event
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LevelProgress");
            #endregion
        }

        #region Game Analytics Event Functions
        private void HandleStaminaUpgrade(int upgradeLevel)
        {
            //GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Stamina", upgradeLevel, "Upgrade", "Upgrade");
        }

        private void HandleIncomeUpgrade(int upgradeLevel)
        {
            //GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Income", upgradeLevel, "Upgrade", "Upgrade");
        }

        private void HandleSizeUpgrade(int upgradeLevel)
        {
            //GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Size", upgradeLevel, "Upgrade", "Upgrade");
        }
        #endregion
    }
}
