using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : BaseballElement, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	public DuelGrid duelGrid;
	public GameObject objectBeingDragged;
	Vector3 startPosition;
	Vector3 startScale;

	private Vector3 targetPosition;
	private Vector3 positionVelocity;

	private Vector3 targetScale;
	private Vector3 scaleVelocity;

	private static float smoothTime = 0.1f;
	private static float maxSpeed = Mathf.Infinity;

	private Vector3 scaleUp = new Vector3(2, 2, 2);

	private int highlightColumn;
	private int highlightRow;
	public Color highlightColor;

	void Start () {
		duelGrid = app.views.duelGrid.GetComponent<DuelGrid> ();

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
		
	public void OnDrag (PointerEventData eventData)
	{
		RectTransform parentRect = (RectTransform)transform.parent;
		Vector2 posInParent;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, Camera.allCameras[1], out posInParent);
		targetPosition = posInParent;

		if (eventData.pointerEnter != null) {
			HighlightCells (eventData.pointerEnter, true);
		} else {
			HighlightCells (null, false);
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		RectTransform parentRect = (RectTransform)transform.parent;
		Vector2 posInParent;

		if (eventData.pointerEnter != null) {
			targetPosition = eventData.pointerEnter.GetComponent<RectTransform> ().anchoredPosition;
		} else {
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, Camera.allCameras[1], out posInParent);
			targetPosition = posInParent;
		}

		objectBeingDragged.GetComponent<RawImage> ().raycastTarget = true;
		targetScale = new Vector3(1,1,1);

		HighlightCells (null, false);
	}





	void HighlightCells (GameObject pointerEnter, bool highlightActive) {

		if (objectBeingDragged.name == "Pitch Marker") {
			highlightColor = app.model.redTeamColor;
		} else {
			highlightColor = app.model.blueTeamColor;
		}

		if (highlightActive) {
			// find the current column and row
			for (int column = 0; column < duelGrid.gridCells.GetLength (0); column++) {
				for (int row = 0; row < duelGrid.gridCells.GetLength (1); row++) {

					if (duelGrid.gridCells[column, row] == pointerEnter) {
						// Debug.Log (column + ", " + row);
						highlightColumn = column;
						highlightRow = row;
					}
				}		
			}

			// highlight current column and row
			for (int column = 0; column < duelGrid.gridCells.GetLength (0); column++) {
				for (int row = 0; row < duelGrid.gridCells.GetLength (1); row++) {
					if (column == highlightColumn || row == highlightRow) {
						duelGrid.gridCells [column, row].transform.FindChild ("Highlight").gameObject.SetActive (true);
						duelGrid.gridCells [column, row].transform.FindChild ("Highlight").gameObject.GetComponent<RawImage> ().color = highlightColor;
					} else {
						duelGrid.gridCells [column, row].transform.FindChild ("Highlight").gameObject.SetActive (false);
					}
				}		
			}

		} else {
			foreach (GameObject cell in duelGrid.gridCells) {
				cell.transform.FindChild ("Highlight").gameObject.SetActive (false);
			}
		}

	}

}
