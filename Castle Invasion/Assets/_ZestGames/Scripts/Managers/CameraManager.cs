using UnityEngine;
using Cinemachine;
using System;
using CastleInvasion;

namespace ZestGames
{
    public class CameraManager : MonoBehaviour
    {
        private BatteringRam _batteringRam;

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
        private float _struggleShakeAmplitude = 0.1f;
        private float _struggleShakeFrequencyRate, _currentStruggleShakeFrequency;
        private readonly float _minStruggleShakeFrequency = 0f;
        private readonly float _maxStruggleShakeFrequency = 50f;

        public static Action OnShakeCam;

        private void Awake()
        {
            _batteringRam = FindObjectOfType<BatteringRam>();

            _gameplayCMTransposer = gameplayCM.GetCinemachineComponent<CinemachineTransposer>();
            _gameplayCMBasicPerlin = gameplayCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
            _shakeTimer = _shakeDuration;

            gameStartCM.Priority = 2;
            gameplayCM.Priority = 1;

            _currentStruggleShakeFrequency = _minStruggleShakeFrequency;
            _struggleShakeFrequencyRate =  (_maxStruggleShakeFrequency - _minStruggleShakeFrequency) / _batteringRam.StrugglePullCount;
        }

        private void Start()
        {
            GameEvents.OnGameStart += () => gameplayCM.Priority = 3;
            OnShakeCam += () => _shakeStarted = true;
            PlayerEvents.OnStartStruggle += StartStruggle;
            PlayerEvents.OnStopStruggle += StopStruggle;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= () => gameplayCM.Priority = 3;
            OnShakeCam -= () => _shakeStarted = true;
            PlayerEvents.OnStartStruggle -= StartStruggle;
            PlayerEvents.OnStopStruggle -= StopStruggle;
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
            {
                _gameplayCMBasicPerlin.m_AmplitudeGain = _struggleShakeAmplitude;
                _gameplayCMBasicPerlin.m_FrequencyGain = _currentStruggleShakeFrequency;
            }
        }

        private void StartStruggle()
        {
            if (!_struggleStarted)
            {
                _struggleStarted = true;
            }

            _currentStruggleShakeFrequency += _struggleShakeFrequencyRate;
        }

        private void StopStruggle()
        {
            if (_struggleStarted)
                _struggleStarted = false;

            _currentStruggleShakeFrequency = _minStruggleShakeFrequency;
            _gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
            _gameplayCMBasicPerlin.m_FrequencyGain = 1f;
        }
    }
}
