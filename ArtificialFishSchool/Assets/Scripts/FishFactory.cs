using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FishFactory {

    private Dictionary<string, FishSetting> FishSettingDictionary;

    public GameObject CreateFish(FishSetting fishsetting)
    {
        Dictionary<BehaviorType, bool> enablebehavior = CreateEmptyEnableBehavior();
        foreach (BehaviorType b in fishsetting.BehaviorList)
        {
            enablebehavior[b] = true;
        }
        GameObject fishTemplate = UnityEngine.Object.Instantiate(fishsetting.Prefab);
        fishTemplate.AddComponent<FishController>();
        fishTemplate.GetComponent<FishController>().Initialize(fishsetting.GroupID, enablebehavior, fishsetting.PreyIDList, fishsetting.PredatorIDList);

        return fishTemplate;
    }

    private Dictionary<BehaviorType, bool> CreateEmptyEnableBehavior()
    {
        Dictionary<BehaviorType, bool> empty = new Dictionary<BehaviorType, bool>();
        for (int i = 0; i < Enum.GetNames(typeof(BehaviorType)).Length; i++)
        {
            empty.Add((BehaviorType)i, false);
        }
        return empty;
    }
}
