using UnityEngine;
using System.Collections;

public class SlidableTile : MonoBehaviour
{
    public bool isVertical;
    private bool mouseDown = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            mouseDown = true;
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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            if (isVertical)
            {
                mousePos.x = transform.position.x;
            }
            else
            {
                mousePos.y = transform.position.y;
            }
            transform.position = mousePos;
        }
    }
}