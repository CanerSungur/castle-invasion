using UnityEngine;
using DG.Tweening;

namespace ZestGames
{
    public class GameManager : MonoBehaviour
    {
        public static Enums.GameState GameState { get; private set; }
        public static Enums.GameEnd GameEnd { get; private set; }

        [Header("-- REFERENCES --")]
        private UiManager _uiManager;
        private LevelManager _levelManager;
        private SettingsManager _settingsManager;
        private DataManager _dataManager;
        private CameraManager _cameraManager;

        private void Init()
        {
            Application.targetFrameRate = 240;
            //DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1250,5);

            GameState = Enums.GameState.WaitingToStart;
            GameEnd = Enums.GameEnd.None;

            _levelManager = GetComponent<LevelManager>();
            _levelManager.Init(this);
            _dataManager = GetComponent<DataManager>();
            _dataManager.Init(this);
            _settingsManager = GetComponent<SettingsManager>();
            _settingsManager.Init(this);
            _uiManager = GetComponent<UiManager>();
            _uiManager.Init(this);
            _cameraManager = GetComponent<CameraManager>();
            _cameraManager.Init(this);

            UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            UiEvents.OnUpdateLevelText?.Invoke(LevelHandler.Level);
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnGameEnd += HandleGameEnd;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnGameEnd -= HandleGameEnd;

            DOTween.KillAll();
        }

        private void HandleGameStart()
        {
            GameState = Enums.GameState.Started;
        }

        private void HandleGameEnd(Enums.GameEnd gameEnd)
        {
            GameState = Enums.GameState.GameEnded;
            GameEnd = gameEnd;

            if (GameEnd == Enums.GameEnd.Success)
            {
                GameEvents.OnLevelSuccess?.Invoke();
                GameAnalyticsEvent.OnLevelSuccess?.Invoke();
            }
            else if (GameEnd == Enums.GameEnd.Fail)
            {
                GameEvents.OnLevelFail?.Invoke();
                GameAnalyticsEvent.OnLevelFail?.Invoke();
            }
        }
    }
}
