using UnityEngine;
using DG.Tweening;

namespace ZestGames
{
    public class AiMovement : MonoBehaviour, IAiMovement
    {
        private Ai _ai;
        private Transform _currentTarget;
        private Vector3 _currentPosition;
        private bool _targetReached = false;

        public bool IsMoving { get; private set; }
        public bool IsGrounded { get; private set; }

        public void Init(Ai ai)
        {
            _ai = ai;
            _currentTarget = ai.Target;
            transform.rotation = Quaternion.identity;

            _ai.OnSetTarget += SetTarget;
        }

        private void OnDisable()
        {
            _ai.OnSetTarget -= SetTarget;
        }

        private void SetTarget(Transform target) => _currentTarget = target;
        
        public void Motor()
        {
            
        }
    }
}
