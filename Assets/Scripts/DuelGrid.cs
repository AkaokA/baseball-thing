using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DuelGrid : BaseballElement {

	public AnimationCurve easingCurve;

	public static int gridRows = 9;
	public static int gridColumns = 7;
	public static int strikeZoneWidth = 3;
	public static int strikeZoneHeight = 5;

	private float cellWidth;
	private float cellHeight;

	private RectTransform duelGridRect;

	public GameObject[,] gridCells;
	public GameObject gridCell;

	private Color normalDotColor = new Color (1f, 1f, 1f, 0.33f);
	private Color darkerDotColor = new Color (1f, 1f, 1f, 1f);

	void Start () {
		SetUpDuelGrid ();
	}

	void SetUpDuelGrid () {
		gridCells = new GameObject[gridColumns, gridRows];

		duelGridRect = app.views.duelGrid.GetComponent<RectTransform> ();

		if (duelGridRect.rect.width < duelGridRect.rect.height) {
			cellHeight = duelGridRect.rect.height / gridRows;
			cellWidth = cellHeight;
		} else {
			cellWidth = duelGridRect.rect.width / gridColumns;
			cellHeight = cellWidth;
		}

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
				currentCell.transform.SetAsFirstSibling ();
				gridCells [column, row] = currentCell;

				RectTransform cellRect = currentCell.GetComponent<RectTransform> ();
				cellRect.sizeDelta = new Vector2 (cellWidth, cellHeight);
				cellRect.localScale = new Vector3 (1, 1, 1);

				Vector3 cellPosition = new Vector3 (0, 0, 0);
				cellPosition.y = -cellHeight * row + cellHeight * gridRows / 2 - cellHeight / 2;
				cellPosition.x = cellWidth * column - cellWidth * gridColumns / 2 + cellWidth / 2;
				cellRect.localPosition = cellPosition;

				int strikeZoneWidthPadding = (gridColumns - strikeZoneWidth) / 2;
				int strikeZoneHeightPadding = (gridRows - strikeZoneHeight) / 2;

				if (column >= strikeZoneWidthPadding && column < gridColumns - strikeZoneWidthPadding && row >= strikeZoneHeightPadding && row < gridRows - strikeZoneHeightPadding) {
					cellRect.transform.FindChild ("Dot").GetComponent<Image> ().color = darkerDotColor;
				} else {
					cellRect.transform.FindChild ("Dot").GetComponent<Image> ().color = normalDotColor;
				}
			}
		}
	}

}
