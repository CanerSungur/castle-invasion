using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

namespace ZestGames
{
    public class CameraManager : MonoBehaviour
    {
        [Header("-- CAMERA SETUP --")]
        [SerializeField] private CinemachineVirtualCamera gameStartCM;
        [SerializeField] private CinemachineVirtualCamera gameplayCM;
        private CinemachineTransposer _gameplayCMTransposer;
        [SerializeField] private CinemachineVirtualCamera dollyCam;
        private CinemachineTrackedDolly _dollyCamTrackedDolly;
        private readonly float _dollyTrackTime = 3f;

        [Header("-- SHAKE SETUP --")]
        private CinemachineBasicMultiChannelPerlin _gameplayCMBasicPerlin;
        private bool _shakeStarted = false;
        private float _shakeDuration = 1f;
        private float _shakeTimer;

        [Header("-- STRUGGLE SETUP --")]
        private bool _struggleStarted = false;
        private float _struggleShakeAmplitude = 0.25f;
        private float _struggleShakeFrequencyRate, _currentStruggleShakeFrequency;
        private readonly float _minStruggleShakeFrequency = 0f;
        private readonly float _maxStruggleShakeFrequency = 100f;

        // Camera position setup
        private readonly float _defaultCamDistance = -12f;
        private readonly float _defaultCamHeight = 6f;
        private readonly float _camDistanceIncreaseRate = -1.3f;
        private readonly float _camHeightIncreaseRate = 0.15f;
        private float _currentCamDistance, _currentCamHeight;

        public static Action OnShakeCam;

        public void Init(GameManager gameManager)
        {
            _gameplayCMTransposer = gameplayCM.GetCinemachineComponent<CinemachineTransposer>();
            _gameplayCMBasicPerlin = gameplayCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _dollyCamTrackedDolly = dollyCam.GetCinemachineComponent<CinemachineTrackedDolly>();
            _currentCamDistance = _defaultCamDistance;
            _currentCamHeight = _defaultCamHeight;

            // cam distance
            UpdateCameraPosition();

            // shake
            _gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
            _shakeTimer = _shakeDuration;

            dollyCam.Priority = 2;
            gameplayCM.Priority = 1;

            // Dolly Cam Setup
            _dollyCamTrackedDolly.m_PathPosition = 5f;
            dollyCam.transform.rotation = Quaternion.Euler(30f, 0f, 0f);
            ZestCore.Utility.Delayer.DoActionAfterDelay(this, 0.75f, PlayDollyCam);

            _currentStruggleShakeFrequency = _minStruggleShakeFrequency;
            _struggleShakeFrequencyRate = 5;

            GameEvents.OnGameStart += () => gameplayCM.Priority = 3;
            OnShakeCam += () => _shakeStarted = true;
            PlayerEvents.OnStartStruggle += StartStruggle;
            PlayerEvents.OnStopStruggle += StopStruggle;
            UpgradeEvents.OnUpgradeSize += UpdateCameraPosition;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= () => gameplayCM.Priority = 3;
            OnShakeCam -= () => _shakeStarted = true;
            PlayerEvents.OnStartStruggle -= StartStruggle;
            PlayerEvents.OnStopStruggle -= StopStruggle;
            UpgradeEvents.OnUpgradeSize -= UpdateCameraPosition;
        }

        private void Update()
        {
            ShakeCamForAWhile();
            StruggleCam();
        }

        private void UpdateCameraPosition()
        {
            // Update camera according to ram size
            float _currentDist = _currentCamDistance;
            float _currentHeight = _currentCamHeight;

            _currentCamDistance = _defaultCamDistance + ((DataManager.SizeForCurrentLevel - 1) * _camDistanceIncreaseRate);
            _currentCamHeight = _defaultCamHeight + ((DataManager.SizeForCurrentLevel - 1) * _camHeightIncreaseRate);

            DOVirtual.Float(_currentDist, _currentCamDistance, 1f, r => {
                _gameplayCMTransposer.m_FollowOffset = new Vector3(_gameplayCMTransposer.m_FollowOffset.x, _gameplayCMTransposer.m_FollowOffset.y, r);
                _currentCamDistance = r;
            });
            DOVirtual.Float(_currentHeight, _currentCamHeight, 1f, r => {
                _gameplayCMTransposer.m_FollowOffset = new Vector3(_gameplayCMTransposer.m_FollowOffset.x, r, _gameplayCMTransposer.m_FollowOffset.z);
                _currentCamHeight = r;
            });
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

        private void PlayDollyCam()
        {
            //_dollyCamTrackedDolly.m_PathPosition = 3;
            DOVirtual.Float(5, 0, _dollyTrackTime, r => {
                _dollyCamTrackedDolly.m_PathPosition = r;
            });
            DOVirtual.Float(30, 0, _dollyTrackTime * 0.75f, r => {
                dollyCam.transform.rotation = Quaternion.Euler(r, 0f, 0f);
            }).OnComplete(() => {
                DOVirtual.Float(0, 30, _dollyTrackTime * 0.25f, r => {
                    dollyCam.transform.rotation = Quaternion.Euler(r, 0f, 0f);
                });
            });
        }
    }
}
