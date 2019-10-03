using UnityEngine;

[System.Serializable]
public class SceneInfoRandomWithoutAdjacentDuplicatesSetSelection : SceneInfoSetSelection
{
    [Range(0.0f, 1.0f)]
    public float allowDuplicatesRate;

    public SceneInfoRandomWithoutAdjacentDuplicatesSetSelection()
    {
        var result = new RandomWithoutAdjacentDuplicatesSelection<SceneGroup.SceneInfo>();
        result.allowDuplicatesRate = this.allowDuplicatesRate;
        this.setSelection = result;
    }
}