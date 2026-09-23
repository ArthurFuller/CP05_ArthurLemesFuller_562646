using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    [Header("Modelos disponíveis")]
    [Tooltip("Coloque os modelos na ordem em que devem aparecer.")]
    [SerializeField] private GameObject[] models;

    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;
        ShowModel(currentIndex);
    }

    public void NextModel()
    {
        if (models == null || models.Length == 0)
        {
            Debug.LogWarning("[ModelSwitcher] Nenhum modelo foi configurado.");
            return;
        }

        currentIndex = (currentIndex + 1) % models.Length;
        ShowModel(currentIndex);

        Debug.Log($"[ModelSwitcher] Modelo atual: {currentIndex} - {models[currentIndex].name}");
    }

    private void ShowModel(int index)
    {
        for (int i = 0; i < models.Length; i++)
        {
            if (models[i] != null)
                models[i].SetActive(i == index);
        }
    }
}
