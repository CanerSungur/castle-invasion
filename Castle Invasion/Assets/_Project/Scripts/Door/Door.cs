using UnityEngine;
using DG.Tweening;
using ZestGames;

namespace CastleInvasion
{
    public class Door : MonoBehaviour
    {
        private DoorAnimationController animationController;
        public DoorAnimationController AnimationController => animationController == null ? animationController = GetComponent<DoorAnimationController>() : animationController;

        [Header("-- SETUP --")]
        [SerializeField] private int maxHealth = 100;
        private int _currentHealth;
        private bool _gotHit = false;

        [Header("-- EFFECT SETUP --")]
        [SerializeField] private ParticleSystem woodParticles;

        public bool GotHit => _gotHit;

        private void Awake()
        {
            _currentHealth = maxHealth;
            AnimationController.Init(this);

            DoorEvents.OnResetDoor += () => _gotHit = false;
        }

        private void OnDisable()
        {
            DoorEvents.OnResetDoor -= () => _gotHit = false;
        }

        public void GetHit(int damage)
        {
            _gotHit = true;
            _currentHealth -= damage;
            woodParticles.Play();
            if (_currentHealth <= 0)
                Break();

            DoorEvents.OnGetHit?.Invoke();
            //Shake();
        }

        private void Break()
        {
            DoorEvents.OnBreak?.Invoke();
            GameEvents.OnGameEnd?.Invoke(Enums.GameEnd.Success);
        }

        private void Shake()
        {
            transform.DOShakeScale(.25f, .25f);
            transform.DOShakeRotation(.25f, .25f);
        }
    }
}
