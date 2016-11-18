using UnityEngine;

public class GameManager : Singleton<GameManager> {
	public BuildingButton ClickedBuildingBtn { get; set; }

	[SerializeField]
	private Canvas buildingCanvas;

	[SerializeField]
	private Canvas showBuildingCanvasButton;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			HandleEscape();
	}

	public void PickBuilding(BuildingButton buildingButton) {
		if (ResourceManager.Instance.CheckIfHaveEnough(buildingButton)) { 
			this.ClickedBuildingBtn = buildingButton;
			Hover.Instance.Activate(buildingButton.Sprite);
			ToggleBuildingCanvas();
			showBuildingCanvasButton.enabled = false;
		}
	}

	public void BuyBuilding() {
		for (int i = 0; i < ClickedBuildingBtn.Price.Length; i++)
			ResourceManager.Instance.ResourceValue[i] -= ClickedBuildingBtn.Price[i];

		ResourceManager.Instance.UpdateCurrencyValue();

		if (!ResourceManager.Instance.CheckIfHaveEnough(ClickedBuildingBtn))
			Hover.Instance.Deactivate();
		else if (!Input.GetKey(KeyCode.LeftShift))
			Hover.Instance.Deactivate();
	}

	private void HandleEscape() {
		Hover.Instance.Deactivate();
	}

	public void ToggleBuildingCanvas() {
		if (buildingCanvas.enabled) {
			buildingCanvas.enabled = false;
			showBuildingCanvasButton.enabled = true;
		} else {
			buildingCanvas.enabled = true;
			showBuildingCanvasButton.enabled = false;
		}
	}
}