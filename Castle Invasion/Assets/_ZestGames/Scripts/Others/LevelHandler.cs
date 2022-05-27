using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZestGames
{
    public static class LevelHandler
    {
        public static int Level { get; private set; }
        private static int _currentLevel, _lastSceneBuildIndex;

        public static int GetSceneBuildIndexToBeLoaded()
        {
            Level = PlayerPrefs.GetInt("Level", 1);
            #region For Incrementals
            //PlayerPrefs.SetInt("ResetStaminaForNewLevel", 0);
            //PlayerPrefs.Save();
            #endregion

            // Uncomment this and run game once to reset level.
            //DeleteLevelData();

            _lastSceneBuildIndex = SceneManager.sceneCountInBuildSettings - 1;
            int index = Level % _lastSceneBuildIndex;
            if (index == 0)
                _currentLevel = _lastSceneBuildIndex;
            else
                _currentLevel = index;

            return _currentLevel;
        }

        public static void IncreaseLevel(LevelManager levelManager)
        {
            // Reset upgrade levels for current level.
            DataManager.ResetUpgradesForCurrentLevel();

            Level++;
            PlayerPrefs.SetInt("Level", Level);
            //PlayerPrefs.SetInt("ResetStaminaForNewLevel", 1);
            PlayerPrefs.Save();
        }
    }
}
