using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class TouchManager : MonoBehaviour
{
    [Header("Câmera utilizada pelo Vuforia")]
    [SerializeField] private Camera arCamera;

    private void Awake()
    {
        if (arCamera == null)
            arCamera = GetComponent<Camera>();

        if (arCamera == null)
            Debug.LogError("[TouchManager] Nenhuma Camera foi encontrada no ARCamera.");
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        // Celular usando o Input System novo (configuração atual deste projeto).
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            DetectTouch(touchPosition);
            return;
        }

#if UNITY_EDITOR
        // Permite testar também com clique do mouse no Editor.
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectTouch(Mouse.current.position.ReadValue());
        }
#endif

#elif ENABLE_LEGACY_INPUT_MANAGER
        // Fallback caso o projeto seja alterado para o Input Manager antigo.
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DetectTouch(Input.GetTouch(0).position);
        }
#endif
    }

    private void DetectTouch(Vector2 screenPosition)
    {
        if (arCamera == null)
            return;

        Ray ray = arCamera.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                Mathf.Infinity,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide))
        {
            Debug.Log("[TouchManager] Toque detectado, mas o Raycast não acertou nenhum Collider.");
            return;
        }

        ModelSwitcher switcher = hit.collider.GetComponentInParent<ModelSwitcher>();

        if (switcher == null)
        {
            Debug.Log($"[TouchManager] Raycast acertou '{hit.collider.name}', mas não encontrou ModelSwitcher nos pais.");
            return;
        }

        Debug.Log($"[TouchManager] Toque em '{hit.collider.name}'. Trocando modelo.");
        switcher.NextModel();
    }
}
