public class SetAbsoluteSceneScrollerSpeed : SceneGroup.SceneInfo.ModifySceneScrollerSpeed {
    public float value;

    public SetAbsoluteSceneScrollerSpeed() {
        var modifyValue = new SetAbsoluteValue<float>();
        modifyValue.absoluteValue = value;
        this.modifyValue = modifyValue;
    }
}