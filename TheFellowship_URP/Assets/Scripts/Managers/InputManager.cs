using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Public Variables
    public static PlayerControlsAsset inputActions = new PlayerControlsAsset();
    public static event Action<InputActionMap> OnActionMapChange;
    #endregion

    #region Unity Callbacks
    void Start() => ToggleActionMap(inputActions.Player);
    #endregion

    #region My Functions
    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
            return;

        inputActions.Disable();
        OnActionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }
    #endregion
}