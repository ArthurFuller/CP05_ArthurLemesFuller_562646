using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private Camera cam;

    private void Awake() => cam = GetComponent<Camera>();

    private void Update()
    {
        if (Touchscreen.current == null ||
            !Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return;

        var ray = cam.ScreenPointToRay(
            Touchscreen.current.primaryTouch.position.ReadValue());

        if (Physics.Raycast(ray, out var hit))
            hit.collider.GetComponentInParent<ModelSwitcher>()?.NextModel();
    }
}
