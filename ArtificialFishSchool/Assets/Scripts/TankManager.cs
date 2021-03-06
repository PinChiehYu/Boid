﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankManager : MonoBehaviour {

    // Use this for initialization
    public static TankManager Instance { get; private set; }

    private FishFactory fishFactory;

    [SerializeField]
    private FishSetting fishsettingA;
    [SerializeField]
    private FishSetting fishsettingB;
    [SerializeField]
    private FishSetting predatorsetting;

    [SerializeField]
    public GameObject FoodClone;
    [SerializeField]
    private GameObject WallClone;
    [SerializeField]
    private float SchoolSize;
    [SerializeField]
    private float TankSize;

    private List<FishController> FishControllerList;
    private List<GameObject> FoodList;

    [SerializeField]
    private float NeighborDistance;
    [SerializeField]
    private float CrowdDistance;


    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);

            InitializeTank();
        }
    }

    private void InitializeTank()
    {
        Instantiate(WallClone, new Vector3(TankSize, 0, 0), Quaternion.Euler(90f, 0f, 90f));
        Instantiate(WallClone, new Vector3(-1 * TankSize, 0, 0), Quaternion.Euler(-90f, 0f, -90f));

        Instantiate(WallClone, new Vector3(0, TankSize, 0), Quaternion.Euler(0f, 0f, 180f));
        Instantiate(WallClone, new Vector3(0, -1 * TankSize, 0), Quaternion.Euler(0f, 0f, 0f));

        Instantiate(WallClone, new Vector3(0, 0, TankSize), Quaternion.Euler(-90f, 0, 0f));
        Instantiate(WallClone, new Vector3(0, 0, -1 * TankSize), Quaternion.Euler(90f, 0, 0f));

        FishControllerList = new List<FishController>();
        FoodList = new List<GameObject>();

        fishFactory = new FishFactory();

        for (int i = 0; i < SchoolSize; i++)
        {
            CreateAndAddFish(fishsettingA);
        }
        for (int i = 0; i < SchoolSize; i++)
        {
            CreateAndAddFish(fishsettingB);
        }
        for (int i = 0; i < 10; i++)
        {
            CreateAndAddFish(predatorsetting);
        }
    }

    public void Update()
    {
        foreach (FishController fish in FishControllerList)
        {
            fish.Steering();
        }

        foreach (FishController fish in FishControllerList)
        {
            fish.Locomotion();
        }
    }
    
    private void CreateAndAddFish(FishSetting setting)
    {
        FishController fishController = fishFactory.CreateFish(setting).GetComponent<FishController>();
        FishControllerList.Add(fishController);
    }

    public float GetTankSize()
    {
        return TankSize;
    }

    public List<FishController> GetControllerList()
    {
        return FishControllerList;
    }

    public List<GameObject> GetFoodList()
    {
        return FoodList;
    }

    public float GetNeighborDistance()
    {
        return NeighborDistance;
    }

    public float GetCrowdDistance()
    {
        return CrowdDistance;
    }

    public void UpdateNeighborDistance(Slider slider)
    {
        NeighborDistance = slider.value;
    }

    public void UpdateCrowdDistance(Slider slider)
    {
        CrowdDistance = slider.value;
    }
}
