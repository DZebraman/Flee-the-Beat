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

	private float prevCamAudioTime;

	private float avgAudioTime;
	private List<float> audioTimes;
	private int audioTimeCounter;
	public int audioAvgPrecision;

	// Use this for initialization
	void Awake () {
		cameraAudio = Camera.main.GetComponent<AudioSource>();

		//cameraAudio.time = 120;

		GridObject.onBeat = false;

		audioTimeCounter=0;

		beatCounters = new beatObject[timeSig];
		beat = 0;

		prevCamAudioTime = Time.deltaTime;

		audioTimes = new List<float>();
		audioTimeCounter = 0;

		for(int i = 0; i < beatCounters.Length; i++){
			beatCounters[i].go = (GameObject)Instantiate(visualizer,counterAnchor.position + Vector3.right * i * margin + Vector3.back * 2,Quaternion.Euler(180,0f,0f));
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
		Debug.Log(GridObject.onBeat);

			if(currentTime > timeForBeat-halfMargin){
				beatCounters[(beat+1)%timeSig].mat.color = Color.green;
				GridObject.onBeat = true;
				clicked = true;
			}else if(currentTime < halfMargin){
				beatCounters[beat].mat.color = Color.green;
				GridObject.onBeat = true;
				clicked = true;
			}else{
				beatCounters[beat].mat.color = Color.red * 0.75f;
				GridObject.onBeat = false;
		}




		//Debug.Log(cameraAudio.time - prevCamAudioTime);

		audioTimes.Add(Time.deltaTime);

		//Debug.Log(cameraAudio.time + " , " + prevCamAudioTime );
		audioTimeCounter++;

		float avg = 0;
		if(audioTimes.Count != 0){
			for(int i = 0; i < audioTimes.Count; i++){
				
				avg += audioTimes[i];
			}
			
			avg /=  audioTimes.Count;
		}

		currentTime += avg;

		//Debug.Log(avg);

		prevCamAudioTime = Time.deltaTime;

		if (currentTime > timeForBeat) {
			//Debug.Log(currentTime);
			currentTime = 0;
			beatCounters[beat].outline.color = Color.red * 0.75f;
			beat++;
			beat %= timeSig;
			sound.Play();
			beatCounters[beat].go.transform.localScale = Vector3.one * scaleMax;
			beatCounters[beat].outline.color = Color.yellow;
		}
		if (!clicked && currentTime < halfMargin) {
			beatCounters[beat].mat.color = Color.red * 0.75f;
		} else if(clicked && currentTime > halfMargin && currentTime < timeForBeat - halfMargin){
			clicked = false;
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
