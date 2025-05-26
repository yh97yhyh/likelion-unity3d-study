
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshHightlighter : MonoBehaviour
{
    [SerializeField] List<SkinnedMeshRenderer> meshsToHighlight;
    [SerializeField] Material originalMaterial;
    [SerializeField] Material highlightedMaterial;

    public void HighlightMesh(bool highlight)
    {
        foreach (var mesh in meshsToHighlight)
        {
            mesh.material = (highlight) ? highlightedMaterial : originalMaterial;
        }
    }
}
