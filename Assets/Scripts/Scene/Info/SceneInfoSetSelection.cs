using UnityEngine;

[System.Serializable]
public class SceneInfoSetSelection: SceneGroup.SceneInfoSelection {
    public ISetSelection<SceneGroup.SceneInfo> setSelection;

    override public SceneGroup.SceneInfo GetNext(SceneGroup.SceneInfo[] from) {
        return this.setSelection.GetNext(from);
    }
};