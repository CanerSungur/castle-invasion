using UnityEngine;

namespace ZestGames
{
    public class CollectableExample : CollectableBase
    {
        private CollectableMovement movement;

        private void OnEnable()
        {
            if (TryGetComponent(out coll))
                coll.enabled = true;

            if (TryGetComponent(out movement))
            {
                movement.Init(this);
                if (collectableStyle == CollectableStyle.Reward)
                    movement.TriggerRewardMovement();
            }
        }

        public override void Apply()
        {
            if (coll) coll.enabled = false;

            // Play Audio

            if (collectStyle == CollectStyle.OnSite)
            {
                // collect instantly
            }
            else if (collectStyle == CollectStyle.MoveToUi)
            {
                if (movement)
                {
                    movement.OnStartMovement?.Invoke();
                }
                else
                {
                    // collect instantly

                }
            }
        }
    }
}
