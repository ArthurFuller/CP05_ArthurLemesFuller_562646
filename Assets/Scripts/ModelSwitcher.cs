using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] models;
    private int index;

    private void Start()
    {
        if (models == null || models.Length == 0) return;

        foreach (var model in models)
            if (model != null) model.SetActive(false);

        Show(0);
    }

    public void NextModel()
    {
        if (models == null || models.Length < 2) return;
        Show((index + 1) % models.Length);
    }

    private void Show(int next)
    {
        if (models[index] != null) models[index].SetActive(false);

        index = next;
        if (models[index] == null) return;

        models[index].SetActive(true);

        foreach (var lod in models[index].GetComponentsInChildren<LODGroup>(true))
            lod.ForceLOD(0);
    }
}
