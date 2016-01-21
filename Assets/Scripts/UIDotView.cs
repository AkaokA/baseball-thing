using UnityEngine;
using System.Collections;

public class UIDotView : BaseballElement {

	public Color disabledColor;
	public Color ballDotColor;
	public Color strikeDotColor;
	public Color outDotColor;

	public IEnumerator changeColor (Color color) {
		float time = 0.2f;
		float currentLerpTime;

		Color initialColor = GetComponent<UnityEngine.UI.RawImage> ().color;
		Color currentColor;

		for ( currentLerpTime = 0f; currentLerpTime <= time; currentLerpTime += Time.deltaTime ) {
			float perc = currentLerpTime / time;
			currentColor = Color.Lerp (initialColor, color, perc);

			GetComponent<UnityEngine.UI.RawImage> ().color = currentColor;
			yield return null;
		}
	}
}
