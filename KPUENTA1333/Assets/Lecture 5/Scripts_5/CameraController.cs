using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [FormerlySerializedAs("_zoomSpeed")] [SerializeField] private float ZoomSpeed = 100f;
    [FormerlySerializedAs("_zoomTime")] [SerializeField] private float ZoomTime = 0.1f;

    [FormerlySerializedAs("_maxHeight")] [SerializeField] private float MaxHeight = 100f;
    [FormerlySerializedAs("_minHeight")] [SerializeField] private float MinHeight = 20f;

    [FormerlySerializedAs("_focusHeight")] [SerializeField] private float FocusHeight = 10f;
    [FormerlySerializedAs("_focusDistance")] [SerializeField] private float FocusDistance = 20f;

    [FormerlySerializedAs("_panBorder")] [SerializeField] private int PanBorder = 25;
    [FormerlySerializedAs("_dragPanSpeed")] [SerializeField] private float DragPanSpeed = 25f;
    [FormerlySerializedAs("_edgePanSpeed")] [SerializeField] private float EdgePanSpeed = 25f;
    [FormerlySerializedAs("_keyPanSpeed")] [SerializeField] private float KeyPanSpeed = 25f;

    private float _zoomVelocity = 0f;
    private float _targetHeight;

    private void Start()
    {
        // Start zoomed out
        _targetHeight = MaxHeight;
    }

    private void Update()
    {
        var newPosition = transform.position;

        // First, calculate the height we want the camera to be at
        _targetHeight += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * -1f;
        _targetHeight = Mathf.Clamp(_targetHeight, MinHeight, MaxHeight);

        // Then, interpolate smoothly towards that height
        newPosition.y = Mathf.SmoothDamp(transform.position.y, _targetHeight, ref _zoomVelocity, ZoomTime);

        // Always pan the camera using the keys
        var pan = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * KeyPanSpeed * Time.deltaTime;

        // Optionally pan the camera by either dragging with middle mouse or when the cursor touches the screen border
        if (Input.GetMouseButton(2))
        {
            pan -= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * DragPanSpeed * Time.deltaTime;
        }
        else
        {
            var border = Vector2.zero;
            if (Input.mousePosition.x < PanBorder) border.x -= 1f;
            if (Input.mousePosition.x >= Screen.width - PanBorder) border.x += 1f;
            if (Input.mousePosition.y < PanBorder) border.y -= 1f;
            if (Input.mousePosition.y > Screen.height - PanBorder) border.y += 1f;
            pan += border * EdgePanSpeed * Time.deltaTime;
        }

        newPosition.x += pan.x;
        newPosition.z += pan.y;

        var focusPosition = new Vector3(newPosition.x, FocusHeight, newPosition.z + FocusDistance);

        transform.position = newPosition;
        transform.LookAt(focusPosition);
    }
}
