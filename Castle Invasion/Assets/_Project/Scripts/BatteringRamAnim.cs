using UnityEngine;
using DG.Tweening;
using ZestCore.Utility;

namespace CastleInvasion
{
    public class BatteringRamAnim : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        //[SerializeField] private Transform doorTransform;

        private float _zAxisPull = 0.5f;
        private float _yAxisPull = 0.1f;

        private bool _startCooldown = false;
        private float _cooldown = 1f;
        private float _timer;
        private Vector3 _defaultPos;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            transform.DOMoveY(transform.position.y - 3f, 3f).SetEase(Ease.OutBounce);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _startCooldown = false;
                _timer = _cooldown;

                transform.DOMoveZ(transform.position.z - _zAxisPull, 0.5f);
                transform.DOMoveY(transform.position.y + _yAxisPull, 0.5f);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _startCooldown = true;
            }

            ReleaseTimer();
        }

        private void ReleaseTimer()
        {
            if (_startCooldown)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0f)
                {
                    _timer = _cooldown;
                    //transform.DOJump(doorTransform.position, -1f, 1, 3f);
                    _rigidbody.isKinematic = false;
                    //Delayer.DoActionAfterDelay(this, 3f, () => {
                    //    _rigidbody.isKinematic = true;
                    //    transform.DOMove(_defaultPos, 3f).SetEase(Ease.OutBounce);
                    //});
                }
            }
        }
    }
}
