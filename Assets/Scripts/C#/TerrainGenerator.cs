using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    public int numberOfPoints;
    public int heightOffset = 0;

    private SpriteShapeController _ss;

    public Vector2 terrainOffset = Vector2.zero;

    void Start()
    {
        _ss = GetComponent<SpriteShapeController>();
        
        terrainOffset = new Vector2(Random.Range(-100000, 100000), Random.Range(-100000, 100000));
        DrawTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor) return;
    }

    public void DrawTerrain()
    {
        // Get Camera Bounds and set bounds variables
        var bounds = GetCameraBounds();
        var end = new Vector2(bounds.xMax, bounds.yMin);
        var maxHeight = bounds.height / 2;
        var range = (bounds.width / numberOfPoints);

        // Clear Sprite Shape and set first position
        var spline = _ss.spline;
        spline.Clear();
        spline.InsertPointAt(0, bounds.min - Vector2.one);
        var lastIndex = 0;

        // Create the terrain
        for (int i = 0; i < numberOfPoints; i++)
        {
            // Calculate X, generate value and create Y based on that
            var x = range * i;
            var value = Mathf.PerlinNoise((x + terrainOffset.x) / 10, terrainOffset.y) + 0.2f; // * Mathf.PerlinNoise((x + terrainOffset)/3, 0);
            var newPoint = new Vector3(x - bounds.width / 2, (value * maxHeight - bounds.height / 2) + heightOffset, -2);
            newPoint.y += .1f;

            // Add point to the Sprite Shape
            spline.InsertPointAt(i+1, newPoint);
            spline.SetTangentMode(i+1, ShapeTangentMode.Broken);
            lastIndex = i+1;
        }

        // Add last point to the Sprite Shape
        spline.InsertPointAt(lastIndex+1, end - new Vector2(-1, 1));
        spline.SetTangentMode(lastIndex+1, ShapeTangentMode.Broken);
    }

    private Rect GetCameraBounds()
    {
        var height = Camera.main.orthographicSize * 2f;
        var width = (height * Screen.width / Screen.height)+2;
        var pos = Camera.main.transform.position;
        
        return new Rect(pos.x - width/2, pos.y - height/2, width, height);
    }
}
