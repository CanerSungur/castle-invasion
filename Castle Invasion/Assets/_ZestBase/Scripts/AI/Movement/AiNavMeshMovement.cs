using UnityEngine;
using UnityEngine.AI;

namespace ZestGames
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AiNavMeshMovement : MonoBehaviour, IAiMovement
    {
        private Ai _ai;
        private Transform _currentTarget;
        private NavMeshAgent _agent;

        public bool IsMoving => _agent.velocity.magnitude > 0.05f;
        public bool IsGrounded => _ai.IsGrounded;

        public void Init(Ai ai)
        {
            _ai = ai;
            _currentTarget = ai.Target;
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (!_ai.CanMove) return;
            Motor();
        }

        public void Motor()
        {
            _agent.SetDestination(_currentTarget.position);
            _agent.speed = _ai.CurrentMovementSpeed;
        }
    }
}
