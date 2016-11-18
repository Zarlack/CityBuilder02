using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : Singleton<TileScript> {
	public Point GridPosition { get; private set; }

	public bool TileOccupied { get; private set; }

	public bool TileWater { get; set; }

	public bool TileSideCorner { get; set; }

	public GameObject TileType { get; set; }

	public string TileGroup { get; set; }

	private Color32 notAllowedTile = new Color32(255, 118, 118, 255);

	private Color32 allowedTile = new Color32(96, 255, 90, 255);

	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetupTile(Point gridPos, Vector2 worldPos, Transform parent, GameObject tileType, string tileGroup) {
		if (tileGroup == "deepWater" || tileGroup == "shallowWater")
			TileWater = true;
		else if (tileGroup == "side" || tileGroup == "corner")
			TileSideCorner = true;

		this.GridPosition = gridPos;
		transform.position = worldPos;
		transform.SetParent(parent);
		TileType = tileType;
		TileGroup = tileGroup;

		if (LevelManager.Instance.TileDictionary.ContainsKey(gridPos)) {
			LevelManager.Instance.TileDictionary[gridPos] = this;
		} else {
			LevelManager.Instance.TileDictionary.Add(gridPos, this);
		}
	}

	private void OnMouseOver() {
		if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBuildingBtn != null) {
			if (!TileWater && !TileSideCorner && !TileOccupied)
				ColorTile(allowedTile);

			if (TileWater || TileSideCorner || TileOccupied)
				ColorTile(notAllowedTile);
			else if (Input.GetMouseButtonDown(0))
				PlaceBuilding();
		}
	}

	private void OnMouseExit() {
		ColorTile(Color.white);
	}

	private void PlaceBuilding() {
		GameObject building = (GameObject)Instantiate(GameManager.Instance.ClickedBuildingBtn.BuildingPrefab, transform.position, Quaternion.identity);
		building.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.y;
		building.transform.SetParent(transform);
		TileOccupied = true;
		ColorTile(Color.white);
		GameManager.Instance.BuyBuilding();
	}

	public void ColorTile(Color32 newColor) {
		spriteRenderer.color = newColor;
	}
}