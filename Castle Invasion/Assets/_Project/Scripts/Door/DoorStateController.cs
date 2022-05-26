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

        //[Header("-- DEBRIS SETUP --")]
        //[SerializeField] private GameObject debris

        public void Init(Door door)
        {
            _door = door;
            _currentState = Enums.DoorState.Solid;

            _renderer = GetComponent<SkinnedMeshRenderer>();
            _brokenMat = _renderer.materials[1];
            _renderer.SetBlendShapeWeight(_dentBlendKey, 0f);
            _renderer.SetBlendShapeWeight(_breakBlendKey, 0f);

            DisableCracks();

            DoorEvents.OnUpdateState += HandleStateChange;
        }

        private void OnDisable()
        {
            DoorEvents.OnUpdateState -= HandleStateChange;
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
                    break;
                case Enums.DoorState.Dent_2:
                    _renderer.SetBlendShapeWeight(_dentBlendKey, 100f);
                    break;
                case Enums.DoorState.Break_1:
                    _renderer.SetBlendShapeWeight(_breakBlendKey, 50f);
                    break;
                case Enums.DoorState.Break_2:
                    _renderer.SetBlendShapeWeight(_breakBlendKey, 100f);
                    break;
            }
        }

        private void EnableCracks() => _brokenMat.color = new Color(1f, 1f, 1f, 1f);
        private void DisableCracks() => _brokenMat.color = new Color(1f, 1f, 1f, 0f);
    }
}
