using UnityEngine;

namespace ZestGames
{
    public interface IAiMovement
    {
        public void Motor();
        public bool IsMoving { get; }
        public bool IsGrounded { get; }
    }
}
