using UnityEngine;

[System.Serializable]
public class Consumivel
{
    public int id;
    public string nome;
    public float offSet;
    public int qtd;

    [System.NonSerialized]
    public GameObject modelo3D;
}