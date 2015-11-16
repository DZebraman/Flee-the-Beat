using UnityEngine;
using System.Collections;

public class SlidableTile : MonoBehaviour
{
    public bool isVertical;
    private bool mouseDown = false;
	private ControllerScript control;
	public int row, column;
	Vector3 mousePos, prevPos;
	public int length;

    // Use this for initialization
    void Start()
    {
		control = GameObject.Find("Controller").GetComponent<ControllerScript> ();
    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
			mouseDown = true;
			prevPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            mouseDown = false;
        }
    }


    void OnMouseOut()
    {
        mouseDown = false;
    }
    

    void OnMouseDrag()
    {
        if (mouseDown)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
			Vector3 newPos = mousePos;
			Vector3 mouseDir = prevPos - mousePos;
			int direction = (int)Mathf.Floor(Vector3.Distance(transform.position,mousePos));

			if (isVertical)
            {
				if(mouseDir.y > 0){
					row += direction;
					if(row > 6){
						row = 6;
					}
				}
				else if(mouseDir.y < 0){
					row -= direction;
					if(row < 1){
						row = 1;
					}
				}
				if(length % 2 == 0){
					newPos.x = control.grid[row, column].transform.position.x;
					newPos.y = control.grid[row, column].transform.position.y - 0.5f;
				}
				else{
					newPos = control.grid[row, column].transform.position;
				}
				//newPos.x = transform.position.x;
            }
			else{
				if(mouseDir.y > 0){
					column += 1;
					if(column > 6){
						column = 6;
					}
				}
				else if(mouseDir.y < 0){
					column -= 1;
					if(column < 0){
						column = 0;
					}	
				}
				if(length % 2 == 0){
					newPos.y = control.grid[row, column].transform.position.y;
					newPos.x = control.grid[row, column].transform.position.x - 0.5f;
				}
				else{
					newPos = control.grid[row, column].transform.position;
				}
				//newPos.y = transform.position.y;
            }
            //transform.position = Vector3.Lerp(transform.position,newPos,Time.deltaTime);
			//Vector3 temp = transform.position;
			//temp.y = Mathf.Lerp(temp.y,mousePos.y,Time.deltaTime);
			transform.position = newPos;
			//transform.position = newPos;

        }
    }
}