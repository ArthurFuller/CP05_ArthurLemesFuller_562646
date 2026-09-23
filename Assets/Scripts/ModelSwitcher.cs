using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    [Header("Modelos disponíveis")]
    [Tooltip("Coloque os modelos na ordem em que devem aparecer.")]
    [SerializeField] private GameObject[] models;

    [Header("LOD")]
    [Tooltip("Mantém sempre o modelo completo (LOD0), evitando que partes desapareçam quando a câmera se afasta.")]
    [SerializeField] private bool forceHighestLOD = true;

    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;

        ConfigureLODs();
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

    private void ConfigureLODs()
    {
        if (models == null)
            return;

        int lodGroupsFound = 0;

        foreach (GameObject model in models)
        {
            if (model == null)
                continue;

            LODGroup[] lodGroups = model.GetComponentsInChildren<LODGroup>(true);
            lodGroupsFound += lodGroups.Length;

            foreach (LODGroup lodGroup in lodGroups)
            {
                // 0 = sempre usa o LOD de maior qualidade.
                // -1 = devolve o controle para o sistema automático do Unity.
                lodGroup.ForceLOD(forceHighestLOD ? 0 : -1);
            }
        }

        if (forceHighestLOD && lodGroupsFound > 0)
        {
            Debug.Log($"[ModelSwitcher] {lodGroupsFound} LODGroup(s) fixados em LOD0 para evitar desaparecimento por distância.");
        }
    }

    private void ShowModel(int index)
    {
        for (int i = 0; i < models.Length; i++)
        {
            if (models[i] == null)
                continue;

            bool shouldBeActive = i == index;
            models[i].SetActive(shouldBeActive);

            if (shouldBeActive && forceHighestLOD)
            {
                LODGroup[] lodGroups = models[i].GetComponentsInChildren<LODGroup>(true);

                foreach (LODGroup lodGroup in lodGroups)
                    lodGroup.ForceLOD(0);
            }
        }
    }
}
