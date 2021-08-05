using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMap : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Serialzed Variables
    [SerializeField] private Animator fadeBG;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Callbacks
    void Start()
    {
        fadeBG.Play("Fade_In");
    }

    void Update()
    {

    }
    #endregion

    #region My Functions

    #endregion
}