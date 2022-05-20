using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class BatteringRamRow : MonoBehaviour
    {
        private BatteringRam _batteringRam;
        private Transform _leftTransform, _rightTransform;
        private Rigidbody _rigidbody;

        // Getters
        public Transform LeftTransform => _leftTransform;
        public Transform RightTransform => _rightTransform;

        public void Init(BatteringRam batteringRam)
        {
            _batteringRam = batteringRam;

            _leftTransform = transform.GetChild(0);
            _rightTransform = transform.GetChild(1);

            _rigidbody = GetComponent<Rigidbody>();
            InitRigidbody();

            if (!_batteringRam.Rows.Contains(this))
                _batteringRam.Rows.Add(this);

            GameEvents.OnGameEnd += HandleGameEnd;
        }

        private void OnDisable()
        {
            GameEvents.OnGameEnd -= HandleGameEnd;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Door door) && GameManager.GameState != Enums.GameState.GameEnded)
            {
                _batteringRam.Hit();
                door.GetHit();
                PlayerEvents.OnHitWall?.Invoke();
                CameraManager.OnShakeCam?.Invoke();
            }
        }

        private void InitRigidbody()
        {
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }

        private void HandleGameEnd(Enums.GameEnd gameEnd)
        {
            if (gameEnd == Enums.GameEnd.Fail)
            {
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;
                _rigidbody.AddForce(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(-1f, 1f)) * UnityEngine.Random.Range(2f, 5f), ForceMode.Impulse);
            }
        }
    }
}
