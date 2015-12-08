using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Displacement/Fisheye")]
    public class Fisheye : PostEffectsBase
	{
        [Range(0.0f, 1.5f)]
        public float strengthX = 0.05f;
        [Range(0.0f, 1.5f)]
        public float strengthY = 0.05f;

        public Shader fishEyeShader = null;
        private Material fisheyeMaterial = null;

		private float prevLoudness = 0;
		public float lerpSpeed;
		public float lerpSpeed2;
        public override bool CheckResources ()
		{
            CheckSupport (false);
            fisheyeMaterial = CheckShaderAndCreateMaterial(fishEyeShader,fisheyeMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            if (CheckResources()==false)
			{
                Graphics.Blit (source, destination);
                return;
            }

			AudioSource music = Camera.main.GetComponent<AudioSource>();
			
			float[] spectrum = music.GetSpectrumData(256,0,FFTWindow.Blackman);
			float loudness = 0;

			for(int i = 1; i < spectrum.Length; i++){
				loudness += spectrum[i] * i * 2;
			}
			loudness /= spectrum.Length-1;
			if(loudness > 0.00000125f)
				loudness = 1;
			else{
				loudness = 0;
			}
			loudness = Mathf.Lerp(prevLoudness,loudness,lerpSpeed2);

			prevLoudness = loudness;
			strengthX = loudness /4;//Mathf.Lerp(strengthX,loudness,Time.deltaTime*lerpSpeed) / 10;
			strengthY = loudness ;//Mathf.Lerp(strengthY,loudness,Time.deltaTime*lerpSpeed) / 5;

            float oneOverBaseSize = 80.0f / 512.0f; // to keep values more like in the old version of fisheye

            float ar = (source.width * 1.0f) / (source.height * 1.0f);

            fisheyeMaterial.SetVector ("intensity", new Vector4 (strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize, strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize));
            Graphics.Blit (source, destination, fisheyeMaterial);
        }
    }
}
