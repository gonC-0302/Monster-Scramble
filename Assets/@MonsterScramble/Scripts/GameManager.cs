using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public enum GameState
{
    Matching,
    Battle,
    Finish
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameState _currentGameState;
    public GameState CurrentGameState => _currentGameState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartBattle()
    {
        SwitchGameState(GameState.Battle);
        UIManager.instance.PlayStartLogoAnimation();
    }

    public void SwitchGameState(GameState nextState)
    {
        _currentGameState = nextState;
    }
}
