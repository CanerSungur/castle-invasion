using UnityEngine;

namespace ZestGames
{
    [RequireComponent(typeof(Animator))]
    public class AiAnimationController : MonoBehaviour
    {
        private Ai _ai;

        #region ANIM PARAMETER SETUP

        // booleans
        private readonly int _runID = Animator.StringToHash("Run");
        private readonly int _dieID = Animator.StringToHash("Die");
        private readonly int _resettingID = Animator.StringToHash("Reset");
        private readonly int _rightID = Animator.StringToHash("Right");
        // triggers
        private readonly int _pullID = Animator.StringToHash("Pull");
        private readonly int _hitWallID = Animator.StringToHash("HitDoor");
        private readonly int _winID = Animator.StringToHash("Win");
        // floats
        private readonly int _runSpeedID = Animator.StringToHash("RunSpeed");

        #endregion

        private bool _ramIsReleased => _ai.BatteringRam.Movement.IsReleased;
        private bool _ramIsResetting => _ai.BatteringRam.Movement.Resetting;

        public void Init(Ai ai)
        {
            _ai = ai;
            _ai.Animator.SetBool(_rightID, _ai.CurrentSide == Ai.Side.Right ? true : false);

            Idle();

            //_ai.OnIdle += Idle;
            //_ai.OnMove += Run;
            _ai.OnDie += Die;
            _ai.OnWin += Win;
            PlayerEvents.OnRamPulled += Pull;
            PlayerEvents.OnHitWall += HitWall;
        }

        private void OnDisable()
        {
            //_ai.OnIdle -= Idle;
            //_ai.OnMove -= Run;
            _ai.OnDie -= Die;
            _ai.OnWin -= Win;
            PlayerEvents.OnRamPulled -= Pull;
            PlayerEvents.OnHitWall -= HitWall;
        }

        private void Update()
        {
            //if (_ai.BatteringRam.Movement.IsBeingPulled) return;
            //if (_ramIsResetting)
            //    Pull();
            if (_ramIsReleased)
                Run();
            else
            {
                _ai.Animator.SetBool(_runID, _ai.IsMoving);
            }

            _ai.Animator.SetBool(_resettingID, _ai.BatteringRam.Movement.Resetting);
        }

        private void Idle()
        {
            _ai.Animator.SetBool(_runID, false);
            _ai.Animator.SetBool(_dieID, false);
        }

        private void Run()
        {
            _ai.Animator.SetBool(_runID, true);
            _ai.Animator.SetBool(_dieID, false);
        }

        private void Die()
        {
            _ai.Animator.SetBool(_runID, false);
            _ai.Animator.SetBool(_dieID, true);
        }

        private void Win() => _ai.Animator.SetTrigger(_winID);
        private void Pull() => _ai.Animator.SetTrigger(_pullID);
        private void HitWall() => _ai.Animator.SetTrigger(_hitWallID);
    }
}
