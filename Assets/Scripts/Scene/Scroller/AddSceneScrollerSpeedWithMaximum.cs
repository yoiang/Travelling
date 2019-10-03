public class AddSceneScrollerSpeedWithMaximum : SceneGroup.SceneInfo.ModifySceneScrollerSpeed {
    public float value;
    public float maximum;

    override public float Modify(float previous) {
        var result = previous + this.value;
        if (result > this.maximum) {
            return maximum;
        }

        return result;
    }
}