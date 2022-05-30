using UnityEngine;
using ZestCore.Utility;
using ZestGames;

namespace CastleInvasion
{
    public class MoneyFeedbackHandler : MonoBehaviour
    {
        [SerializeField] private Transform doorTransform;

        private void Start()
        {
            FeedbackEvents.OnGiveMoneyFeedback += ActivateFeedback;
        }

        private void OnDisable()
        {
            FeedbackEvents.OnGiveMoneyFeedback -= ActivateFeedback;
        }

        private void ActivateFeedback(float lastEarnedMoney)
        {
            MoneyFeedback moneyFeedback = ObjectPooler.Instance.SpawnFromPool(Enums.PoolStamp.MoneyFeedback, doorTransform.position + (Vector3.up * 1.75f) + (doorTransform.forward * 3.2f), Quaternion.identity).GetComponent<MoneyFeedback>();
            moneyFeedback.SetMoneyText(lastEarnedMoney);
            Delayer.DoActionAfterDelay(this, 2f, () => moneyFeedback.gameObject.SetActive(false));
        }
    }
}
