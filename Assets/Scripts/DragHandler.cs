using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : BaseballElement, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler {

	public static GameObject objectBeingDragged;
	Vector3 startPosition;

	private Vector3 targetPosition;
	private Vector3 transformVelocity;
	private static float smoothTime = 0.1f;
	private static float maxSpeed = Mathf.Infinity;

	private float liftHeight = -20f;

	void Start () {
		targetPosition = transform.localPosition;
	}

	void Update () {
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref transformVelocity, smoothTime, maxSpeed);
	}
 
	public void OnPointerDown (PointerEventData eventData)
	{
		targetPosition.z = liftHeight;
	}
		
	public void OnBeginDrag (PointerEventData eventData)
	{
		objectBeingDragged = gameObject;
		startPosition = transform.localPosition;
	}

	public void OnDrag (PointerEventData eventData)
	{
		RectTransform parentRect = (RectTransform)transform.parent;
		Vector2 posInParent;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, Camera.allCameras[1], out posInParent);

		targetPosition = posInParent;
		targetPosition.z = liftHeight;
	}

	public void OnEndDrag (PointerEventData eventData)
	{

	}

	public void OnPointerUp (PointerEventData eventData)
	{
		targetPosition.z = 0f;
	}

}
