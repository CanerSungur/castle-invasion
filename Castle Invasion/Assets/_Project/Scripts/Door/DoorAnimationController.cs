using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class DoorAnimationController : MonoBehaviour
    {
        private Animator _animator;

        private readonly int _breakID = Animator.StringToHash("Break");
        private readonly int _getHitID = Animator.StringToHash("GetHit");

        public void Init(Door door)
        {
            _animator = GetComponent<Animator>();

            DoorEvents.OnGetHit += GetHit;
            DoorEvents.OnBreak += Break;
        }

        private void OnDisable()
        {
            DoorEvents.OnGetHit -= GetHit;
            DoorEvents.OnBreak -= Break;
        }

        private void GetHit() => _animator.SetTrigger(_getHitID);
        private void Break() => _animator.SetTrigger(_breakID);
    }
}
