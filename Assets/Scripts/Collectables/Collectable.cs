using UnityEditor;
using UnityEngine;


[System.Serializable] // alows display in inspector
public struct Collectable {
    public static Collectable empty = new Collectable();

    public Sprite sprite;
    public Animation collectAnimation;

    public atribute boostAtribute;
    public float boostAmount;
    public GameObject obj;

    public Collectable(Sprite sprite, Animation animation, atribute boost, float amount, GameObject prefab) {
        this.sprite = sprite;
        this.collectAnimation = animation;
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