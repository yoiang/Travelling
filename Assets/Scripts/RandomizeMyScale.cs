using UnityEngine;

public class RandomizeMyScale : MonoBehaviour
{
    [FloatMinMaxRange(0.0f, 100.0f)]
    public FloatMinMaxRange scale;

    public bool uniformComponents = true;
    public bool randomizeOnStart = true;

    void Start() {
        if (this.randomizeOnStart) {
            this.Randomize();
        }
    }

    void Randomize() {
        if (this.uniformComponents) {
            var scale = this.scale.GetRandomValue();
            this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        } else {
            this.gameObject.transform.localScale = new Vector3(
                this.scale.GetRandomValue(), 
                this.scale.GetRandomValue(), 
                this.scale.GetRandomValue()
            );
        }
    }
}