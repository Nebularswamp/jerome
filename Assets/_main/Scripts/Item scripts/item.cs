using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item : MonoBehaviour
{
    public string itemName = "itemName";
    public GameObject myPrefab;
    public int damage = 1;

    private void Start() {

    }

    public virtual void use() {
        
    }

    public virtual void discard() {
    }
}
