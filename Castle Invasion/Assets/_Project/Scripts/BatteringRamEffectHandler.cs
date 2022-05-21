using UnityEngine;
using ZestGames;

namespace CastleInvasion
{
    public class BatteringRamEffectHandler : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private ParticleSystem smokePS;

        private void Start()
        {
            PlayerEvents.OnHitWall += SmokeParticles;
        }

        private void OnDisable()
        {
            PlayerEvents.OnHitWall -= SmokeParticles;
        }

        private void SmokeParticles()
        {
            ParticleSystem ps = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.HitSmokePS, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            ps.Play();
        }
    }
}
