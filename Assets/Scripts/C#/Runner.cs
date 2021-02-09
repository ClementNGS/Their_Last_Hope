using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [Header("Paramètres de vitesse")]
    public Range speed;
    
    [Tooltip("En pixels/s²")]
    public float acceleration = 1f;
    
    [Header("Autres")]
    public bool isRunning = true;

    private float _speed;
    private float _timer = 0;
    
    private TerrainGenerator _tg;

    public Runner instance;
    public float currentSpeed => _speed;
    
    void Start()
    {
        if (instance == null)
            instance = this;
        
        _tg = GetComponent<TerrainGenerator>();
        
        Debug.Assert(_tg);

        _speed = speed.min;
    }
    
    void Update()
    {
        if (!isRunning) return;
        
        _tg.terrainOffset.x += Time.deltaTime * _speed;

        _speed += Time.deltaTime * acceleration;
        _speed = speed.Clamp(_speed);
        
        _tg.DrawTerrain();
        
        Variables.ActiveScene.Set("RunnerSpeed", _speed * Time.deltaTime);
    }
}
