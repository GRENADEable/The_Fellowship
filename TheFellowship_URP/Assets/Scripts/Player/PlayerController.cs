using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Serialized Variables

    #region Datas
    [Space, Header("Datas")]
    [SerializeField]
    [Tooltip("GameManager Scriptable Object")]
    private GameManagerData gmData;

    [SerializeField]
    [Tooltip("PlayerStats Scriptable Object")]
    private PlayerStatsData plyStatsData;
    #endregion

    [Space, Header("Player Stats")]
    [SerializeField]
    [Tooltip("Starting stamina of the Player")]
    private float startingStamina = 30f;
    #endregion

    #region Private Variables
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private float _currStamina;
    private float _currSpeed;
    private CharacterController2D _charController;
    private float _horizontal;
    private Vector2 _moveDirection;
    private bool _isJumping = false;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        _staminaSlider = GameObject.FindGameObjectWithTag("Stamina_Slider").GetComponent<Slider>();
    }

    void OnDisable()
    {
        _staminaSlider = null;
    }
    #endregion

    void Start()
    {
        _charController = GetComponent<CharacterController2D>();
        Intialise();
    }

    void Update()
    {
        if (gmData.currGameState == GameManagerData.GameState.Game)
            PlayerMovement();
        else
            _charController.Move(0, false, false);
    }
    #endregion

    #region My Functions
    /// <summary>
    /// Intialise variables from ScriptableObject Data;
    /// </summary>
    void Intialise()
    {
        _currSpeed = plyStatsData.speed;
        _currStamina = startingStamina;
        _staminaSlider.value = _currStamina;
    }

    /// <summary>
    /// Function to move the Player on one Axis;
    /// </summary>
    void PlayerMovement()
    {
        _moveDirection = new Vector2(_horizontal, 0f).normalized;
        _charController.Move(_moveDirection.x * Time.fixedDeltaTime * _currSpeed, false, _isJumping);
        _isJumping = false;
    }
    #endregion

    #region Events

    #region Input Systems
    /// <summary>
    /// Tied to Player Input event for movement;
    /// </summary>
    /// <param name="context"> Callback context for checking if the input presed state; </param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed && gmData.currGameState == GameManagerData.GameState.Game)
            _horizontal = context.ReadValue<Vector2>().x;
        else if (context.canceled)
            _horizontal = 0f;
    }

    /// <summary>
    /// Tied to Player Input event for jumping;
    /// </summary>
    /// <param name="context"> Callback context for checking if the input presed state; </param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && gmData.currGameState == GameManagerData.GameState.Game)
            _isJumping = true;
    }
    #endregion

    #endregion
}