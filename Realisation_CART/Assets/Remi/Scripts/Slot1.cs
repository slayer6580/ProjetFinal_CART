﻿using UnityEngine;

public class Slot1 : MonoBehaviour
{
    /// <summary> Sert a tester le placement de slot avec le prefab SlotTest </summary>
    private void Start()
    {
        if (transform.childCount == 1)
        {
            int randomColor = Random.Range(0, 5);
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = GetRandomColor(randomColor);
        }
    }

    private void Update()
    {
        // Sert a tester le placement de slot avec rendu au gameplay (Scene)
        Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.black);
    }

    /// <summary> Donne une couleur random a la slot spécial </summary>
    Color GetRandomColor(int value)
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
    }

}