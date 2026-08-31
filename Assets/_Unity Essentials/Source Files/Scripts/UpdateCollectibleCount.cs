using UnityEngine;
using TMPro;
using System;

public class UpdateCollectibleCount : MonoBehaviour
{
    private TextMeshProUGUI collectibleText;

    private int totalCollectibles;

    void Start()
    {
        collectibleText = GetComponent<TextMeshProUGUI>();

        if (collectibleText == null)
        {
            Debug.LogError("UpdateCollectibleCount requires a TextMeshProUGUI component.");

            return;
        }

        UpdateCollectibleDisplay();
    }

    private void UpdateCollectibleDisplay()
    {
        totalCollectibles = 0;

        Type collectibleType = Type.GetType("Collectible");

        if (collectibleType != null)
        {
            totalCollectibles += FindObjectsByType(collectibleType).Length;
        }

        Type collectible2DType = Type.GetType("Collectible2D_Custom");

        if (collectible2DType != null)
        {
            totalCollectibles += FindObjectsByType(collectible2DType).Length;
        }

        collectibleText.text = $"Collectibles remaining: {totalCollectibles}";
    }

    public void SubtractCollectible()
    {
        totalCollectibles -= 1;

        collectibleText.text = $"Collectibles remaining: {totalCollectibles}";

        GetComponent<AudioSource>().Play();
    }
}