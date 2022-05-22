using UnityEngine;
using ZestGames;
using DG.Tweening;

namespace CastleInvasion
{
    public class AiStaminaController : MonoBehaviour
    {
        private Ai _ai;
        private bool _startedStruggling = false;

        [Header("-- EFFECTS --")]
        [SerializeField] private ParticleSystem sweatParticles;
        [SerializeField] private float rendererChangeTime = 1f;
        private SkinnedMeshRenderer _renderer;
        //private Color _currentRimColor;
        private float _currentRimValue;
        private float _currentRimSize;
        private float _currentStruggleRate;

        public float CurrentStruggleRate => _currentStruggleRate;

        public void Init(Ai ai)
        {
            _ai = ai;

            _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            _renderer.material.SetFloat("_RimEnabled", 1);
            _renderer.material.SetColor("_FlatRimColor", Color.red);
            _renderer.material.SetFloat("_FlatRimLightAlign", 0);
            _renderer.material.SetFloat("_FlatRimSize", 0);
            //_currentRimColor = Color.white;
            _currentRimValue = _currentRimSize = _currentStruggleRate = 0f;

            UpdateRendererChangeTime();

            PlayerEvents.OnStartStruggle += StartStruggle;
            PlayerEvents.OnStopStruggle += StopStruggle;
            PlayerEvents.OnDecreaseAiLimits += DecreaseAiLimits;
        }

        private void OnDisable()
        {
            PlayerEvents.OnStartStruggle -= StartStruggle;
            PlayerEvents.OnStopStruggle -= StopStruggle;
            PlayerEvents.OnDecreaseAiLimits -= DecreaseAiLimits;
        }

        private void StartStruggle()
        {
            if (_startedStruggling) return;
            _startedStruggling = true;
            
            sweatParticles.Play();
            MakeCharacterRed();
        }
        private void StopStruggle()
        {
            if (!_startedStruggling) return;
            _startedStruggling = false;

            sweatParticles.Stop();
            MakeCharacterNormal();
        }

        private void MakeCharacterRed()
        {
            _renderer.DOKill();

            //DOVirtual.Color(_currentRimColor, Color.red, rendererChangeTime, r => {
            //    _renderer.material.SetColor("_FlatRimColor", r);
            //    _currentRimColor = r;
            //});
            DOVirtual.Float(_currentRimValue, 1f, rendererChangeTime, r => {
                _renderer.material.SetFloat("_FlatRimLightAlign", r);
                _currentRimValue = r;
            });
            DOVirtual.Float(_currentRimSize, 0.5f, rendererChangeTime, r => {
                _renderer.material.SetFloat("_FlatRimSize", r);
                _currentRimSize = r;
            });
            DOVirtual.Float(_currentStruggleRate, 2f, rendererChangeTime, r => {
                _currentStruggleRate = r;
            });
        }

        private void MakeCharacterNormal()
        {
            _renderer.DOKill();

            //DOVirtual.Color(_currentRimColor, Color.white, rendererChangeTime + 2f, r => {
            //    _renderer.material.SetColor("_FlatRimColor", r);
            //    _currentRimColor = r;
            //});
            DOVirtual.Float(_currentRimValue, 0, rendererChangeTime + 2f, r => {
                _renderer.material.SetFloat("_FlatRimLightAlign", r);
                _currentRimValue = r;
            });
            DOVirtual.Float(_currentRimSize, 0, rendererChangeTime + 2f, r => {
                _renderer.material.SetFloat("_FlatRimSize", r);
                _currentRimSize = r;
            });
            DOVirtual.Float(_currentStruggleRate, 0f, rendererChangeTime, r => {
                _currentStruggleRate = r;
            });
        }

        private void UpdateRendererChangeTime()
        {
            rendererChangeTime = (DataManager.StruggleLimit / DataManager.PullStaminaCost) * 0.75f;
            Debug.Log(rendererChangeTime);
        }

        private void DecreaseAiLimits(int hitCount)
        {
            if (hitCount > 0)
                rendererChangeTime = (DataManager.StruggleLimit / DataManager.PullStaminaCost) * 0.75f / hitCount;
        }
    }
}
