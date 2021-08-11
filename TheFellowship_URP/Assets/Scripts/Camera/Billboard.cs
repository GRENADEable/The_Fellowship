using UnityEngine;

public class Billboard : MonoBehaviour
{
    #region Private Variables
    private Camera _cam;
    #endregion

    #region Unity Callbacks
    void Start() => _cam = Camera.main;

    void LateUpdate()
    {
        transform.LookAt(_cam.transform);
    }
    #endregion
}