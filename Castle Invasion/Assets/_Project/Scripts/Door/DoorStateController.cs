using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class DoorStateController : MonoBehaviour
    {
        private Door _door;
        private Enums.DoorState _currentState;

        [Header("-- SETUP --")]
        private SkinnedMeshRenderer _renderer;
        private Material _brokenMat;
        private readonly int _dentBlendKey = 1;
        private readonly int _breakBlendKey = 2;
        private bool _isCracked = false;

        [Header("-- DEBRIS SETUP --")]
        [SerializeField] private GameObject debris_1;
        [SerializeField] private GameObject debris_2;
        [SerializeField] private GameObject debris_3;
        [SerializeField] private GameObject debris_4;
        [SerializeField] private GameObject finalDebris;
       
        public void Init(Door door)
        {
            _door = door;
            _currentState = Enums.DoorState.Solid;

            _renderer = GetComponent<SkinnedMeshRenderer>();
            _renderer.enabled = true;
            _brokenMat = _renderer.materials[1];
            _renderer.SetBlendShapeWeight(_dentBlendKey, 0f);
            _renderer.SetBlendShapeWeight(_breakBlendKey, 0f);

            DisableCracks();
            DisableAllDebris();
            //HandleDebrisActivation(_currentState);

            DoorEvents.OnUpdateState += HandleStateChange;
            DoorEvents.OnBreak += HandleDoorBreak;
        }

        private void OnDisable()
        {
            DoorEvents.OnUpdateState -= HandleStateChange;
            DoorEvents.OnBreak -= HandleDoorBreak;
        }

        private void HandleStateChange()
        {
            // There are five states for this door
            if (!_isCracked)
            {
                EnableCracks();
                _isCracked = true;
            }

            if (_door.CurrentHealth < _door.MaxHealth * 0.9f && _door.CurrentHealth >= _door.MaxHealth * 0.75f)
                _currentState = Enums.DoorState.Dent_1;
            else if (_door.CurrentHealth < _door.MaxHealth * 0.75f && _door.CurrentHealth >= _door.MaxHealth * 0.5f)
                _currentState = Enums.DoorState.Dent_2;
            else if (_door.CurrentHealth < _door.MaxHealth * 0.5f && _door.CurrentHealth >= _door.MaxHealth * 0.25f)
                _currentState = Enums.DoorState.Break_1;
            else if (_door.CurrentHealth < _door.MaxHealth * 0.25f)
                _currentState = Enums.DoorState.Break_2;
            else
                _currentState = Enums.DoorState.Solid;

            UpdateState(_currentState);
        }

        private void UpdateState(Enums.DoorState doorState)
        {
            switch (doorState)
            {
                case Enums.DoorState.Solid:
                    _renderer.SetBlendShapeWeight(_dentBlendKey, 0f);
                    _renderer.SetBlendShapeWeight(_breakBlendKey, 0f);
                    break;
                case Enums.DoorState.Dent_1:
                    _renderer.SetBlendShapeWeight(_dentBlendKey, 50f);
                    debris_1.SetActive(true);
                    break;
                case Enums.DoorState.Dent_2:
                    _renderer.SetBlendShapeWeight(_dentBlendKey, 100f);
                    debris_2.SetActive(true);
                    break;
                case Enums.DoorState.Break_1:
                    _renderer.SetBlendShapeWeight(_breakBlendKey, 50f);
                    debris_3.SetActive(true);
                    break;
                case Enums.DoorState.Break_2:
                    _renderer.SetBlendShapeWeight(_breakBlendKey, 100f);
                    debris_4.SetActive(true);
                    break;
            }

            //HandleDebrisActivation(doorState);
        }

        private void EnableCracks() => _brokenMat.color = new Color(1f, 1f, 1f, 1f);
        private void DisableCracks() => _brokenMat.color = new Color(1f, 1f, 1f, 0f);
        private void DisableAllDebris()
        {
            debris_1.SetActive(false);
            debris_2.SetActive(false);
            debris_3.SetActive(false);
            debris_4.SetActive(false);
            finalDebris.SetActive(false);
        }
        private void HandleDebrisActivation(Enums.DoorState doorState)
        {
            switch (doorState)
            {
                case Enums.DoorState.Solid:
                    DisableAllDebris();
                    break;
                case Enums.DoorState.Dent_1:
                    break;
                case Enums.DoorState.Dent_2:
                    break;
                case Enums.DoorState.Break_1:
                    break;
                case Enums.DoorState.Break_2:
                    break;
            }
        }
        private void HandleDoorBreak()
        {
            _renderer.enabled = false;
            finalDebris.SetActive(true);
        }
    }
}
