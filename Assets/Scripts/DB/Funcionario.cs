using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Funcionarios
{
    public int id;
    public string nome;
    public string funcao;

    [System.NonSerialized]
    public GameObject personagem3D;

    [System.NonSerialized]
    public List<Consumivel> consumiveisPegos;
    [System.NonSerialized]
    public List<Consumivel> consumiveisDevolvidos;
    [System.NonSerialized]
    public bool devolucaoCorreta;
}
