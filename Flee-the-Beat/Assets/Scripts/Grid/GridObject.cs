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

	void Start(){
		onBeat = false;
		scriptMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridManager>();
	}

	void OnMouseDrag(){
		if(onBeat){
			scriptMaster.MoveGridObject(this);
		}
	}
}
