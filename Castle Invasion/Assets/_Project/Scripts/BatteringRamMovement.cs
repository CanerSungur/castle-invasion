using UnityEngine;
using ZestGames;
using DG.Tweening;

namespace CastleInvasion
{
    public class BatteringRamMovement : MonoBehaviour
    {
        private BatteringRam _batteringRam;

        private float _timer;
        private float _countdown = 1f;
        private bool _pulling, _released;

        private float _startingPoint;
        private float _targetPullPoint;
        private float _pullRate = 0.1f;

        public bool CanRelease => _pulling && !_released && GameManager.GameState == Enums.GameState.Started;
        public bool CanPull => !_released && GameManager.GameState == Enums.GameState.Started;
        public bool IsBeingPulled => _pulling;
        public bool IsReleased => _released;
        public bool Resetting { get; private set; }

        public void Init(BatteringRam batteringRam)
        {
            _batteringRam = batteringRam;

            _startingPoint = transform.position.z;
            _targetPullPoint = _startingPoint;
            _pulling = _released = false;
            Resetting = false;

            InputEvents.OnTapHappened += Move;
        }

        private void OnDisable()
        {
            InputEvents.OnTapHappened -= Move;
        }

        private void Update()
        {
            if (CanRelease)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    Release();
                }
            }
        }

        private void Move()
        {
            if (!CanPull || GameManager.GameState != Enums.GameState.Started) return;

            _timer = _countdown;
            _pulling = true;
            _targetPullPoint -= _pullRate;
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - _pullRate);

            transform.DOKill();
            transform.DOMoveZ(_targetPullPoint, 0.5f);
            PlayerEvents.OnRamPulled?.Invoke();
        }

        private void Release()
        {
            if (GameManager.GameState != Enums.GameState.Started) return;

            _released = true;
            _targetPullPoint = _startingPoint;

            transform.DOKill();
            transform.DOMoveZ(_batteringRam.Door.transform.position.z, 2f);
            PlayerEvents.OnRamReleased?.Invoke();
        }

        public void ResetPulling()
        {
            Resetting = true;
            transform.DOKill();
            transform.DOMoveZ(_targetPullPoint, 3f).OnComplete(() => {
                _pulling = _released = Resetting = false;
            });
        }
    }
}
