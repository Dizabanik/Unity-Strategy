using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MinimapState
{
	Hover,
	Unhover,
	Other
}

public class MiniMap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Camera _mainCamera;
	public Camera RaycastCamera;
	private Camera _minimapCamera;
	private Vector3 _startPoint;
	private Vector3 _cameraStartPosition;
	private Plane _plane;
	bool isStartMoving = false;
	public MinimapState CurrentMinimapState = MinimapState.Other;

	MoveCameraBoundary MoveCameraBoundary;

	private void Start()
    {
		_mainCamera = Camera.main;
		_minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Camera>();
		_plane = new Plane(Vector3.up, Vector3.zero);
		MoveCameraBoundary = FindObjectOfType<MoveCameraBoundary>();
	}

	private void Update()
	{
		if (CurrentMinimapState == MinimapState.Hover)
		{
			MovingMinimap();
			ScalingMinimap();
			ClickMinimap();
		}
	}

	public void ClickMinimap()
	{
		if (Input.GetMouseButton(0))
		{
			Rect minimapRect = GetComponent<RectTransform>().rect;

/*			Rect screenRect = new Rect(
				transform.position.x,
				transform.position.y,
				miniMapRect.width, 
				miniMapRect.height
			);*/

			Vector3 mousePos = Input.mousePosition;
			mousePos.y -= transform.position.y;
			mousePos.x -= transform.position.x;

			_mainCamera.transform.position = new Vector3(
				mousePos.x * (_minimapCamera.orthographicSize * 2 / minimapRect.width) + _minimapCamera.transform.position.x,
				_mainCamera.transform.position.y,
				mousePos.y * (_minimapCamera.orthographicSize * 2 / minimapRect.height) - MoveCameraBoundary.OffsetPositionZ + _minimapCamera.transform.position.z
			);
		}
	}

	void MovingMinimap()
    {
		Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

		float distance;
		_plane.Raycast(ray, out distance);
		Vector3 point = ray.GetPoint(distance);

		if (Input.GetMouseButtonDown(2))
		{
			_startPoint = point;
			_cameraStartPosition = _minimapCamera.transform.position;
			isStartMoving = true;
		}

		if (Input.GetMouseButton(2) && isStartMoving)
		{
			Vector3 offset = point - _startPoint;
			_minimapCamera.transform.position = _cameraStartPosition - offset;
		}

		if (Input.GetMouseButtonUp(2))
		{
			isStartMoving = false;
		}

		float offset2 = 50 - _minimapCamera.orthographicSize;

		_minimapCamera.transform.position = new Vector3(
			Mathf.Clamp(_minimapCamera.transform.position.x, -offset2, offset2),
			_minimapCamera.transform.position.y,
			Mathf.Clamp(_minimapCamera.transform.position.z, -offset2, offset2)
		);
	}

	void ScalingMinimap()
    {
		_minimapCamera.orthographicSize -= Input.mouseScrollDelta.y;
		_minimapCamera.orthographicSize = Mathf.Clamp(_minimapCamera.orthographicSize, 10, 50);
		RaycastCamera.orthographicSize -= Input.mouseScrollDelta.y;
		RaycastCamera.orthographicSize = Mathf.Clamp(RaycastCamera.orthographicSize, 10, 50);
	}

	public void OnPointerEnter(PointerEventData eventData)
    {
		_mainCamera.GetComponent<CameraMove>().enabled = false;
		CurrentMinimapState = MinimapState.Hover;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_mainCamera.GetComponent<CameraMove>().enabled = true;
		CurrentMinimapState = MinimapState.Unhover;
	}
}
