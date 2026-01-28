using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class Utilities
{
    public static bool IsSameVectorPosition(Vector3 positionA, Vector3 positionB, float tolerance = .2f)
    {
        return Mathf.Abs(positionA.x - positionB.x) < tolerance
            && Mathf.Abs(positionA.y - positionB.y) < tolerance
            && Mathf.Abs(positionA.z - positionB.z) < tolerance;
    }

    public static float GetDistanceBetween(Vector2 positionA, Vector2 positionB)
    {
        return Vector2.Distance(positionA, positionB);
    }

    public static float GetDistanceBetween(Vector3 positionA, Vector3 positionB)
    {
        return Vector3.Distance(positionA, positionB);
    }

    public static float GetDistanceBetween(Vector3Int positionA, Vector3Int positionB)
    {
        return Vector3Int.Distance(positionA, positionB);
    }

    public static bool TestRoll(float chancePercent)
    {
        return Random.Range(0f, 100f) < chancePercent;
    }

    public static float GetRandomFloat(float lowerBounds, float upperBounds)
    {
        return Random.Range(lowerBounds, upperBounds);
    }

    public static int GetRandomInt(int lowerBounds, int upperBounds)
    {
        return Random.Range(lowerBounds, ++upperBounds);
    }

    public static float RoundToHundredths(float toRound)
    {
        return Mathf.Round(toRound * 100f) / 100f;
    }

    public static int RountToInt(float toRound)
    {
        return Mathf.RoundToInt(toRound);
    }

    public static async UniTask FadeInUIElement(Graphic uiElement, float holdSeconds = .25f, float fadeSeconds = 1f)
    {
        Color originalColor = uiElement.color;
        originalColor.a = 1f;
        uiElement.color = originalColor;

        await UniTask.Delay((int)(holdSeconds * 1000));

        float elapsed = 0f;
        while (elapsed < fadeSeconds)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeSeconds);
            Color fadeColor = uiElement.color;
            fadeColor.a = Mathf.Lerp(0f, 1f, t);
            uiElement.color = fadeColor;
            await UniTask.Yield();
        }

        originalColor.a = 1f;
        uiElement.color = originalColor;
    }
}
