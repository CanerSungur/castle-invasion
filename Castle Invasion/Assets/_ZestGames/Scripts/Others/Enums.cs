namespace ZestGames
{
    public class Enums
    {
        public enum GameState { WaitingToStart, Started, PlatrofmEnded, GameEnded }
        public enum GameEnd { None, Success, Fail }
        public enum PoolStamp { Something, HitSmokePS, RamRow, LeftRowSoldier, RightRowSoldier, MoneyFeedback, RamRowTip, RamRowEnd }
        public enum AudioType { Testing_PlayerMove, Button_Click }
        public enum DoorState { Solid, Dent_1, Dent_2, Break_1, Break_2 }
    }
}
