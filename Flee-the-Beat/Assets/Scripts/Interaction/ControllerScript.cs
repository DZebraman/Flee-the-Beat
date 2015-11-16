using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour
{

    public GameObject[,] grid = new GameObject[7,7];

    // Use this for initialization
    void Start()
    {
		GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
		for (int i = 0; i < cells.Length; i++) {
			Cell thisCell = cells[i].GetComponent<Cell> ();
			grid [thisCell.row, thisCell.column] = cells[i];
		}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
