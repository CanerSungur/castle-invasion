using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class BatteringRamRow : MonoBehaviour
    {
        private BatteringRam _batteringRam;
        private Transform _leftTransform, _rightTransform;

        // Getters
        public Transform LeftTransform => _leftTransform;
        public Transform RightTransform => _rightTransform;

        public void Init(BatteringRam batteringRam)
        {
            _batteringRam = batteringRam;

            _leftTransform = transform.GetChild(0);
            _rightTransform = transform.GetChild(1);

            if (!_batteringRam.Rows.Contains(this))
                _batteringRam.Rows.Add(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Door door))
            {
                _batteringRam.Hit();
                door.GetHit();
                CameraManager.OnShakeCam?.Invoke();
            }
        }
    }
}
