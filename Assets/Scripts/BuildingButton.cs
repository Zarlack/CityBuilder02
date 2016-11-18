using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour {
	[SerializeField]
	private GameObject buildingPrefab;

	[SerializeField]
	private Sprite sprite;

	//Gold, wood
	[SerializeField]
	private int[] price;

	[SerializeField]
	private Text[] priceText;

	[SerializeField]
	private Canvas priceCanvas;

	private void Start() {
		for (int i = 0; i < price.Length; i++)
			priceText[i].text = price[i].ToString();
	}

	public GameObject BuildingPrefab {
		get { return buildingPrefab; }
	}

	public Sprite Sprite {
		get { return sprite; }
	}

	public int[] Price {
		get { return price; }
	}

	public void ShowPrice() {
		if (GameManager.Instance.ClickedBuildingBtn == null)
			priceCanvas.enabled = true;
	}

	public void HidePrice() {
		priceCanvas.enabled = false;
	}
}