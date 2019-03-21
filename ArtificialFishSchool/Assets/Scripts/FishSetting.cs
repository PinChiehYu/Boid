using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FishSetting : ScriptableObject {

    public GameObject Prefab;
    public int GroupID;
    public List<BehaviorType> BehaviorList;
    public List<int> PreyIDList;
    public List<int> PredatorIDList;
}
