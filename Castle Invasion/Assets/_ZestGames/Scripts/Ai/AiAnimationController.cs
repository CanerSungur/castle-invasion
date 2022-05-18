using UnityEngine;

namespace ZestGames
{
    [RequireComponent(typeof(Animator))]
    public class AiAnimationController : MonoBehaviour
    {
        private Ai _ai;

        #region ANIM PARAMETER SETUP

        private readonly int _idleID = Animator.StringToHash("Idle");
        private readonly int _runID = Animator.StringToHash("Run");
        private readonly int _dieID = Animator.StringToHash("Die");
        private readonly int _resettingID = Animator.StringToHash("Reset");
        private readonly int _pullID = Animator.StringToHash("Pull");
        private readonly int _hitWallID = Animator.StringToHash("HitWall");

        #endregion

        private bool _ramIsReleased => _ai.BatteringRam.Movement.IsReleased;
        private bool _ramIsResetting => _ai.BatteringRam.Movement.Resetting;

        public void Init(Ai ai)
        {
            _ai = ai;

            //_ai.Animator.SetBool(_pullingID, false);
            Idle();

            //_ai.OnIdle += Idle;
            //_ai.OnMove += Run;
            _ai.OnDie += Die;
            PlayerEvents.OnRamPulled += Pull;
            PlayerEvents.OnHitWall += HitWall;
        }

        private void OnDisable()
        {
            //_ai.OnIdle -= Idle;
            //_ai.OnMove -= Run;
            _ai.OnDie -= Die;
            PlayerEvents.OnRamPulled -= Pull;
            PlayerEvents.OnHitWall -= HitWall;
        }

        private void Update()
        {
            //if (_ai.BatteringRam.Movement.IsBeingPulled) return;
            if (_ramIsResetting)
                Pull();
            else if (_ramIsReleased)
                Run();
            else
            {
                if (_ai.IsMoving)
                {
                    Run();

                }
                else
                    Idle();
            }
        }

        private void Idle()
        {
            _ai.Animator.SetBool(_idleID, true);
            _ai.Animator.SetBool(_runID, false);
            _ai.Animator.SetBool(_dieID, false);
        }

        private void Run()
        {
            _ai.Animator.SetBool(_idleID, false);
            _ai.Animator.SetBool(_runID, true);
            _ai.Animator.SetBool(_dieID, false);
        }

        private void Die()
        {
            _ai.Animator.SetBool(_idleID, false);
            _ai.Animator.SetBool(_runID, false);
            _ai.Animator.SetBool(_dieID, true);
        }

        public void StartResetting() => _ai.Animator.SetBool(_resettingID, true);
        private void Pull() => _ai.Animator.SetTrigger(_pullID);
        private void HitWall() => _ai.Animator.SetTrigger(_hitWallID);
    }
}
