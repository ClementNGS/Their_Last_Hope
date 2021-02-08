using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainGenerator : MonoBehaviour
{
    public int numberOfPoints;
    public int heightOffset = 0;

    private LineRenderer _lr;
    private PolygonCollider2D _pc;
    private SpriteShapeController _ss;

    public float terrainOffset = 0;

    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _pc = GetComponent<PolygonCollider2D>();

        Debug.Assert(_lr);

        terrainOffset += Random.Range(-100000, 100000);
        DrawTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            terrainOffset -= Time.deltaTime * 5;
            DrawTerrain();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            terrainOffset += Time.deltaTime * 5;
            DrawTerrain();
        }
    }

    void DrawTerrain()
    {
        // Reset points
        _lr.SetPositions(new Vector3[0] { });
        _lr.positionCount = numberOfPoints;

        var bounds = GetCameraBounds();

        _pc.SetPath(0, new Vector2[0] { });
        var end = new Vector2(bounds.xMax, bounds.yMin);
        var pointsArr = new List<Vector2>() { bounds.min };


        var maxHeight = bounds.height / 2;
        var range = (bounds.width / numberOfPoints);

        for (int i = 0; i < numberOfPoints; i++)
        {
            var x = range * i;
            var value = Mathf.PerlinNoise((x + terrainOffset) / 10, 0) + 0.2f; // * Mathf.PerlinNoise((x + terrainOffset)/3, 0);
            var newPoint = new Vector3(x - bounds.width / 2, (value * maxHeight - bounds.height / 2) + heightOffset, -2);
            _lr.SetPosition(i, newPoint);
            if (i % 10 == 0)
            {
                pointsArr.Add(newPoint);
            }
        }

        
        pointsArr.Add(end);

        _pc.SetPath(0, pointsArr);
    }

    private Rect GetCameraBounds()
    {
        var height = Camera.main.orthographicSize * 2f;
        var width = (height * Screen.width / Screen.height)+2;
        var pos = Camera.main.transform.position;
        
        return new Rect(pos.x - width/2, pos.y - height/2, width, height);
    }
}
