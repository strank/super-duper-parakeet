using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationSystem : MonoBehaviour {

    #region Variables
    //CreateBox createBox;
    //CreateLedge createLedge;

    private float ledgeHeight;
    [SerializeField] private float MAX_DIST_LEDGE_TO_BOX = 1.0f;

    static GenerationSystem instance;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one GenerationSystem in scene");
            return;
        }
        instance = this;

        //createBox = CreateBox.GetInstance();
        //createLedge = CreateLedge.GetInstance();
    }

    /*
     * Receives randomly selected height value of the ledge to reevaluate and update the possible scale range 
     * for a created box. This is to ensure that the box will be of sufficient size to reach the ledge.
     */
    public void SetLedgeInformation(float height)
    {
        ledgeHeight = height;
    }

    public float GetLedgeInformation()
    {
        return ledgeHeight;
    }

    public float GetDistFromLedgeToBox()
    {
        return MAX_DIST_LEDGE_TO_BOX;
    }
    

    public static GenerationSystem GetInstance()
    {
        return instance;
    }

    #endregion
}
