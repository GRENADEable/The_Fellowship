using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTargetGroup : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables
    private CinemachineTargetGroup _cTarget;
    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    #endregion

    #region Unity Callbacks

    #region Events
    //void OnEnable() => GameManagerLvl.OnCharSpawn += OnCharSpawnEventReceieved;

    //void OnDisable() => GameManagerLvl.OnCharSpawn -= OnCharSpawnEventReceieved;

    //void OnDestroy() => GameManagerLvl.OnCharSpawn -= OnCharSpawnEventReceieved;
    #endregion

    void Start() => _cTarget = GetComponent<CinemachineTargetGroup>();
    #endregion

    #region Events
    void OnCharSpawnEventReceieved(GameObject obj)
    {
        _players.Add(obj);

        for (int i = 0; i < _players.Count; i++)
            _cTarget.m_Targets[i].target = _players[i].transform;
        //for (int i = 0; i < _cTarget.m_Targets.Length; i++)
        //{
        //    _cTarget.m_Targets[i].target = obj.transform;
        //}
    }
    #endregion
}