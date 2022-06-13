using System;

namespace ZestGames
{
    public static class EventManager { }

    public static class GameEvents 
    {
        public static Action OnGameStart, OnLevelSuccess, OnLevelFail, OnChangePhase;
        public static Action<Enums.GameEnd> OnGameEnd, OnChangeScene;
    }

    public static class PlayerEvents 
    {
        public static Action OnStartStruggle, OnStopStruggle, OnRamPulled, OnRamReleased, OnHitDoor, OnSetCurrentStamina, OnSetCurrentSize;
        public static Action<int> OnDecreaseAiLimits;
    }

    public static class UiEvents
    {
        public static Action OnUpdateStaminaText, OnUpdateIncomeText, OnUpdateSizeText;
        public static Action<int> OnUpdateLevelText;
        public static Action<float> OnUpdateCollectableText;
        public static Action<string, FeedBackUi.Colors> OnGiveFeedBack;
    }

    public static class CollectableEvents
    {
        public static Action<float> OnCollect;
    }
    
    public static class InputEvents
    {
        public static Action OnTapHappened, OnTouchStarted, OnTouchStopped;
    }

    public static class UpgradeEvents
    {
        public static Action OnUpgradeIncome, OnUpgradeStamina, OnUpgradeSize;
    }

    public static class DoorEvents
    {
        public static Action OnResetDoor, OnGetHit, OnBreak, OnUpdateState;
    }

    public static class FeedbackEvents
    {
        public static Action<float> OnGiveMoneyFeedback;
    }

    public static class AudioEvents
    {
        public static Action OnPlayRamCreak, OnPlayDoorHit;
    }

    public static class GameAnalyticsEvent
    {
        public static Action<int> OnGameStart, OnLevelFail, OnLevelSuccess;
        public static Action<int> OnUpgradeStamina, OnUpgradeIncome, OnUpgradeSize;
    }
}
