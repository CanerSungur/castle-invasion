using UnityEngine;


namespace CastleInvasion
{
    public class BatteringRam : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Vector3 _pullRate = new Vector3(0f, 0.15f, -0.5f);

        private bool _startCooldown = false;
        private float _cooldown = 1f;
        private float _timer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = false;

            _timer = _cooldown;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.isKinematic = true;

                _startCooldown = false;
                _timer = _cooldown;

                transform.position += _pullRate;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _startCooldown = true;
            }

            if (_startCooldown)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0f)
                {
                    _timer = _cooldown;
                    _rigidbody.isKinematic = false;
                }
            }
        }
    }
}
