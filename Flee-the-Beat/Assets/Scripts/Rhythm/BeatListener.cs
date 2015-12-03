using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatListener : MonoBehaviour {
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
	
	public float scaleMax, scaleMin;
	
	public GameObject visualizer;
	public float halfMargin;
	
	public Transform counterAnchor;
	
	private Material debugMat;
	bool clicked;
	
	private beatObject[] beatCounters;
	public int timeSig;
	
	private int beat;
	
	public float margin;
	
	private AudioSource sound;
	private AudioSource cameraAudio;

	private bool onBeat;
	
	// Use this for initialization
	void Awake () {
		cameraAudio = Camera.main.GetComponent<AudioSource>();
		
		//cameraAudio.time = 120;
		
		GridObject.onBeat = false;
		onBeat = false;

		beatCounters = new beatObject[timeSig];
		beat = 0;

		for(int i = 0; i < beatCounters.Length; i++){
			beatCounters[i].go = (GameObject)Instantiate(visualizer,counterAnchor.position + Vector3.right * i * margin + Vector3.back * 2,Quaternion.Euler(180,0f,0f));
			beatCounters[i].go.transform.localScale = Vector3.one * scaleMin;
			beatCounters[i].go.name = "BeatCounter " + i;
			beatCounters[i].mat = beatCounters[i].go.GetComponent<SpriteRenderer>();
			beatCounters[i].outline = beatCounters[i].go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
			beatCounters[i].outline.color = Color.yellow;
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

		float[] spectrumData = cameraAudio.GetSpectrumData(1024,0,FFTWindow.Blackman);
		float loudness = 0;

		for(int i = 1; i < 100; i++){
			loudness += spectrumData[i]*i;
		}
		loudness /= 99;

		if(loudness > 0.000005f && !onBeat){
			Debug.Log("Beat");
			onBeat = true;
			GridObject.onBeat = true;
			beat++;
			beat %= timeSig;
			beatCounters[beat].mat.color = Color.green * 0.75f;
			beatCounters[beat].go.transform.localScale = Vector3.one * scaleMax;
		}
		if(loudness < 0.000005f && onBeat){
			//Debug.Log("Beat");
			beatCounters[beat].mat.color = Color.red * 0.75f;
			beatCounters[beat].go.transform.localScale = Vector3.one * scaleMin;
			onBeat = false;
			GridObject.onBeat = false;
		}

		for(int i = 0; i < beatCounters.Length; i++){
			if(i != beat){
				beatCounters[i].go.transform.localScale = Vector3.Lerp (beatCounters[i].go.transform.localScale, Vector3.one*scaleMin, currentTime);
				if(beatCounters[i].mat.color != Color.red * 0.75f) beatCounters[i].mat.color = Color.Lerp(beatCounters[i].mat.color,Color.red * 0.75f,currentTime / 8);
			}
		}
		beatCounters[beat].go.transform.localScale = Vector3.Lerp (beatCounters[beat].go.transform.localScale, Vector3.one*scaleMin, currentTime);
		//clicked = false;
	}
}