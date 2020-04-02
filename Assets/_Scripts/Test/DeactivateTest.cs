using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DeactivateTest : MonoBehaviour {

	[SerializeField]private float duration = 3.0f;

	private void Start()
	{
		StartCoroutine(DeactivateAfterTime());
	}

	private IEnumerator DeactivateAfterTime()
	{
		yield return new WaitForSeconds(duration);
		this.gameObject.SetActive(false);
	}
}
