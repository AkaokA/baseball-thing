using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : BaseballElement, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler {

	public static GameObject objectBeingDragged;
	Vector3 startPosition;
	Vector3 startScale;

	public Vector3 targetPosition;
	private Vector3 positionVelocity;

	private Vector3 targetScale;
	private Vector3 scaleVelocity;

	private static float smoothTime = 0.1f;
	private static float maxSpeed = Mathf.Infinity;

	private Vector3 scaleUp = new Vector3(2, 2, 2);

	void Start () {
		startPosition = transform.localPosition;
		startScale = transform.localScale;

		targetPosition = startPosition;
		targetScale = startScale;
	}

	void Update () {
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref positionVelocity, smoothTime, maxSpeed);
		transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref scaleVelocity, smoothTime, maxSpeed);
	}
 
	public void OnPointerDown (PointerEventData eventData)
	{
		objectBeingDragged = gameObject;
		objectBeingDragged.GetComponent<RawImage> ().raycastTarget = false;
		targetScale = scaleUp;
	}
		
	public void OnBeginDrag (PointerEventData eventData)
	{

	}

	public void OnDrag (PointerEventData eventData)
	{
		RectTransform parentRect = (RectTransform)transform.parent;
		Vector2 posInParent;


		if (eventData.pointerEnter != null) {
			posInParent = eventData.pointerEnter.GetComponent<RectTransform> ().localPosition;
		} else {
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, Camera.allCameras[1], out posInParent);
		}
		targetPosition = posInParent;

	}

	public void OnEndDrag (PointerEventData eventData)
	{

	}

	public void OnPointerUp (PointerEventData eventData)
	{
		objectBeingDragged.GetComponent<RawImage> ().raycastTarget = true;
		targetScale = new Vector3(1,1,1);
	}

}
