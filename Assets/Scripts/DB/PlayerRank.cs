using UnityEngine;

[System.Serializable]
public class PlayerRank
{
    private string _nome;

    public string nome
    {
        get => _nome;
        set => _nome = (value != null && value.Length > 5) ? value.Substring(0, 5) : value;
    }
    public int pontos;
}