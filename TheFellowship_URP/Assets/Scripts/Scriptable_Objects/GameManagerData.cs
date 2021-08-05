using UnityEngine;

[CreateAssetMenu(fileName = "GameManager_Data", menuName = "Manager/GameManager Data")]
public class GameManagerData : ScriptableObject
{
    #region Public Variables
    [Space, Header("Enums")]
    public GameState currGameState = GameState.Game;
    public enum GameState { Menu, Intro, Game, Paused, PlayerSwap, Dead };
    #endregion

    #region My Functions
    public void ChangeGameState(string state)
    {
        if (state == "Menu")
            currGameState = GameState.Menu;

        if (state == "Intro")
            currGameState = GameState.Intro;

        if (state == "Game")
            currGameState = GameState.Game;

        if (state == "Paused")
            currGameState = GameState.Paused;

        if (state == "PlayerSwap")
            currGameState = GameState.PlayerSwap;

        if (state == "Dead")
            currGameState = GameState.Dead;
    }
    #endregion
}