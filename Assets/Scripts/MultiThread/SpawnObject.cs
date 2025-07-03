using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    
    [SerializeField] float radius;
    
    [SerializeField] int count;
    

    private float fps;
    

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition =  UnityEngine.Random.insideUnitSphere * radius;
            randomPosition.y = 0; // Keep objects on the ground plane
            Instantiate(prefab, randomPosition, Quaternion.identity , transform);
        }
    }
    
    private void Update()
    {
        // Calculate FPS
        fps = 1.0f / Time.deltaTime;
    }

    private void OnGUI()
    {
        //Draw FPS 
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        if(fps > 60)
            style.normal.textColor = Color.green;
        else if(fps > 30)
            style.normal.textColor = Color.yellow;
        else
            style.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 10, 200, 20), $"FPS: {fps}", style);
    }
}
