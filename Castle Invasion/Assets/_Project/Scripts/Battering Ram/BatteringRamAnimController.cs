using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class BatteringRamAnimController : MonoBehaviour
    {
        private Animator _animator;

        private readonly int _hitID = Animator.StringToHash("Hit");
        private readonly int _pulledID = Animator.StringToHash("Pulled");

        public void Init(BatteringRam batteringRam)
        {
            _animator = GetComponent<Animator>();

            PlayerEvents.OnRamPulled += Pulled;
            PlayerEvents.OnHitDoor += Hit;
        }

        private void OnDisable()
        {
            PlayerEvents.OnRamPulled -= Pulled;
            PlayerEvents.OnHitDoor -= Hit;
        }

        private void Hit() => _animator.SetTrigger(_hitID);
        private void Pulled() => _animator.SetTrigger(_pulledID);
    }
}
