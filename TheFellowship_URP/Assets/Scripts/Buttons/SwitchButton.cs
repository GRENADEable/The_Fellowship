using UnityEngine;
using TMPro;

public class SwitchButton : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public int switchIndex;
    [Tooltip("Text in the Button")] public TextMeshProUGUI switchButtonText;

    public delegate void SendEventIntObject(int index, GameObject obj);
    /// <summary>
    /// Event sent from SwitchButton to GameManagerLvl Script;
    /// This event just sends the index that can be used for referring the List of scriptable objects;
    /// </summary>
    public static event SendEventIntObject OnPlayerSwitchSelected;
    #endregion

    #region Unity Callbacks
    void Start()
    {

    }

    void Update()
    {

    }
    #endregion

    #region My Functions
    public void OnClick_SwitchPlayer() => OnPlayerSwitchSelected?.Invoke(switchIndex, gameObject);
    #endregion
}