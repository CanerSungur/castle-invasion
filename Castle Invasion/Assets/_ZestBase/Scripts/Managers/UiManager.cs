using UnityEngine;

namespace ZestGames
{
    public class UiManager : MonoBehaviour
    {
        private GameManager _gameManager;

        [Header("-- REFERENCES --")]
        [SerializeField] private TouchToStart touchToStart;
        [SerializeField] private Hud hud;
        [SerializeField] private LevelFail levelFail;
        [SerializeField] private LevelSuccess levelSuccess;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            hud.Init(this);
            levelFail.Init(this);
            levelSuccess.Init(this);
        }
    }
}
