using UnityEngine;

public class Hover : Singleton<Hover> {
	private SpriteRenderer spriteRenderer;

	void Start () {
		this.spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if (spriteRenderer.enabled)
			FollowMouse();
	}

	private void FollowMouse() {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(transform.position.x, transform.position.y, 10);
	}

	public void Activate(Sprite sprite) {
		this.spriteRenderer.sprite = sprite;
		spriteRenderer.enabled = true;
	}

	public void Deactivate() {
		spriteRenderer.enabled = false;
		GameManager.Instance.ClickedBuildingBtn = null;
		GameManager.Instance.ToggleBuildingCanvas();
	}
}