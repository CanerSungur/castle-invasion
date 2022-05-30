using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace CastleInvasion
{
    public class Fog : MonoBehaviour
    {
        private MeshRenderer _mesh;

        private void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            DisableFog();
            
            GameEvents.OnGameStart += EnableFogWithDelay;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= EnableFogWithDelay;
        }

        private void EnableFogWithDelay() => Delayer.DoActionAfterDelay(this, 1f, EnableFog);
        private void EnableFog() => _mesh.enabled = true;
        private void DisableFog() => _mesh.enabled = false;
    }
}
