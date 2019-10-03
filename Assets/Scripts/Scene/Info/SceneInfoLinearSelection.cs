using UnityEngine;

[System.Serializable]
public class SceneInfoLinearSelection : SceneInfoSetSelection
{
    public SceneInfoLinearSelection()
    {
        this.setSelection = new LinearSelection<SceneGroup.SceneInfo>();
    }
}
