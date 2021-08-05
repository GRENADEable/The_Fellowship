using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManagerLvl : MonoBehaviour
{
    #region Serialzed Variables
    [Space, Header("Datas")]
    [SerializeField] private GameManagerData gmData;

    [Space, Header("Panels")]
    //[SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Animator fadeBG;

    [Space, Header("Buttons")]
    [SerializeField] private Button[] gameButtons;
    [SerializeField] private GameObject firstSelectedPauseButton;

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

    #region Pause Panel
    public void OnClick_ResumeGame()
    {
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
        gmData.ChangeGameState("Game");
        pausePanel.SetActive(false);
    }

    public void OnClick_Map()
    {
        DisableGameButtons();
    }
    public void OnClick_Menu()
    {
        StartCoroutine(MenuDelay());
        DisableGameButtons();
    }
    #endregion

    void DisableGameButtons()
    {
        for (int i = 0; i < gameButtons.Length; i++)
            gameButtons[i].interactable = false;
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
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedPauseButton);
        }
    }
    #endregion

    #endregion

    #region Coroutines
    /// <summary>
    /// Quits to menu;
    /// </summary>
    /// <returns> Float delay for yield; </returns>
    IEnumerator MenuDelay()
    {
        fadeBG.Play("Fade_Out");
        yield return new WaitForSeconds(1f);
        gmData.ChangeMap(0);
    }
    #endregion
}