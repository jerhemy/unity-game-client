﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Client.Entity;
using Net;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Manages all entities in the scene
/// </summary>
public class ClientEntityManager : MonoBehaviour
{
    /// <summary>
    /// Holds client entity
    /// </summary>
    private PlayerEntity _playerEntity;
    private GameObject _playerGameObject;
    
    
    /// <summary>
    /// Collection holds all entities in scene
    /// </summary>
    private ConcurrentDictionary<long, EntityGO> _entity;
    
    public GameObject activeContainer;

    [SerializeField]
    private int objectPoolSize = 2000;
    private List<GameObject> objectPool;
    
    public static ClientEntityManager instance;
    
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;

        }
        //If instance already exists and it's not this:
        else if (instance != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
            
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);             
    }
    
    // Start is called before the first frame update
    void Start()
    {      
        EventManager.Subscribe(OP.ServerAddEntity, AddEntity);
        if (_entity == null)
        {
            _entity = new ConcurrentDictionary<long, EntityGO>();
            
            activeContainer = new GameObject("Active");
            activeContainer.transform.position = new Vector3(0,0,0);
            activeContainer.transform.parent = transform;                     
        } 
        
        objectPool = new List<GameObject>();
        for (var count = 0; count < objectPoolSize; count++)
        {
            var newGO = new GameObject();
            var entityGO = newGO.AddComponent<EntityGO>();
            
            #if UNITY_EDITOR
            var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.position = new Vector3(0f, 0.5f, 0f);
            box.transform.parent = newGO.transform;
            #endif
            
            objectPool.Add(newGO);
        }
    }
    
    private IEnumerator Process()
    {
        while (true)
        {
            // Get all EntityGO objects active in scene
            var entities = _entity.Where(entity => entity.Value.gameObject.activeInHierarchy).Select( go => go.Value);
            
            // Run Update
            foreach (var go in entities)
            {
                go.GetComponent<EntityGO>().Process();
            }
            
            yield return new WaitForSeconds(0.1f);
        }

    }

    private long GetID()
    {
        return DateTime.UtcNow.Ticks;
    }

    
    public void AddEntity(NetworkPacket packet)
    {
        // On creation we use the Mob object as this is the initial state
        // For Updates we use the GameObject as that will maintain current state
        
        // Find available object from pool
        var go = GetFreeObject();
        
        var id = GetID();
        go.SetID(id);
        //go.AddEntity(e);

        _entity.TryAdd(id, go);
        
        go.Spawn();       
    }


    public void SetPlayerID(long id)
    {
        
    }
    
    public void CreatePlayer(long id, PlayerEntity client)
    {   
        _playerGameObject = new GameObject("Player");
        var entityGO = _playerGameObject.AddComponent<EntityGO>();
        entityGO.AddEntity(client);
        
        entityGO.Spawn();
    }
    
    private EntityGO GetFreeObject()
    {
        var go = objectPool.FirstOrDefault(g => g.gameObject.activeInHierarchy == false);
        if (go) return go.GetComponent<EntityGO>();
        
        var newGO = new GameObject();
        var entityGO = newGO.AddComponent<EntityGO>();
        objectPool.Add(newGO);
        return entityGO;
    }

    public EntityGO GetTargetByID(long id)
    {
        _entity.TryGetValue(id, out var entity);
        return entity;
    }
}

