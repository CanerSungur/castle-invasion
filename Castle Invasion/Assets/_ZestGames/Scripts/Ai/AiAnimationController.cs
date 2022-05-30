using UnityEngine;
using DG.Tweening;
using System;

namespace ZestGames
{
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
        private readonly int _pullSpeedID = Animator.StringToHash("PullSpeed");
        private readonly int _struggleRateID = Animator.StringToHash("StruggleRate");
        // ints
        private readonly int _winCountID = Animator.StringToHash("WinCount");
        // layer
        private readonly int _carryingLayer = 1;

        #endregion

        private float _currentPullSpeed;
        private readonly float _defaultPullSpeed = 1f;
        private readonly float _struggledPullSPeed = 0.75f;

        private bool _ramIsReleased => _ai.BatteringRam.Movement.IsReleased;

        public void Init(Ai ai)
        {
            _ai = ai;
            _ai.Animator.SetBool(_rightID, _ai.CurrentSide == Ai.Side.Right ? true : false);
            _currentPullSpeed = _defaultPullSpeed;
            _ai.Animator.SetLayerWeight(_carryingLayer, 1f);
            DefaultPullSpeed();

            Idle();

            _ai.OnDie += Die;
            _ai.OnWin += Win;
            PlayerEvents.OnRamPulled += Pull;
            PlayerEvents.OnHitDoor += HitWall;
            PlayerEvents.OnStartStruggle += StruggledPullSpeed;
            PlayerEvents.OnStopStruggle += DefaultPullSpeed;
        }

        private void OnDisable()
        {
            if (!_ai) return;
            _ai.OnDie -= Die;
            _ai.OnWin -= Win;
            PlayerEvents.OnRamPulled -= Pull;
            PlayerEvents.OnHitDoor -= HitWall;
            PlayerEvents.OnStartStruggle -= StruggledPullSpeed;
            PlayerEvents.OnStopStruggle -= DefaultPullSpeed;
        }

        private void Update()
        {
            if (GameManager.GameEnd == Enums.GameEnd.Success)
            {
                _ai.Animator.SetFloat(_struggleRateID, 0);
                return;
            }

            if (_ramIsReleased)
                Run();
            else
            {
                _ai.Animator.SetBool(_runID, _ai.IsMoving);
            }

            _ai.Animator.SetBool(_resettingID, _ai.BatteringRam.Movement.Resetting);
            _ai.Animator.SetFloat(_struggleRateID, _ai.StaminaController.CurrentStruggleRate);
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
            _ai.Animator.SetLayerWeight(_carryingLayer, 0f);

            _ai.Animator.SetBool(_runID, false);
            _ai.Animator.SetBool(_dieID, true);
        }

        private void Win()
        { 
            _ai.Animator.SetLayerWeight(_carryingLayer, 0f); 
            _ai.Animator.SetTrigger(_winID);
            _ai.Animator.SetInteger(_winCountID, UnityEngine.Random.Range(0, 2));
        }
        private void Pull() => _ai.Animator.SetTrigger(_pullID);
        private void HitWall() => _ai.Animator.SetTrigger(_hitWallID);
        private void DefaultPullSpeed()
        {
            DOVirtual.Float(_currentPullSpeed, _defaultPullSpeed, 1f, r => {
                _ai.Animator.SetFloat(_pullSpeedID, r);
                _currentPullSpeed = r;
            });
        }
        private void StruggledPullSpeed()
        {
            DOVirtual.Float(_currentPullSpeed, _struggledPullSPeed, 1f, r => {
                _ai.Animator.SetFloat(_pullSpeedID, r);
                _currentPullSpeed = r;
            });
        }
    }
}
