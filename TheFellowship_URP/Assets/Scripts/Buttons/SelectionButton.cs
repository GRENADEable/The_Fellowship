using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionButton : MonoBehaviour
{
    #region Public Variables
    /*[HideInInspector]*/ public int index;
    [Tooltip("Text in the Button")] public TextMeshProUGUI buttonText;

    public delegate void SendEventInt(int index);
    /// <summary>
    /// Event sent from SelectionButton to GameManagerLvl Script;
    /// This event just sends the index that can be used for referring the List of scriptable objects;
    /// </summary>
    public static event SendEventInt OnPlayerSelected;
    #endregion

    #region Private Variables
    private Button _selectButton;
    #endregion

    #region Unity Callbacks
    void Start() => _selectButton = GetComponent<Button>();
    #endregion

    #region My Functions
    /// <summary>
    /// Tied with the Player_Button;
    /// Just sends an event with int variable and disables the button;
    /// </summary>
    public void OnClick_SelectPlayer()
    {
        OnPlayerSelected?.Invoke(index);
        _selectButton.interactable = false;
    }
    #endregion
}