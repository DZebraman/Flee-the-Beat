using UnityEngine;
using System.Collections;

public class BackgroudnVisuals : MonoBehaviour {

	public int numLines;
	public float lerpSpeed;
	public float lerpSpeed2;

	public GameObject[,] lines;
	public GameObject line;

	private AudioSource music;
	private float prevLoudness;

	// Use this for initialization
	void Start () {
		lines = new GameObject[numLines,numLines];
		music = GameObject.Find("ScriptAnchor").GetComponent<AudioSource>();

		prevLoudness = 0;

		for(int i = 0; i < numLines; i++){
			for(int k = 0; k < numLines; k++){
				lines[i,k] = (GameObject)Instantiate(line,new Vector3(-8f + (i*0.5f),-2 +  (k*0.5f), 10),Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = music.GetSpectrumData(1024,0,FFTWindow.Blackman);

		float loudness = 0;
		for(int i = 1; i < spectrum.Length; i++){
			loudness += spectrum[i]* i;
		}
		loudness /= spectrum.Length-1;
		loudness = Mathf.Lerp(prevLoudness,loudness,Time.deltaTime*lerpSpeed);
		prevLoudness = loudness;
		loudness *= 100;
		Debug.Log(loudness);

		Quaternion temp = Quaternion.Lerp(lines[0,0].transform.rotation,Quaternion.Euler(new Vector3(0,0,loudness)),Time.deltaTime*lerpSpeed2);

		for(int i = 0; i < numLines; i++){
			for(int k = 0; k < numLines; k++){
				lines[i,k].transform.rotation = temp;
			}
		}
	}
}
