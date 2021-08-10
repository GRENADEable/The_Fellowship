using UnityEngine;
using TMPro;

public class PlayersStatsButton : MonoBehaviour
{
    #region Serialised Variables
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Transform playerPos;
    [SerializeField] private int statSpeed;
    [SerializeField] private TextMeshProUGUI speedText;

    public delegate void SendEvents();
    public static event SendEvents OnPlayerSpawned;
    #endregion

    #region Private Variables
    private GameObject _spawnedPlayer;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable() => PlayersStatsButton.OnPlayerSpawned += OnPlayerSpawnedEventRecieved;

    void OnDisable() => PlayersStatsButton.OnPlayerSpawned -= OnPlayerSpawnedEventRecieved;

    void OnDestroy() => PlayersStatsButton.OnPlayerSpawned -= OnPlayerSpawnedEventRecieved;
    #endregion

    #endregion

    #region My Functions
    public void OnClick_PlayerSpawnTest()
    {
        OnPlayerSpawned?.Invoke();
        _spawnedPlayer = Instantiate(playerObj, playerPos.position, Quaternion.identity);
        speedText.text = $"Speed : {statSpeed}";
    }
    #endregion

    #region Events
    void OnPlayerSpawnedEventRecieved()
    {
        Destroy(_spawnedPlayer);
        _spawnedPlayer = null;
    }
    #endregion
}