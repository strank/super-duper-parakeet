using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

    #region Variables


    #endregion

    #region Unity Methods

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + " has collided with" + collision.gameObject.name);
    }



    #endregion
}
