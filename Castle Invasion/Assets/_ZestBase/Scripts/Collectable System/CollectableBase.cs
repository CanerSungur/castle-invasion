using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace ZestGames
{
    public abstract class CollectableBase : MonoBehaviour, ICollectable
    {
        [SerializeField] private int value;
        [SerializeField, Tooltip("Is it in scene or is it rewarded when something happens.")] protected CollectableStyle collectableStyle;
        [SerializeField, Tooltip("Select movement type of this object when it's collected.")] protected CollectStyle collectStyle;
        [SerializeField] protected GameObject collectEffect;
        protected Collider coll;
        private float _bounceDuration = 0.3f;

        public int Value => value;

        public virtual void Collect()
        {
            if (collectEffect)
            {
                ParticleSystem ps = Instantiate(collectEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                ps.Play();
            }

            Apply();
            Dispose();
        }

        public abstract void Apply();

        public void Dispose()
        {
            if (collectStyle == CollectStyle.OnSite)
            {
                Bounce();
                StartCoroutine(DestroyWithDelay(_bounceDuration));
            }
        }

        private void Bounce() => transform.DOScale(0, _bounceDuration).SetEase(Ease.InBounce);

        private IEnumerator DestroyWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            transform.DOKill();
            Destroy(gameObject);
        }
    }
}

public enum CollectStyle { OnSite, MoveToUi }
public enum CollectableStyle { Collect, Reward }