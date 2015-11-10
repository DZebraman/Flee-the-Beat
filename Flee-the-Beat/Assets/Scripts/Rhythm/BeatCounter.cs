using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BeatCounter : MonoBehaviour {

	struct beatObject{
		public GameObject go;
		public SpriteRenderer mat;
		public SpriteRenderer outline;
		public bool isClicked;
	}

	Dictionary<GameObject,ClickAble> isClickableMap;

	public float BPM;
	private float timeForBeat;
	private float currentTime;

	public GameObject visualizer;
	public float halfMargin;

	private Material debugMat;
	bool clicked;

	private beatObject[] beatCounters;
	public int timeSig;

	private int beat;

	public float margin;

	private AudioSource sound;

	// Use this for initialization
	void Awake () {
		beatCounters = new beatObject[timeSig];
		beat = 0;

		for(int i = 0; i < beatCounters.Length; i++){
			beatCounters[i].go = (GameObject)Instantiate(visualizer,Vector3.right * i * margin,Quaternion.Euler(270f,0f,0f));
			beatCounters[i].mat = beatCounters[i].go.GetComponent<SpriteRenderer>();
			beatCounters[i].outline = beatCounters[i].go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
		}

		isClickableMap = new Dictionary<GameObject,ClickAble>();
		GameObject[] clickable = GameObject.FindGameObjectsWithTag("Clickable");
		for(int i = 0; i < clickable.Length; i++){
			ClickAble temp = clickable[i].GetComponent<ClickAble>();
			isClickableMap[clickable[i]] = temp;
		}

		sound = GetComponent<AudioSource>();

		timeForBeat = 60/BPM;
		currentTime = 0;
		clicked = false;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetMouseButtonDown (0) && isClickableMap[beatCounters[beat].go].isClicked) {
			if(currentTime > timeForBeat-halfMargin){
				beatCounters[(beat+1)%timeSig].mat.color = Color.green;
				clicked = true;
			}else if(currentTime < halfMargin){
				beatCounters[beat].mat.color = Color.green;
				clicked = true;
			}else
				beatCounters[beat].mat.color = Color.red * 0.75f;
		}


		currentTime += Time.deltaTime;
		if (currentTime > timeForBeat) {
			currentTime = 0;
			beatCounters[beat].outline.color = Color.red * 0.75f;
			beat++;
			beat %= timeSig;
			sound.Play();
			beatCounters[beat].go.transform.localScale = Vector3.one * 0.5f;
			beatCounters[beat].outline.color = Color.yellow;
		}
		if (!clicked && currentTime < halfMargin) {
			beatCounters[beat].mat.color = Color.red * 0.75f;
		} else if(clicked && currentTime > halfMargin && currentTime < timeForBeat - halfMargin){
			clicked = false;
		}

		for(int i = 0; i < beatCounters.Length; i++){
			if(i != beat){
				beatCounters[i].go.transform.localScale = Vector3.Lerp (beatCounters[i].go.transform.localScale, Vector3.one*0.25f, currentTime);
				if(beatCounters[i].mat.color != Color.red * 0.75f) beatCounters[i].mat.color = Color.Lerp(beatCounters[i].mat.color,Color.red * 0.75f,currentTime / 8);
			}
		}
		beatCounters[beat].go.transform.localScale = Vector3.Lerp (beatCounters[beat].go.transform.localScale, Vector3.one*0.25f, currentTime);
		//clicked = false;
	}
}
