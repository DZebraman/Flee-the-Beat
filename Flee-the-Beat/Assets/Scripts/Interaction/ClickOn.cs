using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ClickOn : MonoBehaviour {

	static Dictionary<GameObject,ClickAble> isClickableMap;

	// Use this for initialization
	void Start () {
		isClickableMap = new Dictionary<GameObject,ClickAble>();
		GameObject[] clickable = GameObject.FindGameObjectsWithTag("Clickable");
		for(int i = 0; i < clickable.Length; i++){
			ClickAble temp = clickable[i].GetComponent<ClickAble>();
			isClickableMap[clickable[i]] = temp;
		}
	}

	void Update(){
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit)){
				if(isClickableMap.ContainsKey(hit.transform.gameObject)){
					Debug.Log("Clicked");
					isClickableMap[hit.transform.gameObject].isClicked = true;
				}
			}
		}
	}

	void OnMouseDown(){
		Debug.Log("Clicked");
		isClickableMap[gameObject].isClicked = true;
	}
}
