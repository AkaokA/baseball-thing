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
	private bool isMoving = false;
	private bool isScaling = false;

	private Vector3 targetScale;
	private Vector3 scaleVelocity;

	private static float smoothTime = 0.1f;
	private static float maxSpeed = Mathf.Infinity;

	private Vector3 scaleUp = new Vector3(2, 2, 2);

	private DuelGridCoordinates currentGridPosition;
	private DuelGridCoordinates gridPositionUnderPointer;

	public Color highlightColor;

	void Start () {
		currentGridPosition = new DuelGridCoordinates (Mathf.FloorToInt (DuelGrid.gridColumns / 2), Mathf.FloorToInt (DuelGrid.gridRows / 2));

		startPosition = transform.localPosition;
		startScale = transform.localScale;

		duelGrid = app.views.duelGrid.GetComponent<DuelGrid> ();
	}

	void Awake () {
		isMoving = false;
		isScaling = false;
	}
		
	public void MoveToCell (int column, int row) {
		int targetColumn = ClampToGridEdges (column, row).column;
		int targetRow = ClampToGridEdges (column, row).row;

		targetPosition = app.views.duelGrid.GetComponent<DuelGrid> ().gridCells[targetColumn, targetRow].GetComponent<RectTransform> ().anchoredPosition;

		if (!isMoving) {
			StartCoroutine (MoveMarker ());
		}

		ReportMarkerPosition ();
	}

	IEnumerator MoveMarker () {
		isMoving = true;
		while ( (targetPosition - transform.localPosition).magnitude > 0.01f ) {
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref positionVelocity, smoothTime, maxSpeed);
			yield return null;
		}
		isMoving = false;
		// Debug.Log ("done moving!"); 
	}

	IEnumerator ResizeMarker () {
		isScaling = true;
		while ( (targetScale - transform.localScale).magnitude > 0.01f ) {
			transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref scaleVelocity, smoothTime, maxSpeed);
			yield return null;
		}
		isScaling = false;
	}

	public void ResetPosition () {
		transform.localPosition = startPosition;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		objectBeingDragged = gameObject;
		objectBeingDragged.GetComponent<Image> ().raycastTarget = false;

		// grow marker
		targetScale = scaleUp;
		if (!isScaling) {
			StartCoroutine (ResizeMarker ());
		}
	}
		
	public void OnDrag (PointerEventData eventData)
	{
		if (eventData.pointerEnter != null) {
			if (eventData.pointerEnter.tag == "Duel Grid Cell") {

				DuelGridCoordinates gridPositionUnderPointer = GridPositionUnderPointer (eventData.pointerEnter);
				if (currentGridPosition != gridPositionUnderPointer) {
					currentGridPosition = gridPositionUnderPointer;
					HighlightCells (true);
					ReportMarkerPosition ();

					MoveToCell (currentGridPosition.column, currentGridPosition.row);
				}

				if (app.views.duelPitchEndMarker.activeInHierarchy) {
					StartCoroutine (app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());
				}
			}
		} else {
			if (app.views.duelPitchEndMarker.activeInHierarchy) {
				StartCoroutine (app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());
			}
			HighlightCells (false);
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (eventData.pointerEnter != null) {
			if (eventData.pointerEnter.tag == "Duel Grid Cell") {

				DuelGridCoordinates gridPositionUnderPointer = GridPositionUnderPointer (eventData.pointerEnter);
				if (currentGridPosition != gridPositionUnderPointer) {
					currentGridPosition = gridPositionUnderPointer;
					HighlightCells (true);
					ReportMarkerPosition ();

					MoveToCell (currentGridPosition.column, currentGridPosition.row);
				}

				if (app.views.duelPitchEndMarker.activeInHierarchy) {
					StartCoroutine (app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());
				}

			}
		}

		HighlightCells (false);
		ReportMarkerPosition ();
		objectBeingDragged.GetComponent<Image> ().raycastTarget = true;

		// shrink marker
		targetScale = Vector3.one;
		if (!isScaling) {
			StartCoroutine (ResizeMarker ());
		}

	}

	void ReportMarkerPosition () {
		if ( gameObject == app.views.duelSwingMarker ) {
			app.duelController.currentSwingLocation.column = currentGridPosition.column;
			app.duelController.currentSwingLocation.row = currentGridPosition.row;
		}
		if ( gameObject == app.views.duelPitchMarker ) {
			app.duelController.currentPitchLocation.column = currentGridPosition.column;
			app.duelController.currentPitchLocation.row = currentGridPosition.row;
		}
	}
		
	public IEnumerator MoveToPitchDestination () {
		yield return null;
		int targetColumn = app.duelController.currentPitchLocation.column + app.duelController.currentPitch.movement.column;
		int targetRow = app.duelController.currentPitchLocation.row + app.duelController.currentPitch.movement.row;
		targetColumn = ClampToGridEdges (targetColumn, targetRow).column;
		targetRow = ClampToGridEdges (targetColumn, targetRow).row;
		DuelGridCoordinates targetGridLocation = new DuelGridCoordinates (targetColumn, targetRow);

		MoveToCell (targetGridLocation.column, targetGridLocation.row);
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

	DuelGridCoordinates GridPositionUnderPointer (GameObject pointerEnter) {

		// find the current column and row
		for (int column = 0; column < duelGrid.gridCells.GetLength (0); column++) {
			for (int row = 0; row < duelGrid.gridCells.GetLength (1); row++) {

				if (duelGrid.gridCells[column, row] == pointerEnter) {
					// Debug.Log (column + ", " + row);
					gridPositionUnderPointer = new DuelGridCoordinates(column, row);
				}
			}		
		}
		return gridPositionUnderPointer;
	}

	void HighlightCells (bool highlightActive) {

		if (objectBeingDragged == app.views.duelPitchMarker) {
			highlightColor = app.model.redTeamColor;
		} else {
			highlightColor = app.model.blueTeamColor;
		}

		if (highlightActive) {

			// highlight current column and row
			for (int column = 0; column < duelGrid.gridCells.GetLength (0); column++) {
				for (int row = 0; row < duelGrid.gridCells.GetLength (1); row++) {
					GameObject highlightDot = duelGrid.gridCells [column, row].transform.FindChild ("Highlight").gameObject;

					if (column == currentGridPosition.column || row == currentGridPosition.row) {
						highlightDot.SetActive (true);

						int strikeZoneWidthPadding = (DuelGrid.gridColumns - DuelGrid.strikeZoneWidth) / 2;
						int strikeZoneHeightPadding = (DuelGrid.gridRows - DuelGrid.strikeZoneHeight) / 2;

						if (column >= strikeZoneWidthPadding && column < DuelGrid.gridColumns - strikeZoneWidthPadding && row >= strikeZoneHeightPadding && row < DuelGrid.gridRows - strikeZoneHeightPadding) {
							highlightDot.GetComponent<Image> ().sprite = app.views.duelGrid.GetComponent<DuelGrid> ().strikeZoneHighlightDot;
							highlightDot.GetComponent<Image> ().color = highlightColor;
						} else {
							highlightDot.GetComponent<Image> ().color = highlightColor;
						}


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
