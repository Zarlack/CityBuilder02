using UnityEngine;

public class CameraMovement : MonoBehaviour {
	[SerializeField]
	private float cameraWASDSpeed = 0;

	[SerializeField]
	private float cameraEdgeSpeed = 0;

	private float xMax;
	private float yMin;

	private string direction;

	private bool edgeMousedOver = false;

	private void Update () {
		if (!edgeMousedOver)	GetInput();
		else					CameraEdgeMovement();
	}

	private void GetInput() {
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))		transform.Translate(Vector2.up * cameraWASDSpeed * Time.deltaTime);
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))		transform.Translate(Vector2.left * cameraWASDSpeed * Time.deltaTime);
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))		transform.Translate(Vector2.down * cameraWASDSpeed * Time.deltaTime);
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))	transform.Translate(Vector2.right * cameraWASDSpeed * Time.deltaTime);

		//KEEP Vector3
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);
	}

	public void SetLimits(Vector2 maxTile) {
		Vector2 wp = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));

		xMax = maxTile.x - wp.x;
		yMin = maxTile.y - wp.y;
	}

	public void ToggleCameraEdgeMovement(string directionRecived) {
		direction = directionRecived;
		edgeMousedOver = !edgeMousedOver;
	}

	private void CameraEdgeMovement() {
		if (direction == "Up")			transform.Translate(Vector2.up * cameraEdgeSpeed * Time.deltaTime);
		if (direction == "Left")		transform.Translate(Vector2.left * cameraEdgeSpeed * Time.deltaTime);
		if (direction == "Down")		transform.Translate(Vector2.down * cameraEdgeSpeed * Time.deltaTime);
		if (direction == "Right")		transform.Translate(Vector2.right * cameraEdgeSpeed * Time.deltaTime);

		if (direction == "LeftUp")		transform.Translate((Vector2.left + Vector2.up) * cameraEdgeSpeed * Time.deltaTime);
		if (direction == "UpRight")		transform.Translate((Vector2.up + Vector2.right) * cameraEdgeSpeed * Time.deltaTime);
		if (direction == "RightDown")	transform.Translate((Vector2.down + Vector2.right) * cameraEdgeSpeed * Time.deltaTime);
		if (direction == "DownLeft")	transform.Translate((Vector2.down + Vector2.left) * cameraEdgeSpeed * Time.deltaTime);

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);
	}
}