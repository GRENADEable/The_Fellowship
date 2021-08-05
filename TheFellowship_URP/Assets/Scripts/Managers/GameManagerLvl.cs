using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerLvl : MonoBehaviour
{
    #region Serialzed Variables
    [Space, Header("Datas")]
    [SerializeField] private GameManagerData gmData;


    [Space, Header("UI")]
    //[SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private Animator fadeBG;
    #endregion

    #region Private Variables
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        InputManager.inputActions.Player.Pause.performed += OnPause;
    }

    void OnDisable()
    {
        InputManager.inputActions.Player.Pause.performed -= OnPause;
    }

    void OnDestroy()
    {
        InputManager.inputActions.Player.Pause.performed -= OnPause;
    }
    #endregion

    void Start()
    {
        fadeBG.Play("Fade_In");
        gmData.ChangeGameState("Game");
    }

    void Update()
    {

    }
    #endregion

    #region My Functions
    public void OnClick_ResumeGame()
    {
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
        gmData.ChangeGameState("Game");
        pausePanel.SetActive(false);
    }
    #endregion

    #region Events

    #region Input Systems
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && gmData.currGameState == GameManagerData.GameState.Game)
        {
            gmData.ChangeGameState("Paused");
            //hudPanel.SetActive(false);
            pausePanel.SetActive(true);
            InputManager.ToggleActionMap(InputManager.inputActions.UI);
        }
    }
    #endregion

    #endregion

    #region Coroutines

    #endregion
}