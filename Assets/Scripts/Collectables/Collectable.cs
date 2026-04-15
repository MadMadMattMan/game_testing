using UnityEditor;
using UnityEngine;


[System.Serializable] // alows display in inspector
public struct Collectable {
    public static Collectable empty = new Collectable();

    public Sprite sprite;

    public item item;
    public atribute boostAtribute;
    public float boostAmount;
    public GameObject obj;

    public Collectable(Sprite sprite, item item, atribute boost, float amount, GameObject prefab) {
        this.sprite = sprite;
        this.item = item;
        this.boostAtribute = boost;
        this.boostAmount = amount;
        this.obj = prefab;
    }
}

public enum atribute {
    none,
    speedMulti,
    jumpMulit
}
public enum item {
    none, 
}