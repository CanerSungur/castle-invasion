using System.Collections.Generic;
using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace CastleInvasion
{
    public class BatteringRam : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private Door door;
        private List<BatteringRamRow> rows = new List<BatteringRamRow>();
        private List<Ai> leftRowSoldiers = new List<Ai>();
        private List<Ai> rightRowSoldiers = new List<Ai>();

        [Header("-- DAMAGE SETUP --")]
        private int _damage;

        [Header("-- STRUGGLE SETUP --")]
        private int _pullStaminaCost, _struggleLimit, _pullMaxStamina;
        private bool _firstHitHappened = false;
        private readonly float _distanceBetweenRamRows = 1.031f;

        public int StrugglePullCount { get; private set; }
        public int PullCount { get; private set; }
        public bool DoorIsBroken { get; private set; }

        // Struggle after first hit
        private readonly float _defaultLimitDecreaseRate = 0.5f;
        private readonly float _limitDecreaseRate = 0.1f;
        private int _hitCount = 0;

        #region Components
        private Rigidbody _rigidbody;
        #endregion

        #region Script References
        private BatteringRamMovement _movement;
        private BatteringRamAnimController _animationController;
        #endregion

        #region Getters
        public List<BatteringRamRow> Rows => rows;
        public Door Door => door;
        public Rigidbody Rigidbody => _rigidbody;
        public BatteringRamMovement Movement => _movement;
        public int Damage => _damage;
        #endregion

        private void Start()
        {
            DoorIsBroken = false;

            _rigidbody = GetComponent<Rigidbody>();

            _movement = GetComponent<BatteringRamMovement>();
            _movement.Init(this);
            _animationController = GetComponent<BatteringRamAnimController>();
            _animationController.Init(this);

            Delayer.DoActionAfterDelay(this, 0.5f, SetupRam);

            _pullStaminaCost = DataManager.PullStaminaCost;
            _struggleLimit = DataManager.StruggleLimit;
            _pullMaxStamina = (int)DataManager.MaxStamina;
            StrugglePullCount = PullCount = 0;

            PlayerEvents.OnRamPulled += IncreasePullCount;
            PlayerEvents.OnRamReleased += ResetPullCount;
            PlayerEvents.OnSetCurrentStamina += UpdateLimits;
            PlayerEvents.OnSetCurrentSize += AddRow;

            DoorEvents.OnBreak += () => DoorIsBroken = true;
        }

        private void OnDisable()
        {
            rows.Clear();

            PlayerEvents.OnRamPulled -= IncreasePullCount;
            PlayerEvents.OnRamReleased -= ResetPullCount;
            PlayerEvents.OnSetCurrentStamina -= UpdateLimits;
            PlayerEvents.OnSetCurrentSize -= AddRow;

            DoorEvents.OnBreak -= () => DoorIsBroken = true;
        }

        public int Hit()
        {
            UpdateLimits();
            _hitCount++;

            ResetPull();
            CalculateDamage();
            PullCount = 0;
            return _damage;
        }

        #region Setup Functions

        private void SetupRam()
        {
            for (int i = 0; i < DataManager.CurrentSize; i++)
            {
                BatteringRamRow row;
                if (i == 0)
                    row = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.RamRowTip, Vector3.zero, Quaternion.identity).GetComponent<BatteringRamRow>();
                else if (i == DataManager.CurrentSize - 1)
                    row = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.RamRowEnd, Vector3.zero, Quaternion.identity).GetComponent<BatteringRamRow>();
                else
                    row = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.RamRow, Vector3.zero, Quaternion.identity).GetComponent<BatteringRamRow>();

                //BatteringRamRow row = Instantiate(rowPrefab, Vector3.zero, Quaternion.identity).GetComponent<BatteringRamRow>();
                row.transform.parent = transform;
                row.transform.localPosition = new Vector3(0, 0, -i * _distanceBetweenRamRows);

                row.Init(this);

                AddSoldiers();
            }
        }

        private void AddRow()
        {
            //// Change last row mesh to middle row mesh first
            rows[rows.Count - 1].ChangeEndPartMesh();

            // Then add last row as row end style
            BatteringRamRow row = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.RamRowEnd, Vector3.zero, Quaternion.identity).GetComponent<BatteringRamRow>();
            row.transform.parent = transform;
            row.transform.localPosition = new Vector3(0, 0, -(DataManager.CurrentSize - 1) * _distanceBetweenRamRows);
            row.Init(this);

            AddSoldiers();
        }

        private void AddSoldiers()
        {
            // Setup left row soldier
            Ai leftAi = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.LeftRowSoldier, new Vector3(-15f, 0f, -26f), Quaternion.identity).GetComponent<Ai>();
            leftAi.transform.parent = null;
            if (!leftRowSoldiers.Contains(leftAi))
            {
                leftRowSoldiers.Add(leftAi);
                leftAi.SetSoldierRowNumber(leftRowSoldiers.IndexOf(leftAi));
            }

            // Setup right row soldier
            Ai rightAi = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.RightRowSoldier, new Vector3(15f, 0f, -26f), Quaternion.identity).GetComponent<Ai>();
            rightAi.transform.parent = null;
            if (!rightRowSoldiers.Contains(rightAi))
            {
                rightRowSoldiers.Add(rightAi);
                rightAi.SetSoldierRowNumber(rightRowSoldiers.IndexOf(rightAi));
            }
        }

        #endregion

        private void CalculateDamage() => _damage = DataManager.CurrentDamage * PullCount;

        private void ResetPull()
        {
            Delayer.DoActionAfterDelay(this, 1f, () =>
            {
                transform.rotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                Movement.ResetPulling();
            });
        }

        private void IncreasePullCount()
        {
            PullCount++;
            _pullMaxStamina -= _pullStaminaCost;
            if (_pullMaxStamina <= _struggleLimit)
            {
                PlayerEvents.OnStartStruggle?.Invoke();
                StrugglePullCount++;
            }
            if (_pullMaxStamina <= 0)
            {
                GameEvents.OnGameEnd?.Invoke(Enums.GameEnd.Fail);
                PlayerEvents.OnStopStruggle?.Invoke();
            }
        }

        private void ResetPullCount()
        {
            _pullStaminaCost = DataManager.PullStaminaCost;
            StrugglePullCount = 0;

            PlayerEvents.OnStopStruggle?.Invoke();
        }

        private void UpdateLimits()
        {
            if (_defaultLimitDecreaseRate > (_limitDecreaseRate * _hitCount))
            {
                _struggleLimit = (int)(DataManager.StruggleLimit * (_defaultLimitDecreaseRate - (_limitDecreaseRate * _hitCount)));
                _pullMaxStamina = (int)(DataManager.MaxStamina * (_defaultLimitDecreaseRate - (_limitDecreaseRate * _hitCount)));
                PlayerEvents.OnDecreaseAiLimits?.Invoke(_hitCount);
            }
        }
    }
}
