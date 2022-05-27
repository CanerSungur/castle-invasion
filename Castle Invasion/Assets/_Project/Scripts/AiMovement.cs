using UnityEngine;
using DG.Tweening;
using ZestCore.Ai;

namespace ZestGames
{
    public class AiMovement : MonoBehaviour, IAiMovement
    {
        private Ai _ai;
        private Transform _currentTarget;
        private bool _targetReached = false;

        [Header("-- MOVEMENT SETUP --")]
        [SerializeField] private bool updateRotation = true;

        public bool IsMoving => _ai.Rigidbody.velocity.magnitude > 0.05f;
        public bool IsGrounded => _ai.IsGrounded;

        public void Init(Ai ai)
        {
            _ai = ai;
            _currentTarget = ai.Target;
            //_agent = GetComponent<NavMeshAgent>();
            //_agent.enabled = true;
            //_agent.speed = _ai.CurrentMovementSpeed;
            //_agent.updateRotation = updateRotation;
            transform.rotation = Quaternion.identity;

            _ai.OnSetTarget += SetTarget;
            //_ai.OnDie += () => _agent.enabled = false;
            _ai.OnWin += Cheer;
        }

        private void OnDisable()
        {
            _ai.OnSetTarget -= SetTarget;
            //_ai.OnDie -= () => _agent.enabled = false;
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
            transform.position = _currentTarget.position;
        }

        private void SetTarget(Transform newTarget)
        {
            _currentTarget = newTarget;
            _targetReached = false;
        }

        private void Cheer()
        {
            //_agent.speed = _ai.CurrentMovementSpeed * 0.5f;
            if (_ai.CurrentSide == Ai.Side.Left)
                Navigation.MoveTransform(transform, transform.position + new Vector3(Random.Range(-3f, 0f), 0f, Random.Range(-3f, 3f)));
            else if (_ai.CurrentSide == Ai.Side.Right)
                Navigation.MoveTransform(transform, transform.position + new Vector3(Random.Range(0f, 3f), 0f, Random.Range(-3f, 3f)));
            transform.rotation = Quaternion.Euler(0f, Random.Range(-90f, 90f), 0f);
        }
    }
}
