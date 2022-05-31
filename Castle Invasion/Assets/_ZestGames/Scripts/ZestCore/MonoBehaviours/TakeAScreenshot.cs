using UnityEngine;
using ZestCore.Utility;

namespace CastleInvasion
{
    public class TakeAScreenshot : MonoBehaviour
    {
        private void Update()
        {
            Screenshot.TakeAScreenshot();
        }
    }
}
