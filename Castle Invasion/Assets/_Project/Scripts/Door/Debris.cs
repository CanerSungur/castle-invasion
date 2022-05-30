using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class Debris : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private readonly float _minZForce = 1.5f;
        private readonly float _maxZForce = 2.5f;
        private readonly float _minXForce = -1f;
        private readonly float _maxXForce = 1f;

        private Vector3 _defaultPos;

        private bool _soundHasPlayer = false;

        private void OnEnable()
        {
            _defaultPos = transform.position;

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.AddForce(new Vector3(Random.Range(_minXForce, _maxXForce), 0f, -Random.Range(_minZForce, _maxZForce)), ForceMode.Impulse);
        }

        private void OnDisable()
        {
            transform.position = _defaultPos;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_soundHasPlayer)
            {
                _soundHasPlayer = true;
                AudioHandler.PlayAudio(Enums.AudioType.Wood_Impact, 0.5f);
            }
        }
    }
}
