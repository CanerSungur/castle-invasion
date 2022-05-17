using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ZestCore.Utility;

namespace CastleInvasion
{
    public class BatteringRam : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private GameObject rowPrefab;
        [SerializeField] private Door door;
        private List<BatteringRamRow> rows = new List<BatteringRamRow>();

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
        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _movement = GetComponent<BatteringRamMovement>();
            _movement.Init(this);
            _animationController = GetComponent<BatteringRamAnimController>();
            _animationController.Init(this);

            for (int i = 0; i < 6; i++)
            {
                BatteringRamRow row = Instantiate(rowPrefab, Vector3.zero, Quaternion.identity).GetComponent<BatteringRamRow>();
                row.transform.parent = transform;
                row.transform.localPosition = new Vector3(0, 0, -i * 1f);
                
                row.Init(this);
            }
        }

        private void OnDisable()
        {
            rows.Clear();
        }

        public void Hit()
        {
            Shake();
        }

        private void Shake()
        {
            transform.DOKill();

            transform.DOShakePosition(.35f, .1f, 1);
            transform.DOShakeScale(.35f, .2f, 1);
            transform.DOShakeRotation(.35f, .1f, 1);

            Delayer.DoActionAfterDelay(this, 1f, () => {
                transform.rotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                Movement.ResetPulling();
            });
        }
    }
}
