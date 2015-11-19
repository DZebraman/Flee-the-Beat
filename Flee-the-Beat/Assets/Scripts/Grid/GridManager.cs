using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {

	private struct gridTile{
		public Vector3 pos;
		public GameObject gO;
		public GridObject gridObject;
	}

	private struct GridObjectPackage{
		public GridObject gridO;
		public GameObject gO;
		public Vector3 offset;
		public Vector3 lScale;
	}

	//quick gridspace lookup
	private Dictionary<GridObject, Vector2> gridObjectXY;
	private Dictionary<GridObject, GridObjectPackage> gridObjectRef;

	private gridTile[,] grid;
	private GameObject[] gridGO;
	private GridObject[] gridObjectArray;

	private Vector3 mousePos, prevPos;

	public int gridSize;
	public float gridSpacing;

	// Use this for initialization
	void Awake () {
		grid = new gridTile[gridSize,gridSize];
		gridObjectXY = new Dictionary<GridObject, Vector2>();
		gridObjectRef = new Dictionary<GridObject, GridObjectPackage>();


		for(int i = 0; i < gridSize; i++){
			for(int k = 0; k < gridSize; k++){
				grid[i,k] = new gridTile();
				grid[i,k].pos = (new Vector3(i,k,0.0f)) * gridSpacing;
				grid[i,k].gO = (GameObject)Instantiate(Resources.Load("Prefabs/GridMarker"),grid[i,k].pos,Quaternion.identity);
			}
		}

		gridGO = GameObject.FindGameObjectsWithTag("GridObject");
		gridObjectArray = new GridObject[gridGO.Length];

		for(int i = 0; i < gridGO.Length; i++){

			GridObjectPackage tempPackage = new GridObjectPackage();

			GridObject tempGrid = gridGO[i].GetComponent<GridObject>();
			gridObjectArray[i] = tempGrid;
			Vector3 lScale = gridGO[i].transform.localScale;


			//additional sizes will always go down or left

			lScale = new Vector3(lScale.x * tempGrid.sizeX, lScale.y * tempGrid.sizeY,1.0f) * gridSpacing;
			Vector3 posOffset = new Vector3(((-lScale.x * 0.5f)+0.5f),((-lScale.y * 0.5f)-0.5f),0.0f);

			gridGO[i].transform.position = grid[tempGrid.xPos,tempGrid.yPos].pos + posOffset;
			gridGO[i].transform.localScale = lScale;

			//fills the spaces in the grid
			GridFill(tempGrid);
			gridObjectXY[tempGrid] = new Vector2(tempGrid.xPos,tempGrid.yPos);
			//grid[tempGrid.xPos,tempGrid.yPos-1].gridObject = tempGrid;

			tempPackage.gO = tempGrid.gameObject;
			tempPackage.gridO = tempGrid;
			tempPackage.offset = posOffset;
			tempPackage.lScale = lScale;

			gridObjectRef[tempGrid] = tempPackage;
		}
	}

	public void MoveGridObject(GridObject tempGrid){
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		Vector3 newPos = mousePos;
		Vector3 mouseDir = tempGrid.gameObject.transform.position - mousePos;
		int direction = (int)Mathf.Floor(Vector3.Distance(tempGrid.gameObject.transform.position,mousePos));

		//if(direction > gridSpacing){
			Vector2 xyPos = gridObjectXY[tempGrid];
			int x = (int)xyPos.x;
			int y = (int)xyPos.y;
			

		//if the new place we want to move doesn't have a grid object already
			if(mouseDir.y < 0 && tempGrid.yPos < gridSize){
				if(grid[x,y+2].gridObject == null){
					UpdatePosition(tempGrid,0,1);
					GridFill(tempGrid);
				}
			}
			else if(mouseDir.y > 0 && tempGrid.yPos - tempGrid.sizeY > 0){
				if(grid[x,(y-tempGrid.sizeY-1)].gridObject == null){
					UpdatePosition(tempGrid,0,-1);
					GridFill(tempGrid);
				}
			}
		//}

		UpdateGrid();
	}

	void UpdateGrid(){
		for(int i = 0; i < gridSize; i++){
			for(int k = 0; k < gridSize; k++){
				grid[i,k].gridObject = null;
			}
		}
		foreach(GridObject tempGrid in gridObjectArray){
			GridFill(tempGrid);
		}
	}

	void UpdatePosition(GridObject tempGrid, int x, int y){
		tempGrid.yPos += y;
		tempGrid.xPos += x;

		if(tempGrid.yPos == gridSize)
			tempGrid.yPos = gridSize-1;
		if(tempGrid.xPos == gridSize)
			tempGrid.xPos = gridSize;

		if(tempGrid.yPos == -1)
			tempGrid.yPos = 0;
		if(tempGrid.xPos == -1)
			tempGrid.xPos = 0;

		GridObjectPackage tempPackage = gridObjectRef[tempGrid];

		tempPackage.gO.transform.position = grid[tempGrid.xPos,tempGrid.yPos].pos + tempPackage.offset;
	}

	//fills in the corresponding grid spaces
	void GridFill(GridObject tempGrid){
		for(int k = 0; k < tempGrid.sizeX; k++){
			grid[tempGrid.xPos - k,tempGrid.yPos-1].gridObject = tempGrid;
		}
		for(int k = 0; k < tempGrid.sizeY; k++){
			grid[tempGrid.xPos,tempGrid.yPos-k-1].gridObject = tempGrid;
		}
	}

	void OnDrawGizmos(){
		for(int i = 0; i < gridSize; i++){
			for(int k = 0; k < gridSize; k++){
				Gizmos.color = Color.green;
				if(grid[i,k].gridObject != null){
					Gizmos.color = Color.red;
				}
				Gizmos.DrawWireCube(grid[i,k].pos,Vector3.one);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
