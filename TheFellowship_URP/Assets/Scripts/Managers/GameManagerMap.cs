using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMap : MonoBehaviour
{
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
    public void OnClick_Lvl(int index) => StartCoroutine(StartLvlDelay(index));
    #endregion

    #region Coroutines
    IEnumerator StartLvlDelay(int index)
    {
        fadeBG.Play("Fade_Out");
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(index);
    }
    #endregion
}