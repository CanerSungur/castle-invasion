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
            _agent.enabled = true;
            _agent.speed = _ai.CurrentMovementSpeed;
            _agent.updateRotation = updateRotation;
            transform.rotation = Quaternion.identity;

            _ai.OnSetTarget += SetTarget;
            _ai.OnDie += () => _agent.enabled = false;
            _ai.OnWin += Cheer;
        }

        private void OnDisable()
        {
            if (!_ai) return;
            _ai.OnSetTarget -= SetTarget;
            _ai.OnDie -= () => _agent.enabled = false;
            _ai.OnWin -= Cheer;
        }

        private void Update()
        {
            if (GameManager.GameEnd == Enums.GameEnd.Success) return;

            if (!_ai.CanMove || !_currentTarget) return;
            Motor();
        }

        public void Motor()
        {
            //transform.position = _currentTarget.position;

            #region With nav mesh start
            if (_ai.FirstInitialization)
            {
                _agent.SetDestination(_currentTarget.position);
                Navigation.LookAtTarget(transform, _currentTarget.position);

                if (Operation.IsTargetReached(transform, _currentTarget.position, 0.1f) && !_targetReached)
                {
                    _targetReached = true;
                    _ai.CancelFirstInitialization();
                    transform.rotation = Quaternion.identity;
                    _ai.IsMoving = false;
                }
                else
                    _ai.IsMoving = true;
            }
            else
                transform.position = _currentTarget.position;
            #endregion
        }

        private void SetTarget(Transform newTarget)
        {
            _currentTarget = newTarget;
            _targetReached = false;
        }

        private void Cheer()
        {
            _agent.speed = _ai.CurrentMovementSpeed * 0.5f;
            if (_ai.CurrentSide == Ai.Side.Left)
                _agent.SetDestination(transform.position + new Vector3(Random.Range(-3f, 0f), 0f, (Random.Range(-3f, 3f))));
            else if (_ai.CurrentSide == Ai.Side.Right)
                _agent.SetDestination(transform.position + new Vector3(Random.Range(0f, 3f), 0f, (Random.Range(-3f, 3f))));
            transform.rotation = Quaternion.Euler(0f, Random.Range(-90f, 90f), 0f);
        }
    }
}
