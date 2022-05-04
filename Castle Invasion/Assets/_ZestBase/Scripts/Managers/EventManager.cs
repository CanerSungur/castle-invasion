using System;

namespace ZestGames
{
    public static class EventManager { }
    public static class GameEvents
    {
        public static Action OnGameStart, OnLevelSuccess, OnLevelFail;
        public static Action<Enums.GameEnd> OnGameEnd, OnChangeScene;
    }

    public static class PlayerEvents { }
    public static class UiEvents 
    {
        public static Action<int> OnUpdateLevelText, OnUpdateMoneyText, OnUpdateCoinText;
    }
    public static class CollectableEvents
    {
        public static Action<int> OnCollect;
    }
}
