using UnityEngine;
using System;
using ZestCore.Utility;
using CastleInvasion;

namespace ZestGames
{
    public class Ai : MonoBehaviour
    {
        public enum Side { Left, Right }
        private bool _firstInitialization;

        #region COMPONENTS

        private Animator animator;
        public Animator Animator => animator == null ? animator = GetComponent<Animator>() : animator;

        private Collider coll;
        public Collider Collider => coll == null ? coll = GetComponent<Collider>() : coll;

        private Rigidbody rb;
        public Rigidbody Rigidbody => rb == null ? rb = GetComponent<Rigidbody>() : rb;

        #endregion

        #region SCRIPT REFERENCES

        private AiAnimationController animationController;
        public AiAnimationController AnimationController => animationController == null ? animationController = GetComponent<AiAnimationController>() : animationController;

        private AiCollision collision;
        public AiCollision Collision => collision == null ? collision = GetComponent<AiCollision>() : collision;

        private IAiMovement movement;
        public IAiMovement Movement => movement == null ? movement = GetComponent<IAiMovement>() : movement;

        private AiStaminaController staminaController;
        public AiStaminaController StaminaController => staminaController == null ? staminaController = GetComponent<AiStaminaController>() : staminaController;

        #endregion

        [Header("-- MOVEMENT SETUP --")]
        [SerializeField] private float maxMovementSpeed = 3f;
        [SerializeField] private float minMovementSpeed = 1f;
        [SerializeField] private bool useAcceleration = false;
        [SerializeField, Range(0.1f, 3f)] private float accelerationRate = 0.5f;
        private float _currentMovementSpeed;

        [Header("-- GROUNDED SETUP --")]
        [SerializeField, Tooltip("Select layers that you want this object to be grounded.")] private LayerMask groundLayerMask;
        [SerializeField, Tooltip("Height that this object will be considered grounded when above groundable layers.")] private float groundedHeightLimit = 0.05f;

        [Header("-- SOLDIER SETUP --")]
        [SerializeField] private Side currentSide;
        private int _soldierRowNumber = 0;
        private BatteringRam _batteringRam;

        #region CONTROLS

        public bool IsDead { get; private set; }
        public Transform Target { get; private set; }

        public bool CanMove => Target != null && GameManager.GameState != Enums.GameState.GameEnded;
        public bool IsMoving { get; set; }
        public bool IsGrounded => Physics.Raycast(Collider.bounds.center, Vector3.down, Collider.bounds.extents.y + groundedHeightLimit, groundLayerMask);
        public float CurrentMovementSpeed => _currentMovementSpeed;
        public BatteringRam BatteringRam => _batteringRam;
        public Side CurrentSide => currentSide;

        #endregion

        #region EVENTS

        public Action OnIdle, OnMove, OnDie, OnWin;
        public Action<Transform> OnSetTarget;

        #endregion

        private void Init()
        {
            _firstInitialization = true;
            IsDead = false;
            Target = null;
            if (useAcceleration)
                _currentMovementSpeed = minMovementSpeed;
            else
                _currentMovementSpeed = maxMovementSpeed;

            _batteringRam = FindObjectOfType<BatteringRam>();

            CharacterTracker.AddAi(this);
            
            AnimationController.Init(this);
            Movement.Init(this);
            StaminaController.Init(this);

            OnSetTarget += SetTarget;
            GameEvents.OnGameEnd += HandleGameEnd;

            Delayer.DoActionAfterDelay(this, 1f, () => {
                if (currentSide == Side.Left)
                    Target = _batteringRam.Rows[_soldierRowNumber].LeftTransform;
                else if (currentSide == Side.Right)
                    Target = _batteringRam.Rows[_soldierRowNumber].RightTransform;

                OnSetTarget?.Invoke(Target);
            });
        }

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            CharacterTracker.RemoveAi(this);
            OnSetTarget -= SetTarget;
            GameEvents.OnGameEnd -= HandleGameEnd;
        }

        private void Update()
        {
            if (!IsMoving && IsGrounded && Rigidbody) Rigidbody.velocity = Vector3.zero;

            UpdateCurrentMovementSpeed();

            if (!_firstInitialization)
                OnSetTarget?.Invoke(Target);
        }

        private void UpdateCurrentMovementSpeed()
        {
            if (!useAcceleration) return;

            if (IsMoving)
                _currentMovementSpeed = Mathf.MoveTowards(_currentMovementSpeed, maxMovementSpeed, accelerationRate * Time.deltaTime);
            else
                _currentMovementSpeed = minMovementSpeed;
        }

        private void Die()
        {
            IsDead = true;
            OnDie?.Invoke();
            CharacterTracker.RemoveAi(this);
            Delayer.DoActionAfterDelay(this, 5f, () => gameObject.SetActive(false));
        }

        public void SetTarget(Transform transform)
        {
            if (!CanMove) return;

            Target = transform;
            OnMove?.Invoke();
        }

        private void HandleGameEnd(Enums.GameEnd gameEnd)
        {
            if (gameEnd == Enums.GameEnd.Fail)
            {
                OnDie?.Invoke();
                Rigidbody.AddForce(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(-1f, 1f)) * 5, ForceMode.Impulse);
                transform.rotation = Quaternion.Euler(transform.rotation.x, UnityEngine.Random.Range(0f, 180f), transform.rotation.z);
            }
            else if (gameEnd == Enums.GameEnd.Success)
                OnWin?.Invoke();
        }

        public void CancelFirstInitialization()
        {
            _firstInitialization = false;
            //AnimationController.StartedPulling();
        }

        public void SetSoldierRowNumber(int rowNumber) => _soldierRowNumber = rowNumber;
    }
}
