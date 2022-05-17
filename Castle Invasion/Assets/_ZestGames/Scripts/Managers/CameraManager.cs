using UnityEngine;
using Cinemachine;
using System;

namespace ZestGames
{
    public class CameraManager : MonoBehaviour
    {
        [Header("-- CAMERA SETUP --")]
        [SerializeField] private CinemachineVirtualCamera gameStartCM;
        [SerializeField] private CinemachineVirtualCamera gameplayCM;
        private CinemachineTransposer _gameplayCMTransposer;

        [Header("-- SHAKE SETUP --")]
        private CinemachineBasicMultiChannelPerlin _gameplayCMBasicPerlin;
        private bool _shakeStarted = false;
        private float _shakeDuration = 1f;
        private float _shakeTimer;

        [Header("-- STRUGGLE SETUP --")]
        private bool _struggleStarted = false;

        public static Action OnShakeCam;

        private void Awake()
        {
            _gameplayCMTransposer = gameplayCM.GetCinemachineComponent<CinemachineTransposer>();
            _gameplayCMBasicPerlin = gameplayCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
            _shakeTimer = _shakeDuration;

            gameStartCM.Priority = 2;
            gameplayCM.Priority = 1;
        }

        private void Start()
        {
            GameEvents.OnGameStart += () => gameplayCM.Priority = 3;
            OnShakeCam += () => _shakeStarted = true;
            PlayerEvents.OnStartStruggle += () => _struggleStarted = true;
            PlayerEvents.OnStopStruggle += () => _struggleStarted = false;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= () => gameplayCM.Priority = 3;
            OnShakeCam -= () => _shakeStarted = true;
            PlayerEvents.OnStartStruggle -= () => _struggleStarted = true;
            PlayerEvents.OnStopStruggle -= () => _struggleStarted = false;
        }

        private void Update()
        {
            ShakeCamForAWhile();
            StruggleCam();
        }

        private void ShakeCamForAWhile()
        {
            if (_shakeStarted)
            {
                _gameplayCMBasicPerlin.m_AmplitudeGain = 1f;

                _shakeTimer -= Time.deltaTime;
                if (_shakeTimer <= 0f)
                {
                    _shakeStarted = false;
                    _shakeTimer = _shakeDuration;

                    _gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
                }
            }
        }

        private void StruggleCam()
        {
            if (_struggleStarted)
                _gameplayCMBasicPerlin.m_AmplitudeGain = 0.5f;
            else
                _gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
        }
    }
}
