using UnityEngine;

[System.Serializable]
public class SceneInfoRandomWithoutReplacementSetSelection : SceneInfoSetSelection
{
    public SceneInfoRandomWithoutReplacementSetSelection()
    {
        this.setSelection = new RandomWithoutReplacementSelection<SceneGroup.SceneInfo>();
    }
}
