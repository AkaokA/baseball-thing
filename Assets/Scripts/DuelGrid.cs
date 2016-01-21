using UnityEngine;
using System.Collections;

public class DuelGrid : BaseballElement {

	public AnimationCurve easingCurve;

	private int gridRows = 9;
	private int gridColumns = 7;
	private float gridPadding = 44f;

	private float cellWidth;
	private float cellHeight;
	private float cellPadding = 0;

	private RectTransform duelGridRectTransform;

	private GameObject[,] gridCells;
	public GameObject gridCell;

	void Start () {
		gridCells = new GameObject[gridRows, gridColumns];

		duelGridRectTransform = app.views.duelGrid.GetComponent<RectTransform> ();

		cellHeight = (duelGridRectTransform.rect.height - gridPadding) / gridRows;
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
				currentCell.transform.parent = app.views.duelGrid.transform;
				currentCell.transform.localScale = new Vector3(cellWidth - cellPadding/2, cellHeight - cellPadding/2, 1);
				gridCells [row, column] = currentCell;

				Vector3 cellPosition = new Vector3(0,0,0);
				cellPosition.y = -cellHeight * row + cellHeight * gridRows/2 - cellHeight/2;
				cellPosition.x = cellWidth * column - cellWidth * gridColumns/2 + cellWidth/2;
				currentCell.transform.localPosition = cellPosition;

//				Quaternion cellRotation;
//				currentCell.transform.rotation = Quaternion.identity;
			}
		}

//		app.views.duelGrid.SetActive (false);
	}








//	IEnumerator DropCells () {
//		float time = 10f;
//
//		for ( float currentLerpTime = 0f; currentLerpTime <= time; currentLerpTime += Time.deltaTime ) {
//			float perc = currentLerpTime / time;
//			float cellAltitude;
//
//			foreach (GameObject cell in gridCells) {
//				float initialAltitude = 0;
//
//				bool first = true;
//				if (first) {
//					initialAltitude = cell.transform.localPosition.z;
//					first = false;
//				}
//
//				cellAltitude = Mathf.LerpUnclamped (initialAltitude, 0f, easingCurve.Evaluate (perc));
//				Vector3 cellPosition = cell.transform.localPosition;
//				cellPosition.z = cellAltitude;
//
//				cell.transform.localPosition = cellPosition;
//			}
//			yield return null;
//		}
//	}
}
