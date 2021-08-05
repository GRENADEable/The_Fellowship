using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManagerMenu : MonoBehaviour
{
    #region Serialzed Variables
    [Space, Header("Datas")]
    [SerializeField] private GameManagerData gmData;

    [Space, Header("UI")]
    [SerializeField] private GameObject[] firstSelectedButtons;
    [SerializeField] private Button[] buttons;
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
    /// Button tied with Level_Buttons;
    /// Starts the level with a delay;
    /// </summary>
    public void OnClick_StartGame(int index)
    {
        StartCoroutine(StartDelay(index));
        DisableButtons();
    }

    /// <summary>
    /// Button tied with Exit_Button;
    /// Exits the game with a delay;
    /// </summary>
    public void OnClick_ExitGame()
    {
        StartCoroutine(ExitDelay());
        DisableButtons();
    }

    /// <summary>
    /// Disables all the menu buttons;
    /// </summary>
    void DisableButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = false;
    }

    /// <summary>
    /// Highlights the selected button when user switches panels;
    /// </summary>
    /// <param name="index"> Index for which button to highlight in the array; </param>
    public void OnClick_HighlightedButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButtons[index]);
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Switches to new scene after delay;
    /// </summary>
    /// <param name="index"> Must have Int for scene Index; </param>
    /// <returns> Float delay for yield; </returns>
    IEnumerator StartDelay(int index)
    {
        fadeBG.Play("Fade_Out");
        yield return new WaitForSeconds(1f);
        gmData.ChangeMap(index);
    }

    /// <summary>
    /// Quits Game;
    /// </summary>
    /// <returns> Float delay for yield; </returns>
    IEnumerator ExitDelay()
    {
        fadeBG.Play("Fade_Out");
        yield return new WaitForSeconds(1f);
        gmData.Quit();
        Debug.Log("Game Exited");
    }
    #endregion
}