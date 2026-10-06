using UnityEngine;

public class ChangeValue : MonoBehaviour
{
    public FlockSpawner flockSpawner;

    public void ChangeAlignWeight(float value)
    {
        flockSpawner.alignWeight = value;
    }

    public void ChangeCohesionWeight(float value)
    {
        flockSpawner.cohesionWeight = value;
    }

    public void ChangeSeparationWeight(float value)
    {
        flockSpawner.separationWeight = value;
    }

    public void ChangeFlockAmount(float value)
    {
        flockSpawner.flockCount = Mathf.RoundToInt(value);
    }
}
