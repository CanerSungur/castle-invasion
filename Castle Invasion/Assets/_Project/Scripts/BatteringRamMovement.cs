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

        public void Init(BatteringRam batteringRam)
        {
            _batteringRam = batteringRam;

            _startingPoint = transform.position.z;
            _targetPullPoint = _startingPoint;
            _pulling = _released = false;

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
            _timer = _countdown;
            _pulling = true;
            _targetPullPoint -= _pullRate;
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - _pullRate);

            transform.DOKill();
            transform.DOMoveZ(_targetPullPoint, 0.5f);
        }

        private void Release()
        {
            _released = true;
            _targetPullPoint = _startingPoint;

            transform.DOKill();
            transform.DOMoveZ(_batteringRam.Door.transform.position.z, 2f);
        }

        public void ResetPulling()
        {
            transform.DOKill();
            transform.DOMoveZ(_targetPullPoint, 2f).OnComplete(() => {
                _pulling = _released = false;
            });
        }
    }
}
