using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerMenu : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Serialzed Variables
    [SerializeField] private Button[] menuButtons;

    [SerializeField] private Animator fadeBG;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Callbacks
    void Start()
    {
        fadeBG.Play("Fade_In");
    }

    void Update()
    {

    }
    #endregion

    #region My Functions
    /// <summary>
    /// Button tied with Start_Button;
    /// Starts the game with a delay;
    /// </summary>
    public void OnClick_StartGame()
    {
        StartCoroutine(StartDelay());
        DisableButtons();
    }

    /// <summary>
    /// Disables all the menu buttons;
    /// </summary>
    void DisableButtons()
    {
        for (int i = 0; i < menuButtons.Length; i++)
            menuButtons[i].interactable = false;
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Switches to new scene after delay;
    /// </summary>
    IEnumerator StartDelay()
    {
        fadeBG.Play("Fade_Out");
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(1);
        //fadeBG.Play("Fade_In");
    }
    #endregion
}