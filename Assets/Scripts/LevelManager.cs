using System;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager> {
	[SerializeField]
	private GameObject[] tilePrefabs;

	[SerializeField]
	private GameObject[] tilePrefabsEdge;

	private Point centerIslandTilePoint;

	//Up, Right, Down, Left
	private Point[] edgeIslandTilePoint = new Point[4];

	[SerializeField]
	private CameraMovement cameraMovement;

	[SerializeField]
	private Transform map;

	[SerializeField]
	private int mapX;

	[SerializeField]
	private int mapY;

	[SerializeField]
	private int minIslandSize;

	[SerializeField]
	private int maxIslandSize;

	private Vector2 worldStart;

	private System.Random randomNumber = new System.Random();

	public Dictionary<Point, TileScript> TileDictionary { get; set; }

	public float TileSize {
		get { return TilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
	}

	public GameObject[] TilePrefabs {
		get { return tilePrefabs; }
	}

	public GameObject[] TilePrefabsEdge {
		get { return tilePrefabsEdge; }
	}

	void Start () {
		MapBuilder(minIslandSize, maxIslandSize);
	}

	private void MapBuilder(int minSize, int maxSize) {
		CreateWaterStart();
		CreateIslandStart();

		CrossIslandBuilder(0, 0, -1, minSize, maxSize);
		CrossIslandBuilder(1, 1, 0, minSize, maxSize);
		CrossIslandBuilder(2, 0, 1, minSize, maxSize);
		CrossIslandBuilder(3, -1, 0, minSize, maxSize);

		CircleIslandBuilder(1, 0, 1, -1, maxSize);
		CircleIslandBuilder(1, 2, 1, 1, maxSize);
		CircleIslandBuilder(3, 0, -1, -1, maxSize);
		CircleIslandBuilder(3, 2, -1, 1, maxSize);

		FreshWaterBuilder();
	}

	private void CreateWaterStart() {
		TileDictionary = new Dictionary<Point, TileScript>();

		Vector2 maxTile = Vector2.zero;

		worldStart = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height));

		for (int y = 0; y < mapY; y++)
			for (int x = 0; x < mapX; x++)
				PlaceTile(TilePrefabs[0], x, y, "deepWater");

		maxTile = TileDictionary[new Point(mapX - 1, mapY - 1)].transform.position;

		cameraMovement.SetLimits(new Vector2(maxTile.x + TileSize, maxTile.y - TileSize));
	}

	private void CreateIslandStart() {
		centerIslandTilePoint = new Point(mapX / 2, mapY / 2);

		PlaceTile(TilePrefabs[3], centerIslandTilePoint.x, centerIslandTilePoint.y, "grassLand");
		PlaceTile(TilePrefabs[7], centerIslandTilePoint.x, centerIslandTilePoint.y, "grassLand");
	}

	private void PlaceTile(GameObject tileType, int x, int y, string tileGroup) {
		TileScript newTile = Instantiate(tileType).GetComponent<TileScript>();

		newTile.SetupTile(new Point(x, y), new Vector2(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y)), map, tileType, tileGroup);
	}

	private int rng(int minNum, int maxNum) {
		return randomNumber.Next(minNum, maxNum + 1);
	}

	private void CrossIslandBuilder(int index, int xOperator, int yOperator, int minSize, int maxSize) {
		for (int i = 1; i < minSize + 1; i++)
			PlaceTile(TilePrefabs[3], centerIslandTilePoint.x + i * xOperator, centerIslandTilePoint.y + i * yOperator, "grassLand");

		edgeIslandTilePoint[index] = new Point(centerIslandTilePoint.x + minSize * xOperator, centerIslandTilePoint.y + minSize * yOperator);

		/*
		for (int i = minSize + 1; i <= maxSize; i++) {
			if (rng(minSize, maxSize) > i - 1) {
				PlaceTile(tilePrefabs[3], centerIslandTilePoint.x + i * xOperator, centerIslandTilePoint.y + i * yOperator);
				edgeIslandTilePoint[index] = new Point(centerIslandTilePoint.x + i * xOperator, centerIslandTilePoint.y + i * yOperator);
			} else break;
		}
		*/
	}

	private void CircleIslandBuilder(int xIndex, int yIndex, int xOperator, int yOperator, int maxSize) {
		int xLength = Math.Abs(centerIslandTilePoint.x - edgeIslandTilePoint[xIndex].x);
		int yLength = Math.Abs(centerIslandTilePoint.y - edgeIslandTilePoint[yIndex].y);

		int toPlace = 0;	

		//Normally -1 after xLength & yLength
		if (yLength <= xLength)		toPlace = yLength;
		else						toPlace = xLength;

		for (int x = 1; x < maxSize; x++) {
			for (int y = toPlace; y > 0; y--)
				PlaceTile(TilePrefabs[3], centerIslandTilePoint.x + x * xOperator, centerIslandTilePoint.y + y * yOperator, "grassLand");
			toPlace -= 1;
		}
	}

	private void FreshWaterBuilder() {
		for (int y = 0; y < mapY; y++)
			for (int x = 0; x < mapX; x++)
				if (TileDictionary[new Point(x, y)].TileType == TilePrefabs[3]) {
					if (TileDictionary[new Point(x - 1, y)].TileType == TilePrefabs[0])			PlaceTile(TilePrefabs[1], x - 1, y, "shallowWater");
					if (TileDictionary[new Point(x + 1, y)].TileType == TilePrefabs[0])			PlaceTile(TilePrefabs[1], x + 1, y, "shallowWater");
					if (TileDictionary[new Point(x, y - 1)].TileType == TilePrefabs[0])			PlaceTile(TilePrefabs[1], x, y - 1, "shallowWater");
					if (TileDictionary[new Point(x, y + 1)].TileType == TilePrefabs[0])			PlaceTile(TilePrefabs[1], x, y + 1, "shallowWater");
					if (TileDictionary[new Point(x - 1, y - 1)].TileType == TilePrefabs[0])		PlaceTile(TilePrefabs[1], x - 1, y - 1, "shallowWater");
					if (TileDictionary[new Point(x + 1, y + 1)].TileType == TilePrefabs[0])		PlaceTile(TilePrefabs[1], x + 1, y + 1, "shallowWater");
					if (TileDictionary[new Point(x + 1, y - 1)].TileType == TilePrefabs[0])		PlaceTile(TilePrefabs[1], x + 1, y - 1, "shallowWater");
					if (TileDictionary[new Point(x - 1, y + 1)].TileType == TilePrefabs[0])		PlaceTile(TilePrefabs[1], x - 1, y + 1, "shallowWater");

					EdgeChecker(x, y);
				}
	}

	private void EdgeChecker(int x, int y) {
		//0 == top, then clockwise
		bool[] sidesWithWater = new bool[4];

		if (TileDictionary[new Point(x, y - 1)].TileType == TilePrefabs[1])		sidesWithWater[0] = true;
		if (TileDictionary[new Point(x + 1, y)].TileType == TilePrefabs[1])		sidesWithWater[1] = true;
		if (TileDictionary[new Point(x, y + 1)].TileType == TilePrefabs[1])		sidesWithWater[2] = true;
		if (TileDictionary[new Point(x - 1, y)].TileType == TilePrefabs[1])		sidesWithWater[3] = true;

		if (sidesWithWater[0] && !sidesWithWater[1] && !sidesWithWater[3])		PlaceTile(TilePrefabsEdge[0], x, y, "side");
		if (sidesWithWater[1] && !sidesWithWater[0] && !sidesWithWater[2])		PlaceTile(TilePrefabsEdge[2], x, y, "side");
		if (sidesWithWater[2] && !sidesWithWater[1] && !sidesWithWater[3])		PlaceTile(TilePrefabsEdge[4], x, y, "side");
		if (sidesWithWater[3] && !sidesWithWater[2] && !sidesWithWater[0])		PlaceTile(TilePrefabsEdge[6], x, y, "side");

		if (sidesWithWater[0] && sidesWithWater[1])								PlaceTile(TilePrefabsEdge[1], x, y, "side");
		if (sidesWithWater[1] && sidesWithWater[2])								PlaceTile(TilePrefabsEdge[3], x, y, "side");
		if (sidesWithWater[2] && sidesWithWater[3])								PlaceTile(TilePrefabsEdge[5], x, y, "side");
		if (sidesWithWater[3] && sidesWithWater[0])								PlaceTile(TilePrefabsEdge[7], x, y, "side");

		if (!sidesWithWater[0] && !sidesWithWater[1] && !sidesWithWater[2] && !sidesWithWater[3]) {
			if (TileDictionary[new Point(x + 1, y - 1)].TileType == TilePrefabs[1])		PlaceTile(TilePrefabsEdge[8], x, y, "corner");
			if (TileDictionary[new Point(x + 1, y + 1)].TileType == TilePrefabs[1])		PlaceTile(TilePrefabsEdge[9], x, y, "corner");
			if (TileDictionary[new Point(x - 1, y + 1)].TileType == TilePrefabs[1])		PlaceTile(TilePrefabsEdge[10], x, y, "corner");
			if (TileDictionary[new Point(x - 1, y - 1)].TileType == TilePrefabs[1])		PlaceTile(TilePrefabsEdge[11], x, y, "corner");
		}
	}
}