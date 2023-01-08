using UnityEngine;

public class GroundSizeAdjuster : MonoBehaviour
{
    [SerializeField] IntObject selectDifficulty;
    [SerializeField] Vector2IntObject[] mazeDimensionObjects;

    const float ScaleFactor = 0.02f;

    void Start()
    {
        Vector2Int mazeDimensions = mazeDimensionObjects[selectDifficulty.value].value;
        float x = mazeDimensions.y * ScaleFactor;
        float y = mazeDimensions.x * ScaleFactor;

        transform.localScale = new(x, y, 1);
    }
}