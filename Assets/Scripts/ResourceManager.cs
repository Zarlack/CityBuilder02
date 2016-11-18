using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : Singleton<ResourceManager> {
	[SerializeField]
	private int[] resourceValue;

	[SerializeField]
	private Text[] resourceText;

	[SerializeField]
	private Color32 notEnoughResourceColor = new Color32(255, 118, 118, 255);

	[SerializeField]
	private Color32 enoughResourceColor = new Color32(96, 255, 90, 255);

	public int[] ResourceValue {
		get { return resourceValue; }
	}

	void Start() {
		UpdateCurrencyValue();
	}

	public void UpdateCurrencyValue() {
		for (int i = 0; i < ResourceValue.Length; i++)
			this.resourceText[i].text = ResourceValue[i].ToString();
	}

	public bool CheckIfHaveEnough(BuildingButton buildingButton) {
		bool returnValue = true;

		for (int i = 0; i < buildingButton.Price.Length; i++)
			if (resourceValue[i] < buildingButton.Price[i])
				returnValue = false;

		return returnValue;
	}

	public void SetResourceColor(BuildingButton buildingButton) {
		if (GameManager.Instance.ClickedBuildingBtn == null)
			for (int i = 0; i < buildingButton.Price.Length; i++)
				if (buildingButton.Price[i] != 0) {
					if (resourceValue[i] >= buildingButton.Price[i]) resourceText[i].color = enoughResourceColor;
					else resourceText[i].color = notEnoughResourceColor;
				}
	}

	public void ResetResourceColor() {
		for (int i = 0; i < resourceText.Length; i++)
			resourceText[i].color = Color.white;
	}
}