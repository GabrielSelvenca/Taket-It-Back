using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Funcionario
{
    public int id;
    public string Credencial;
    public string nome;
    public string funcao;
    public ListaConsumiveis itensPegosHoje;
}