using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour {

    #region Variables


    public GameObject testObject;
    #endregion

    #region Unity Methods

    private void Start()
    {
        Instantiate(testObject);
    }



    #endregion
}
