using UnityEngine;

namespace CastleInvasion
{
    public class BatteringRamRow : MonoBehaviour
    {
        private Transform _leftTransform, _rightTransform;

        // Getters
        public Transform LeftTransform => _leftTransform;
        public Transform RightTransform => _rightTransform;

        private void Awake()
        {
            _leftTransform = transform.GetChild(0);
            _rightTransform = transform.GetChild(1);
        }
    }
}
