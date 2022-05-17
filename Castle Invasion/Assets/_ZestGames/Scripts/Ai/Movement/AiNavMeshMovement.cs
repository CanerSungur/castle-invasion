using UnityEngine;
using UnityEngine.AI;
using ZestCore.Ai;

namespace ZestGames
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AiNavMeshMovement : MonoBehaviour, IAiMovement
    {
        private Ai _ai;
        private Transform _currentTarget;
        private NavMeshAgent _agent;
        private bool _targetReached = false;

        [Header("-- MOVEMENT SETUP --")]
        [SerializeField] private bool updateRotation = true;

        public bool IsMoving => _agent.velocity.magnitude > 0.05f;
        public bool IsGrounded => _ai.IsGrounded;

        public void Init(Ai ai)
        {
            _ai = ai;
            _currentTarget = ai.Target;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _ai.CurrentMovementSpeed;
            _agent.updateRotation = updateRotation;
            transform.rotation = Quaternion.identity;

            _ai.OnSetTarget += SetTarget;
        }

        private void OnDisable()
        {
            _ai.OnSetTarget -= SetTarget;
        }

        private void Update()
        {
            if (!_ai.CanMove || !_currentTarget) return;
            Motor();
        }

        public void Motor()
        {
            _agent.SetDestination(_currentTarget.position);

            if (Operation.IsTargetReached(transform, _currentTarget.position, 2) && !_targetReached)
            {
                _targetReached = true;
                _ai.CancelFirstInitialization();
                _currentTarget = null;
                _ai.OnIdle?.Invoke();
            }
            else
                _targetReached = false;
        }

        private void SetTarget(Transform newTarget)
        {
            _currentTarget = newTarget;
        }
    }
}
