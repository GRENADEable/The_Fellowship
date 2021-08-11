using UnityEngine;
using Cinemachine;

public class CamFollow : MonoBehaviour
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

    void Start() => _VCam = GetComponent<CinemachineVirtualCamera>();
    #endregion

    #region Events
    void OnPlayerObjEventReceived(GameObject obj)
    {
        _VCam.m_Follow = obj.transform;
        _VCam.m_LookAt = obj.transform;
    }
    #endregion
}