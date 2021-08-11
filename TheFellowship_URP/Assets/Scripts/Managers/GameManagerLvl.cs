using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManagerLvl : MonoBehaviour
{
    #region Serialzed Variables

    #region Datas
    [Space, Header("Datas")]
    [SerializeField]
    [Tooltip("GameManager Scriptable Object")]
    private GameManagerData gmData;

    [SerializeField]
    [Tooltip("PlayerStats Scriptable Object")]
    private PlayerStatsData[] plyStatDatas;
    #endregion

    #region Panels
    [Space, Header("Panels")]
    [SerializeField]
    [Tooltip("Add your pause panel GameObject here")]
    private GameObject pausePanel;

    [SerializeField]
    [Tooltip("Fade Background with Animator Component")]
    private Animator fadeBG;
    #endregion

    #region Selection Panel
    [Space, Header("Selection Panel")]
    [SerializeField]
    [Tooltip("Player selection panel GameObject")]
    private GameObject choosePanel;

    [SerializeField]
    [Tooltip("Total players you can select")]
    private int totalSelectedPlayers = 9;

    [SerializeField]
    [Tooltip("Player selection Button Prefab")]
    private GameObject buttonPrefab;

    [SerializeField]
    [Tooltip("Player selection Button spawn point")]
    private Transform buttonPos;

    [SerializeField]
    [Tooltip("Player selection start Button")]
    private Button startButton;
    #endregion

    #region Game Session
    [Space, Header("Game Session")]
    [SerializeField]
    [Tooltip("Trasnform component for spawning players")]
    private Transform playerSpawn;
    #endregion

    #region Switch Player Panel
    [Space, Header("Switch Player Panel")]
    [SerializeField]
    [Tooltip("Player switch panel GameObject")]
    private GameObject switchPanel;

    [SerializeField]
    [Tooltip("Player switch Button Prefab")]
    private GameObject switchButtonPrefab;

    [SerializeField]
    [Tooltip("Player switch Button spawn point")]
    private Transform switchButtonPos;
    #endregion

    #region Pause UI
    [Space, Header("Pause UI")]
    [SerializeField]
    [Tooltip("All the Buttons in the pause panel")]
    private Button[] gameButtons;

    [SerializeField]
    [Tooltip("First Button to be highlighted when the game is paused")]
    private GameObject firstSelectedPauseButton;
    #endregion

    #region Stamina UI
    [Space, Header("Stamina UI")]
    [SerializeField]
    [Tooltip("Stamina Slider Prefab")]
    private GameObject staminaPrefab;
    #endregion

    public delegate void SendEventsObj(GameObject obj);
    public static event SendEventsObj OnCharSpawn;
    public static event SendEventsObj OnSliderSpawn;
    #endregion

    #region Private Variables
    private List<PlayerStatsData> _currCharSelection = new List<PlayerStatsData>();
    private List<Button> _chooseButtons = new List<Button>();
    private List<Button> _switchButtons = new List<Button>();
    private int _currSelectedPlayers;
    private PlayerInput _plyInput;
    private GameObject _currActivePlayer;
    private GameObject _currActivePlayerButton;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        InputManager.inputActions.Player.Pause.performed += OnPause;
        InputManager.inputActions.Player.Pause.performed += OnSwitch;

        SelectionButton.OnPlayerSelected += OnPlayerSelectedEventReceived;
        SwitchButton.OnPlayerSwitchSelected += OnPlayerSwitchSelectedEventReceived;
    }

    void OnDisable()
    {
        InputManager.inputActions.Player.Pause.performed -= OnPause;
        InputManager.inputActions.Player.Pause.performed -= OnSwitch;

        SelectionButton.OnPlayerSelected -= OnPlayerSelectedEventReceived;
        SwitchButton.OnPlayerSwitchSelected -= OnPlayerSwitchSelectedEventReceived;
    }

    void OnDestroy()
    {
        InputManager.inputActions.Player.Pause.performed -= OnPause;
        InputManager.inputActions.Player.Pause.performed -= OnSwitch;

        SelectionButton.OnPlayerSelected -= OnPlayerSelectedEventReceived;
        SwitchButton.OnPlayerSwitchSelected -= OnPlayerSwitchSelectedEventReceived;
    }
    #endregion

    void Start()
    {
        fadeBG.Play("Fade_In");
        gmData.ChangeGameState("Intro");
        _plyInput = GetComponent<PlayerInput>();
        IntialiseSelectionButtons();
    }

    void Update()
    {

    }
    #endregion

    #region My Functions

    #region Player Selection
    /// <summary>
    /// Tied to Reset_Selection_Button Button;
    /// This just resets the selection if you want to rechoose the players;
    /// </summary>
    public void OnClick_ResetSelection()
    {
        _currSelectedPlayers = 0;

        for (int i = 0; i < _chooseButtons.Count; i++)
            _chooseButtons[i].interactable = true;

        _currCharSelection.Clear();
        startButton.interactable = false;
    }

    /// <summary>
    /// Intialises the selection buttons depending on what PlayerStatsData Scriptable Object is being used in the array;
    /// </summary>
    void IntialiseSelectionButtons()
    {
        for (int i = 0; i < plyStatDatas.Length; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonPos.position, Quaternion.identity, buttonPos);
            buttonObj.name = $"{plyStatDatas[i].playerName}_Button";

            SelectionButton select = buttonObj.GetComponent<SelectionButton>();
            select.buttonText.text = plyStatDatas[i].playerName;
            select.index = plyStatDatas[i].index;

            _chooseButtons.Add(buttonObj.GetComponent<Button>());

        }
    }
    #endregion

    #region Pause Panel
    /// <summary>
    /// Tied to Resume_Button;
    /// This just resumes the game when clicked;
    /// </summary>
    public void OnClick_ResumeGame()
    {
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
        gmData.ChangeGameState("Game");
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Tied to Return_Map_Button;
    /// This just goes to the map scene when clicked;
    /// </summary>
    public void OnClick_Map()
    {
        DisableGameButtons();
    }

    /// <summary>
    /// Tied to Menu_Button;
    /// This just goes to the menu scene when clicked;
    /// </summary>
    public void OnClick_Menu()
    {
        StartCoroutine(MenuDelay());
        DisableGameButtons();
    }

    /// <summary>
    /// Disables interaction on all the buttons to avoid extra clicks when fading out;
    /// </summary>
    void DisableGameButtons()
    {
        for (int i = 0; i < gameButtons.Length; i++)
            gameButtons[i].interactable = false;
    }
    #endregion

    #region Game Session
    /// <summary>
    /// Tied to Start_Button Button;
    /// Disables choose panel and intialises the game on runtime;
    /// </summary>
    public void OnClick_StartGame()
    {
        GameObject charObj = Instantiate(_currCharSelection[0].playerPrefab, playerSpawn.position, Quaternion.identity);
        OnCharSpawn?.Invoke(charObj);
        _currActivePlayer = charObj;

        OnSliderSpawn?.Invoke(staminaPrefab);

        choosePanel.SetActive(false);
        gmData.ChangeGameState("Game");
        _plyInput.enabled = true;
        SelectedPlayers();
    }
    #endregion

    #region Player Switch
    /// <summary>
    /// Button tied to Back_Button Button;
    /// Disables the switch GameObject panel;
    /// </summary>
    public void OnClick_SwitchPanelBack()
    {
        switchPanel.SetActive(false);
        gmData.ChangeGameState("Game");
    }

    /// <summary>
    /// This just shows the player buttons the players can switch. 
    /// This depends on what the player selected on the start of the player selection screen;
    /// </summary>
    void SelectedPlayers()
    {
        int index = 0;

        for (int i = 0; i < _currCharSelection.Count; i++)
        {
            GameObject buttonSwitchObj = Instantiate(switchButtonPrefab, switchButtonPos.position, Quaternion.identity, switchButtonPos);
            buttonSwitchObj.name = $"{_currCharSelection[i].playerName}_Button";

            _switchButtons.Add(buttonSwitchObj.GetComponent<Button>());

            SwitchButton switchPlayer = buttonSwitchObj.GetComponent<SwitchButton>();
            switchPlayer.switchButtonText.text = _currCharSelection[i].playerName;
            switchPlayer.switchIndex = index;
            index++;
        }

        _currActivePlayerButton = _switchButtons[0].gameObject;
    }

    /// <summary>
    /// This spawns the new player according to the Index;
    /// </summary>
    /// <param name="index"> Index used to access the selected players in the list of Scriptable Objects; </param>
    void SwitchToNewPlayer(int index)
    {
        GameObject charObj = Instantiate(_currCharSelection[index].playerPrefab, playerSpawn.position, Quaternion.identity);
        OnCharSpawn?.Invoke(charObj);
        _currActivePlayer = charObj;
        _currActivePlayerButton = _switchButtons[index].gameObject;

        OnSliderSpawn?.Invoke(staminaPrefab);

    }
    #endregion

    #endregion

    #region Events

    #region Input Systems
    /// <summary>
    /// Tied to Manager GameObject Player Input event for pausing;
    /// </summary>
    /// <param name="context"> Callback context for checking if the input presed state; </param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && gmData.currGameState == GameManagerData.GameState.Game)
        {
            gmData.ChangeGameState("Paused");
            pausePanel.SetActive(true);

            InputManager.ToggleActionMap(InputManager.inputActions.UI);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedPauseButton);
        }
    }

    /// <summary>
    /// Tied to Manager GameObject Player Input event for switching;
    /// </summary>
    /// <param name="context"> Callback context for checking if the input presed state; </param>
    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.started && gmData.currGameState == GameManagerData.GameState.Game)
        {
            gmData.ChangeGameState("PlayerSwap");
            switchPanel.SetActive(true);

            InputManager.ToggleActionMap(InputManager.inputActions.UI);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_currActivePlayerButton);
        }
    }
    #endregion

    /// <summary>
    /// Subbed to event from SelectionButton Script;
    /// This just adds the appropiate scriptable object by using the index as reference;
    /// </summary>
    /// <param name="index"> Index used to add the scriptable object to a list </param>
    void OnPlayerSelectedEventReceived(int index)
    {
        _currCharSelection.Add(plyStatDatas[index]);
        _currSelectedPlayers++;

        if (_currSelectedPlayers >= totalSelectedPlayers)
        {
            startButton.interactable = true;

            for (int i = 0; i < _chooseButtons.Count; i++)
                _chooseButtons[i].interactable = false;
        }
    }

    /// <summary>
    /// Subbed to event from SwitchButton Scriptl
    /// This just selects the player according to the index;
    /// </summary>
    /// <param name="index"> Index used to check the scriptable object list </param>
    void OnPlayerSwitchSelectedEventReceived(int index, GameObject obj)
    {
        Destroy(_currActivePlayer);
        _currActivePlayer = null;

        SwitchToNewPlayer(index);

        switchPanel.SetActive(false);
        gmData.ChangeGameState("Game");
    }
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