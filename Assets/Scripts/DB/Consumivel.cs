using UnityEngine;

[System.Serializable]
public class Consumivel
{
    public int id;
    public string nome;
    public bool ehGrande;
    public int qtd;

    [System.NonSerialized]
    public GameObject modelo3D;
}