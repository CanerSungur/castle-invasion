using UnityEngine;
//using GameAnalyticsSDK;
using ZestCore.MonoBehaviours;

namespace CastleInvasion
{
    public class GameAnalyticSDKManager : Singleton<GameAnalyticSDKManager>/*, IGameAnalyticsATTListener*/
    {
        #region Method 1

        //private void Awake()
        //{
        //    this.Reload();

        //    GameAnalytics.Initialize();
        //}

        #endregion

        #region Method 2

        //private void Awake()
        //{
        //    this.Reload();

        //    if (Application.platform == RuntimePlatform.IPhonePlayer)
        //    {
        //        GameAnalytics.RequestTrackingAuthorization(this);
        //    }
        //    else
        //    {
        //        GameAnalytics.Initialize();
        //    }
        //}

        //public void GameAnalyticsATTListenerNotDetermined()
        //{
        //    GameAnalytics.Initialize();
        //}
        //public void GameAnalyticsATTListenerRestricted()
        //{
        //    GameAnalytics.Initialize();
        //}
        //public void GameAnalyticsATTListenerDenied()
        //{
        //    GameAnalytics.Initialize();
        //}
        //public void GameAnalyticsATTListenerAuthorized()
        //{
        //    GameAnalytics.Initialize();
        //}

        #endregion
    }
}

