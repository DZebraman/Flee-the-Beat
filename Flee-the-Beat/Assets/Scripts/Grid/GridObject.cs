using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour {

	//For sanity, the "origin" of a grid object is the first segment
	public int xPos,yPos;
	public int sizeX,sizeY;

	public bool isVertical;
	public bool isPlayer;

	public static bool onBeat;

	Vector3 mousePos;
	Vector3 prevMouse;
	GridManager scriptMaster;

	private bool clicked;
	private bool onePerBeat;

	void Start(){
		onBeat = false;
		clicked = false;
		onePerBeat = false;
		scriptMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridManager>();
	}

//	void OnMouseDown(){
//		clicked = true;
//	}

	void Update(){
		//Debug.Log(onePerBeat);
		if(onBeat && !onePerBeat){
			onePerBeat = true;
			clicked = true;
		}
		else if(!onBeat){
			onePerBeat = false;
		}
	}

	void OnMouseDrag(){

		if(onBeat){
			//scriptMaster.MoveGridObject(this);
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0;
			Debug.Log(clicked);
			if(clicked){
				Debug.Log("Onbeat drag");
				scriptMaster.MoveGridObject(this);
				clicked = false;
			}
			prevMouse = mousePos;
		}
		////if(onBeat){
		//scriptMaster.MoveGridObject(this);
		////}
		//
		//if(clicked){
		//	
		//	mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//	mousePos.z = 0;
		//
		//	if(onBeat && Vector3.Distance(mousePos,prevMouse) > 0.0125f)
		//		Debug.Log("Onbeat drag");
		//	
		//	prevMouse = mousePos;
		//	
		//	clicked = false;
		//}
	}
}
