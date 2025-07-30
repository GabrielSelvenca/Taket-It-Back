using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumivel
{
    public int id;
    public string nome;
    public int qtd;
    public int limitePorFuncionario;
}

[System.Serializable]
public class ListaConsumiveis
{
    public List<Consumivel> listaConsumiveis = new();
}

[System.Serializable]
public class ConsumiveisData
{
    public List<Consumivel> listaConsumiveis;
}