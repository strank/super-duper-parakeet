using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class HFFAgent : MonoBehaviour
{
    private HFFSimInput hffInput;

    public void Awake()
    {
        hffInput = new HFFSimInput();
        hffInput.ConnectController();
        Debug.Log("Simulated Controller connected.");
    }

    public void OnApplicationQuit() {
        hffInput.DisconnectController();
        Debug.Log("Simulated Controller disconnected.");
    }

    public void Start()
    {
        Debug.Log("starting input simulation!");
        StartCoroutine(this.SimulateInput());
    }

    private IEnumerator SimulateInput() {
        yield return new WaitForSeconds(9);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("simulating input start");
            hffInput.Look(32000, 32000);
            yield return new WaitForSeconds(1f);
            Debug.Log("simulating input end");
            hffInput.Look(0, 0);
        }
    }
}
