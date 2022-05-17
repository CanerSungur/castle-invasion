using UnityEngine;
using DG.Tweening;

namespace CastleInvasion
{
    public class Door : MonoBehaviour
    {
        public void GetHit()
        {
            Shake();
        }

        private void Shake()
        {
            transform.DOShakeScale(.25f, .25f);
            transform.DOShakeRotation(.25f, .25f);
        }
    }
}
