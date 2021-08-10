using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats_Data", menuName = "Player/PlayerStats Data")]
public class PlayerStatsData : ScriptableObject
{
    #region Public Variables
    public int index;
    [Tooltip("Player GameObject Prefab")] public GameObject playerPrefab;
    [Tooltip("Name of the Player")] public string playerName = "Player";
    [Tooltip("Weapon name of the Player")] public string playerWeapon;
    [Tooltip("How strong is the Player?")] public int strength;
    [Tooltip("Player speed")] public int speed;
    [Tooltip("Attack range of the Player")] public int atkRange;
    [Tooltip("Rate of fire of the Player")] public int atkRate;
    #endregion

    #region My Functions

    #endregion
}