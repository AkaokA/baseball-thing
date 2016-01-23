using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DuelGrid : BaseballElement {

	public AnimationCurve easingCurve;

	private int gridRows = 9;
	private int gridColumns = 7;
	private int strikeZonePadding = 1;

	private float cellWidth;
	private float cellHeight;

	private RectTransform duelGridRect;

	public GameObject[,] gridCells;
	public GameObject gridCell;

	private Color normalDotColor = new Color (0f, 0f, 0f, 0.25f);
	private Color darkerDotColor = new Color (0f, 0f, 0f, 0.75f);

	void Start () {
		gridCells = new GameObject[gridColumns, gridRows];

		duelGridRect = app.views.duelGrid.GetComponent<RectTransform> ();

		cellHeight = duelGridRect.rect.height / gridRows;
		cellWidth = cellHeight;

		GameObject currentCell;
		int row;
		int column;

		// set up grid
		for (row = 0; row < gridRows; row++) {
			// each row

			for (column = 0; column < gridColumns; column++) {
				// each column
				currentCell = Instantiate (gridCell);
				currentCell.transform.SetParent (app.views.duelGrid.transform);
				gridCells [column, row] = currentCell;

				RectTransform cellRect = currentCell.GetComponent<RectTransform> ();
				cellRect.sizeDelta = new Vector2 (cellWidth, cellHeight);
				cellRect.localScale = new Vector3 (1, 1, 1);

				Vector3 cellPosition = new Vector3(0,0,0);
				cellPosition.y = -cellHeight * row + cellHeight * gridRows/2 - cellHeight/2;
				cellPosition.x = cellWidth * column - cellWidth * gridColumns/2 + cellWidth/2;
				cellRect.localPosition = cellPosition;

				if (row >= strikeZonePadding && row < gridRows - strikeZonePadding && column >= strikeZonePadding && column < gridColumns - strikeZonePadding) {
					cellRect.transform.GetComponentInChildren<DuelGridCell> ().dotColor = darkerDotColor;
					cellRect.transform.GetComponentInChildren<RawImage> ().color = darkerDotColor;
				} else {
					cellRect.transform.GetComponentInChildren<DuelGridCell> ().dotColor = normalDotColor;
					cellRect.transform.GetComponentInChildren<RawImage> ().color = normalDotColor;
				}
			}
		}

//		GameObject.Find ("Swing Marker").GetComponent<DragHandler> ().targetPosition = gridCells [3,4].GetComponent<RectTransform> ().localPosition;

//		app.views.duelGrid.SetActive (false);
	}
}
