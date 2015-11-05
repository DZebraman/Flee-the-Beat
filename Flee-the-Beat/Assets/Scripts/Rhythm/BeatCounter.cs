using UnityEngine;
using System.Collections;

public class BeatCounter : MonoBehaviour {

	public float BPM;
	private float timeForBeat;
	private float currentTime;

	public GameObject visualizer;
	public float halfMargin;

	private Material debugMat;
	bool clicked;

	// Use this for initialization
	void Start () {
		timeForBeat = 60/BPM;
		currentTime = 0;
		debugMat = visualizer.GetComponent<Renderer> ().material;
		clicked = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if(currentTime > timeForBeat-halfMargin || currentTime < halfMargin){
				debugMat.color = Color.green;
				clicked = true;
			}else
				debugMat.color = Color.red;
		}
		currentTime += Time.deltaTime;
		if (currentTime > timeForBeat) {
			Debug.Log (currentTime);
			currentTime = 0;
		}
		if (!clicked && currentTime < halfMargin) {
			debugMat.color = Color.red;
		} else if(clicked && currentTime > halfMargin && currentTime < timeForBeat - halfMargin){
			clicked = false;
		}
		visualizer.transform.localScale = Vector3.Lerp (Vector3.one, Vector3.one * 2, currentTime);
		//clicked = false;
	}
}
