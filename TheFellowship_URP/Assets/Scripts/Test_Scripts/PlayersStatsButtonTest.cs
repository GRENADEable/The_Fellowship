using UnityEngine;
using TMPro;

public class PlayersStatsButtonTest : MonoBehaviour
{
    #region Serialised Variables
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Transform playerPos;
    [SerializeField] private int statSpeed;
    [SerializeField] private TextMeshProUGUI speedText;

    public delegate void SendEvents();
    public static event SendEvents OnPlayerSpawned;

    public delegate void SendEventsObj(GameObject obj);
    public static event SendEventsObj OnPlayerObj;
    #endregion

    #region Private Variables
    private GameObject _spawnedPlayer;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable() => PlayersStatsButtonTest.OnPlayerSpawned += OnPlayerSpawnedEventRecieved;

    void OnDisable() => PlayersStatsButtonTest.OnPlayerSpawned -= OnPlayerSpawnedEventRecieved;

    void OnDestroy() => PlayersStatsButtonTest.OnPlayerSpawned -= OnPlayerSpawnedEventRecieved;
    #endregion

    #endregion

    #region My Functions
    public void OnClick_PlayerSpawnTest()
    {
        OnPlayerSpawned?.Invoke();
        _spawnedPlayer = Instantiate(playerObj, playerPos.position, Quaternion.identity);
        OnPlayerObj?.Invoke(_spawnedPlayer);
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