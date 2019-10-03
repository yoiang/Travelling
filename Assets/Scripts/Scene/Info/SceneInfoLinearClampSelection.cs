using UnityEngine;

[System.Serializable]
public class SceneInfoLinearClampSelection : SceneInfoSetSelection
{
    public SceneInfoLinearClampSelection()
    {
        this.setSelection = new LinearClampSelection<SceneGroup.SceneInfo>();
    }
}
