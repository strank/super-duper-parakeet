using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public class RopeTest : MonoBehaviour {

    #region Variables
    public GameObject startPrefab;
    public GameObject endPrefab;

    private GameObject startObject;
    private GameObject endObject;

    public Transform ropeStart;
    public Transform ropeEnd;

    public Rope rope;

    #endregion


    #region Unity Methods

    private void Start()
    {
        CreateObjects();

        rope.startBody = startObject.GetComponent<Rigidbody>();
        rope.endBody = endObject.GetComponent<Rigidbody>();

        rope.gameObject.SetActive(true);
    }

    private void CreateObjects()
    {
        startObject = (GameObject)Instantiate(startPrefab, ropeStart.position, Quaternion.identity, this.gameObject.transform);
        endObject = (GameObject)Instantiate(endPrefab, ropeEnd.position, Quaternion.identity, this.gameObject.transform);
    }



    #endregion
}
