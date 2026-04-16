using UnityEditor;
using UnityEngine;


[System.Serializable] // alows edit in inspector
public struct Collectable {
    public static Collectable empty = new Collectable();

    public string name;
    public Sprite ground;
    public Sprite small;
    public Sprite big;

    //public atribute boostAtribute;
    //public float boostAmount;
    //public GameObject obj;
}

public enum atribute {
    none,
    walkBoost,
    runBoost,
    jumpBoost
}