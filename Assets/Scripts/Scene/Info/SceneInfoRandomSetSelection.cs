using UnityEngine;

[System.Serializable]
public class SceneInfoRandomSetSelection : SceneInfoSetSelection
{
    public SceneInfoRandomSetSelection()
    {
        this.setSelection = new RandomSelection<SceneGroup.SceneInfo>();
    }
}