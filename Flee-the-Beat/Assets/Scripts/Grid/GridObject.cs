using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour {

	//For sanity, the "origin" of a grid object is the first segment
	public int xPos,yPos;
	public int sizeX,sizeY;

	Vector3 mousePos;
	
	GridManager scriptMaster;

	void Start(){
		scriptMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridManager>();
	}

	void OnMouseDrag(){
		scriptMaster.MoveGridObject(this);
	}
}
