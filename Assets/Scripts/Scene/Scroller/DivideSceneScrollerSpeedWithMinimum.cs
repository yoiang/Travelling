public class DivideSceneScrollerSpeedWithMinimum : SceneGroup.SceneInfo.ModifySceneScrollerSpeed {
    public float value;
    public float minimum;

    override public float Modify(float previous) {
        var result = previous / this.value;
        if (result < this.minimum) {
            return minimum;
        }

        return result;
    }
}