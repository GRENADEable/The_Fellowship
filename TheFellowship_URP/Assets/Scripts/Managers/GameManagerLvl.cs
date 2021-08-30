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
    [Tooltip("Transform component for spawning players")]
    private Transform playerSpawn;

    //[SerializeField]
    //[Tooltip("")]
    //private Slider staminaSlider;

    //[SerializeField]
    //[Tooltip("")]
    //private float startingStamina = 30f;
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

    #region Events
    public delegate void SendEventsObj(GameObject obj);
    /// <summary>
    /// Event sent from GameMangerLvl to CamFollow Script;
    /// This just sends the GameObject of the player to refer at the Cinemachine Camera;
    /// </summary>
    public static event SendEventsObj OnCharSpawn;
    #endregion

    #endregion

    #region Private Variables
    [Header("Switch Player Panel")]
    [SerializeField] private List<PlayerStatsData> _currCharSelection = new List<PlayerStatsData>();
    private List<Button> _chooseButtons = new List<Button>();
    private List<Button> _switchButtons = new List<Button>();

    private int _currSelectedPlayers;
    private PlayerInput _plyInput;

    [Header("UI")]
    private GameObject _currActivePlayer;
    private GameObject _currActivePlayerButton;

    [Header("Player References")]
    private Transform _currPlayerPos;
    private List<GameObject> _spawnedCharObjs = new List<GameObject>();
    //[SerializeField] private int _currPlayerIndex;

    //[Header("Player Stamina")]
    //[SerializeField] private List<float> _currStaminas = new List<float>();
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
        //if (gmData.currGameState == GameManagerData.GameState.Game)
        //    StaminaCheck();
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
        for (int i = 0; i < _currCharSelection.Count; i++)
        {
            GameObject charObj = Instantiate(_currCharSelection[i].playerPrefab, playerSpawn.position, Quaternion.identity);
            _spawnedCharObjs.Add(charObj);
            _spawnedCharObjs[i].SetActive(false);
            //_currStaminas.Add(startingStamina);
        }

        _spawnedCharObjs[0].SetActive(true);
        _currPlayerPos = _spawnedCharObjs[0].transform;
        OnCharSpawn?.Invoke(_spawnedCharObjs[0]);
        _currActivePlayer = _spawnedCharObjs[0];
        //_currPlayerIndex = 0;

        choosePanel.SetActive(false);
        gmData.ChangeGameState("Game");
        _plyInput.enabled = true;

        SelectedPlayers();
        //StaminaSwitch();
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
        for (int i = 0; i < _spawnedCharObjs.Count; i++)
        {
            _spawnedCharObjs[i].SetActive(false);
            _spawnedCharObjs[i].transform.position = _currPlayerPos.position;
        }

        _spawnedCharObjs[index].SetActive(true);
        OnCharSpawn?.Invoke(_spawnedCharObjs[index]);

        _currActivePlayer = _spawnedCharObjs[index];
        _currActivePlayerButton = _switchButtons[index].gameObject;
        _currPlayerPos = _spawnedCharObjs[index].transform;
    }
    #endregion

    #region Player Stamina
    //void StaminaCheck()
    //{
    //    if (_spawnedCharObjs[_currPlayerIndex].activeInHierarchy)
    //    {
    //        _currStaminas[_currPlayerIndex] -= Time.deltaTime;
    //        staminaSlider.value = _currStaminas[_currPlayerIndex];
    //    }
    //    else
    //    {
    //        _currStaminas[_currPlayerIndex] += Time.deltaTime;
    //    }
    //}

    //void StaminaSwitch()
    //{
    //    staminaSlider.value = _currStaminas[_currPlayerIndex];
    //}
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
        SwitchToNewPlayer(index);
        //_currPlayerIndex = index;
        //StaminaSwitch();

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