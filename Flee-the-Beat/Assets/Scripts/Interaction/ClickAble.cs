using UnityEngine;
using System.Collections;

public class ClickAble : MonoBehaviour {

	public bool isClicked;

	// Use this for initialization
	void Start () {
		isClicked = false;
	}
	void LateUpdate(){
		isClicked = false;
	}
}
