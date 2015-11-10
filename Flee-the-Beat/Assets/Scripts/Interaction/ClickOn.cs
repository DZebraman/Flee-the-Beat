using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ClickOn : MonoBehaviour {

	Dictionary<GameObject,ClickAble> isClickableMap;

	// Use this for initialization
	void Start () {
		isClickableMap = new Dictionary<GameObject,ClickAble>();
		GameObject[] clickable = GameObject.FindGameObjectsWithTag("Clickable");
		for(int i = 0; i < clickable.Length; i++){
			ClickAble temp = clickable[i].GetComponent<ClickAble>();
			isClickableMap[clickable[i]] = temp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(Camera.main.transform.position,ray);
			if(Physics.Raycast(ray,out hit)){
				if(isClickableMap[hit.transform.gameObject] != null){
					Debug.Log("Clicked");
					isClickableMap[hit.transform.gameObject].isClicked = true;
				}
			}
		}
	}
}
