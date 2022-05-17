using UnityEngine;

namespace ZestGames
{
    public class TapInput : MonoBehaviour
    {
        private float _startTime;
        private float _delay = 0.5f;

        public bool CanTap => GameManager.GameState == Enums.GameState.Started && Time.time > _startTime + _delay;

        private void Start()
        {
            GameEvents.OnGameStart += () => _startTime = Time.time;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= () => _startTime = Time.time;
        }

        private void Update()
        {
            if (!CanTap) return;

            if (Input.GetMouseButtonDown(0)) InputEvents.OnTapHappened?.Invoke();
        }
    }
}
