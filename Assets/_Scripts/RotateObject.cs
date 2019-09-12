using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    #region Variables

    public Vector3 rotationValue = new Vector3(45.0f, 45.0f, 45.0f);

    #endregion

    #region Unity Methods

    private void Awake()
    {
        RotateThis();
    }

    private void RotateThis()
    {
        transform.localEulerAngles = rotationValue;
    }

    #endregion
}
