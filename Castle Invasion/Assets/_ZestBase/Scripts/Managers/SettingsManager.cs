using UnityEngine;

namespace ZestGames
{
    public class SettingsManager : MonoBehaviour
    {
        private GameManager _gameManager;

        public static bool SoundOn { get; private set; }
        public static bool VibrationOn { get; private set; }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            SoundOn = VibrationOn = false;
        }
    }
}
