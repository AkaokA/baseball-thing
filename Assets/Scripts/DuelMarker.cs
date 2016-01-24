using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DuelMarker : BaseballElement, IPointerDownHandler, IDragHandler, IPointerUpHandler {
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
		objectBeingDragged.GetComponent<Image> ().raycastTarget = false;
		targetScale = scaleUp;
	}
		
	public void OnDrag (PointerEventData eventData)
	{
		if (eventData.pointerEnter != null) {
			if (eventData.pointerEnter.tag == "Duel Grid Cell") {
				targetPosition = eventData.pointerEnter.GetComponent<RectTransform> ().anchoredPosition;
				ReportMarkerPosition ();
				MovePitchEndMarker ();
				HighlightCells (eventData.pointerEnter, true);
			}
		} else {
			HighlightCells (null, false);
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (eventData.pointerEnter != null) {
			if (eventData.pointerEnter.tag == "Duel Grid Cell") {
				targetPosition = eventData.pointerEnter.GetComponent<RectTransform> ().anchoredPosition;
				MovePitchEndMarker ();
			}
		}

		objectBeingDragged.GetComponent<Image> ().raycastTarget = true;
		targetScale = new Vector3(1,1,1);

		HighlightCells (null, false);

		// report current marker position
		ReportMarkerPosition ();
	}

	void ReportMarkerPosition () {
		if ( gameObject == app.views.duelSwingMarker ) {
			app.duelController.currentSwingLocation.column = highlightColumn;
			app.duelController.currentSwingLocation.row = highlightRow;
		}
		if ( gameObject == app.views.duelPitchMarker ) {
			app.duelController.currentPitchLocation.column = highlightColumn;
			app.duelController.currentPitchLocation.row = highlightRow;
		}
	}

	public void MoveToCell (int column, int row) {
		int targetColumn = ClampToGridEdges (column, row).column;
		int targetRow = ClampToGridEdges (column, row).row;

		targetPosition = app.views.duelGrid.GetComponent<DuelGrid> ().gridCells[targetColumn, targetRow].GetComponent<RectTransform> ().anchoredPosition;
		ReportMarkerPosition ();
	}

	public void MovePitchEndMarker () {
		if (app.views.duelPitchEndMarker.activeInHierarchy) {
			int targetColumn = app.duelController.currentPitchLocation.column + app.duelController.currentPitch.movement.column;
			int targetRow = app.duelController.currentPitchLocation.row + app.duelController.currentPitch.movement.row;
			targetColumn = ClampToGridEdges (targetColumn, targetRow).column;
			targetRow = ClampToGridEdges (targetColumn, targetRow).row;
			DuelGridCoordinates targetGridLocation = new DuelGridCoordinates (targetColumn, targetRow);
			app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToCell (targetGridLocation.column, targetGridLocation.row);
		}
	}

	public DuelGridCoordinates ClampToGridEdges (int column, int row) {
		if (column < 0) {
			column = 0;
		} else if (column > DuelGrid.gridColumns - 1) {
			column = DuelGrid.gridColumns - 1;
		} else if (row < 0) {
			row = 0;
		} else if (row > DuelGrid.gridRows - 1) {
			row = DuelGrid.gridRows - 1;
		}
			
		return new DuelGridCoordinates (column, row);
	}

	void GetGridPositionUnderPointer (GameObject pointerEnter) {
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
	}

	void HighlightCells (GameObject pointerEnter, bool highlightActive) {

		if (objectBeingDragged == app.views.duelPitchMarker) {
			highlightColor = app.model.redTeamColor;
		} else {
			highlightColor = app.model.blueTeamColor;
		}

		if (highlightActive) {
			GetGridPositionUnderPointer (pointerEnter);

			// highlight current column and row
			for (int column = 0; column < duelGrid.gridCells.GetLength (0); column++) {
				for (int row = 0; row < duelGrid.gridCells.GetLength (1); row++) {
					GameObject highlightDot = duelGrid.gridCells [column, row].transform.FindChild ("Highlight").gameObject;

					if (column == highlightColumn || row == highlightRow) {
						highlightDot.SetActive (true);
						highlightDot.GetComponent<Image> ().color = highlightColor;
					} else {
						highlightDot.SetActive (false);
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
