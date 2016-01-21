using UnityEngine;
using System.Collections;

public class SunView : BaseballElement {

	public AnimationCurve sunCurve;

	public float initialAngle = 0f;
	public float finalAngle = 60f;
	public float initialIntensity = 0f;
	public float finalIntensity = 1.5f;

	public Color initialColor;
	public Color finalColor;

	public float sunriseTime = 3f;

	// Use this for initialization
	void Start () {
		StartCoroutine (Sunrise (initialAngle, initialAngle, initialIntensity, initialIntensity, initialColor, initialColor, 0.01f));
		GetComponent<Light> ().intensity = 0;
	}
	
	public void BeginSunrise () {
		StartCoroutine (Sunrise (initialAngle, finalAngle, initialIntensity, finalIntensity, initialColor, finalColor, sunriseTime));
	}

	IEnumerator Sunrise (float initialAngle, float finalAngle, float initialIntensity, float finalIntensity, Color initialColor, Color finalColor, float time) {
		float currentLerpTime;

		for ( currentLerpTime = 0f; currentLerpTime <= time; currentLerpTime += Time.deltaTime ) {

			float perc = currentLerpTime / time;

			float angle;
			float intensity;
			Color color;

			angle = Mathf.LerpUnclamped(initialAngle, finalAngle, sunCurve.Evaluate (perc));
			intensity = Mathf.LerpUnclamped(initialIntensity, finalIntensity, sunCurve.Evaluate (perc));
			color = Color.LerpUnclamped(initialColor, finalColor, sunCurve.Evaluate (perc));

			Quaternion tempRotation = transform.rotation;
			Vector3 tempEulerAngles = tempRotation.eulerAngles;
			tempEulerAngles.x = angle;
			tempRotation.eulerAngles = tempEulerAngles;
			transform.rotation = tempRotation;

			GetComponent<Light> ().intensity = intensity;
			GetComponent<Light> ().color = color;

			yield return null;
		}
	}

}
