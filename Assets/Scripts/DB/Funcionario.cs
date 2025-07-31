using UnityEngine;

[System.Serializable]
public class Funcionarios
{
    public int id;
    public string nome;
    public string funcao;

    [System.NonSerialized]
    public GameObject personagem3D;
}
