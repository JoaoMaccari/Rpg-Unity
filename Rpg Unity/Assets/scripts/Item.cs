using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    public Sprite icon;
    public string Name;
    public float Value;

    [System.Serializable]
    public enum Type {
        potion,
        Elixir,
        Crystal
    }

    public Type Itemtype;

    //o que vai ser feito com o player ao pegar o item
    public void GetAction() {
        switch (Itemtype) {

            case Type.potion:
                Debug.Log("Healt + " + Value);
                break;

            case Type.Elixir:
                Debug.Log("elixir + " + Value);
                break;

            case Type.Crystal:
                Debug.Log("cristal + " + Value);
                break;
        }
    }
}
