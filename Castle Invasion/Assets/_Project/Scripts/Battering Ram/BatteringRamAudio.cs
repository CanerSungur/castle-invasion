using System;
using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class BatteringRamAudio : MonoBehaviour
    {
        private BatteringRam _batteringRam;

        private readonly float _pullTargetPitch = 2f;
        private readonly float _pitchIncrement = 0.04f;
        private float _currentPullPitch;

        private bool _pulling;
        private readonly float _cooldown = 2f;
        private float _pullTimer;

        public void Init(BatteringRam batteringRam)
        {
            _batteringRam = batteringRam;

            _currentPullPitch = 1f;
            _pulling = false;
            _pullTimer = _cooldown;

            AudioEvents.OnPlayRamCreak += HandleRamCreak;
            AudioEvents.OnPlayDoorHit += HandleDoorHit;
        }

        private void OnDisable()
        {
            AudioEvents.OnPlayRamCreak -= HandleRamCreak;
            AudioEvents.OnPlayDoorHit -= HandleDoorHit;
        }

        private void Update()
        {
            if (_pulling)
            {
                _pullTimer -= Time.deltaTime;
                if (_pullTimer < 0f)
                {
                    _pullTimer = _cooldown;
                    _pulling = false;
                }

                _currentPullPitch = Mathf.Lerp(_currentPullPitch, _pullTargetPitch, _pitchIncrement * Time.deltaTime);
            }
            else
                _currentPullPitch = 1f;
        }

        private void HandleRamCreak()
        {
            if (GameManager.GameState != Enums.GameState.Started) return;
            AudioHandler.PlayAudio(Enums.AudioType.Ram_Creak, 0.3f, _currentPullPitch);
            _pullTimer = _cooldown;
            _pulling = true;
        }

        private void HandleDoorHit()
        {
            if (GameManager.GameState != Enums.GameState.Started) return;
            AudioHandler.PlayAudio(Enums.AudioType.Ram_Hit, 0.6f);
            AudioHandler.PlayAudio(Enums.AudioType.Ram_Moving, 0.5f);
        }
    }
}
