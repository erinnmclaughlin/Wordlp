namespace Wordlp.Models;

public enum GameKey
{
    A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,
    Backspace,
    Enter
}

public static class GameKeys
{
    public static readonly List<GameKey> Letters = new() 
    {
        GameKey.A,
        GameKey.B,
        GameKey.C,
        GameKey.D,
        GameKey.E,
        GameKey.F,
        GameKey.G,
        GameKey.H,
        GameKey.I,
        GameKey.J,
        GameKey.K,
        GameKey.L,
        GameKey.M,
        GameKey.N,
        GameKey.O,
        GameKey.P,
        GameKey.Q,
        GameKey.R,
        GameKey.S,
        GameKey.T,
        GameKey.U,
        GameKey.V,
        GameKey.W,
        GameKey.X,
        GameKey.Y,
        GameKey.Z
    };

    public static bool IsLetter(this GameKey gameKey) => Letters.Contains(gameKey);
}