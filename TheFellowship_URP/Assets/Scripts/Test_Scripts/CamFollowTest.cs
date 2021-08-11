using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamFollowTest : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables
    private CinemachineVirtualCamera _VCam;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        GameManagerLvl.OnCharSpawn += OnPlayerObjEventReceived;

        PlayersStatsButtonTest.OnPlayerObj += OnPlayerObjEventReceived;
    }

    void OnDisable()
    {
        GameManagerLvl.OnCharSpawn -= OnPlayerObjEventReceived;

        PlayersStatsButtonTest.OnPlayerObj -= OnPlayerObjEventReceived;
    }

    void OnDestroy()
    {
        GameManagerLvl.OnCharSpawn -= OnPlayerObjEventReceived;

        PlayersStatsButtonTest.OnPlayerObj -= OnPlayerObjEventReceived;
    }
    #endregion

    void Start()
    {
        _VCam = GetComponent<CinemachineVirtualCamera>();
    }
    #endregion

    #region Events
    void OnPlayerObjEventReceived(GameObject obj)
    {
        _VCam.m_Follow = obj.transform;
    }
    #endregion
}