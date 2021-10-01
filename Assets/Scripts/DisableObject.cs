using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour {
    [SerializeField]
	private float _lifeTime;
	
	void OnEnable () {
		StartCoroutine(Disabler());
	}
	private IEnumerator Disabler()
	{
		yield return new WaitForSeconds(_lifeTime);
        if (gameObject.tag == "Ufo")
        {
            GameManager.FindObjectOfType<GameManager>().StartCoroutine("SpawnUfo");
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(false);
    }

}
