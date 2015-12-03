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
	GridManager scriptMaster;

	private bool clicked;

	void Start(){
		onBeat = false;
		clicked = false;
		scriptMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridManager>();
	}

	void OnMouseDown(){
		if(onBeat){
			clicked = true;
			Debug.Log("Good");
		}
	}

	void Update(){
		//clicked = false;
	}

	void OnMouseDrag(){
		if(onBeat){
			scriptMaster.MoveGridObject(this);
		}
	}
}
