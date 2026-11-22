using System;

public static class GameEvents
{
    public static event Action<int, int> GameStarted;     // max health, goal
    public static event Action<int, int> PlayerDamaged;   // current, max
    public static event Action<int, int> OrbCollected;    // score, goal
    public static event Action GameWon;
    public static event Action GameLost;

    public static void RaiseGameStarted(int maxHealth, int goal) { GameStarted?.Invoke(maxHealth, goal); }
    public static void RaisePlayerDamaged(int current, int max) { PlayerDamaged?.Invoke(current, max); }
    public static void RaiseOrbCollected(int score, int goal) { OrbCollected?.Invoke(score, goal); }
    public static void RaiseGameWon() { GameWon?.Invoke(); }
    public static void RaiseGameLost() { GameLost?.Invoke(); }
}