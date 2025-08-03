using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameData : MonoBehaviour
    {
        public static GameData Instance { get; private set; }

        [Header("JSONs de Dados")]
        public ListaFuncionarios funcionarios;
        public ListaConsumiveis consumiveis;
        public ListaFuncionariosConsumiveis funcionariosConsumiveis;
        public ListaRanking ranking;

        [Header("Referência para busca de personagens")]
        public GameObject listaPersonagens;

        public GameObject listaConsumiveisModels;

        public Transform DropItemsArea;

        private Dictionary<int, Funcionarios> funcionarioPorId = new();

        [HideInInspector]
        public class ModeloConsumivelFuncionario
        {
            public string nomeConsumivel;
            public int quantidade;
            public List<GameObject> modelos = new();
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else { Destroy(gameObject); return; }

            CarregarDados();
            GerarConsumiveis();
            IniciarVinculos();
        }

        private void CarregarDados()
        {
            // Ranking
            string caminhoR = Path.Combine(Application.persistentDataPath, "ranking.json");
            if (File.Exists(caminhoR))
            {
                string json = File.ReadAllText(caminhoR);
                ranking = JsonUtility.FromJson<ListaRanking>(json);
                Debug.Log("Ranking carregado com sucesso.");
            }
            else
            {
                ranking = new ListaRanking();
                Debug.Log("Arquivo de ranking não encontrado. Criando novo.");
            }

            // Funcionários (manual)
            funcionarios = new ListaFuncionarios
            {
                funcionarios = new List<Funcionarios>
                {
                    new Funcionarios { id = 14254, nome = "Carla Menezes", funcao = "TI" },
                    new Funcionarios { id = 9572, nome = "Ricardo Torres", funcao = "TI" },
                    new Funcionarios { id = 31774, nome = "Duda Vasconcelos", funcao = "Áudio" },
                    new Funcionarios { id = 20892, nome = "Jeferson Santana", funcao = "Áudio" },
                    new Funcionarios { id = 18995, nome = "Nara Kim", funcao = "Dev" },
                    new Funcionarios { id = 15537, nome = "Bruno Alves", funcao = "Dev" },
                    new Funcionarios { id = 44026, nome = "Thiago Duarte", funcao = "Marketing" },
                    new Funcionarios { id = 32137, nome = "Camila Bernardes", funcao = "Marketing" },
                }
            };

            // Consumíveis (manual)
            consumiveis = new ListaConsumiveis
            {
                consumiveis = new List<Consumivel>
                {
                    new Consumivel { id = 1, nome = "Pilha", ehGrande = false },
                    new Consumivel { id = 2, nome = "Borracha", ehGrande = false },
                    new Consumivel { id = 3, nome = "Caneta", ehGrande = false },
                    new Consumivel { id = 4, nome = "Grampeador", ehGrande = true },
                    new Consumivel { id = 5, nome = "Lápis", ehGrande = false },
                    new Consumivel { id = 6, nome = "Mouse", ehGrande = true },
                    new Consumivel { id = 7, nome = "Teclados", ehGrande = true },
                    new Consumivel { id = 8, nome = "Cálculadora", ehGrande = true }
                }
            };
        }

        public void GerarConsumiveis()
        {
            funcionariosConsumiveis = new ListaFuncionariosConsumiveis { funcionarioConsumiveis = new List<FuncionarioConsumivel>() };

            foreach (var func in funcionarios.funcionarios)
            {
                int qtdTipos = UnityEngine.Random.Range(1, UnityEngine.Random.Range(5, 8));
                List<int> usados = new();

                for (int i = 0; i < qtdTipos; i++)
                {
                    Consumivel item;
                    do
                    {
                        item = consumiveis.consumiveis[UnityEngine.Random.Range(0, consumiveis.consumiveis.Count)];
                    } while (usados.Contains(item.id));

                    usados.Add(item.id);
                    int qtd = UnityEngine.Random.Range(1, 5);

                    funcionariosConsumiveis.funcionarioConsumiveis.Add(new FuncionarioConsumivel
                    {
                        idFuncionario = func.id,
                        idConsumivel = item.id,
                        qtd = qtd
                    });
                }
            }
        }

        private void IniciarVinculos()
        {
            funcionarioPorId.Clear();

            foreach (var func in funcionarios.funcionarios)
            {
                funcionarioPorId[func.id] = func;

                Transform child = listaPersonagens.transform.Find(func.id.ToString());
                if (child != null)
                {
                    func.personagem3D = child.gameObject;
                    Debug.Log($"Funcionario {func.nome} vinculado ao objeto {child.name}");
                }
                else
                {
                    Debug.LogWarning($"Objeto com nome {func.id} não encontrado");
                }
            }
        }

        public Funcionarios GetFuncionario(int id) => funcionarioPorId.TryGetValue(id, out var func) ? func : null;

        public GameObject GetPersonagem(int id) => GetFuncionario(id)?.personagem3D;

        public List<FuncionarioConsumivel> GetConsFunc(int idFunc) => funcionariosConsumiveis.funcionarioConsumiveis.FindAll(f => f.idFuncionario == idFunc);

        public int GetPontosDoJogador(string nome) => ranking.ranking.Find(p => p.nome == nome)?.pontos ?? 0;

        public void InserirRanking(string nomeJogador, int pontosGanhos)
        {
            var jogador = ranking.ranking.Find(j => j.nome == nomeJogador);

            if (jogador != null)
            {
                jogador.pontos += pontosGanhos;
                Debug.Log($"Jogador {jogador.nome} atualizado com +{pontosGanhos} pontos (Total: {jogador.pontos})");
            }
            else
            {
                ranking.ranking.Add(new PlayerRank { nome = nomeJogador, pontos = pontosGanhos });
                Debug.Log($"Jogador {nomeJogador} adicionado com {pontosGanhos} pontos.");
            }

            string caminho = Path.Combine(Application.persistentDataPath, "ranking.json");
            File.WriteAllText(caminho, JsonUtility.ToJson(ranking, true));
            Debug.Log($"Ranking salvo em {caminho}");
        }

        private Dictionary<string, List<int>> restricoesPorFuncao = new()
        {
            { "Limpeza", new List<int> { 9, 10 } },
            { "TI", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 } },
            { "Dev", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 } },
            { "Marketing", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 } },
            { "Áudio", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 } }
        };

        public bool VerificarEntregasValidas(int idFuncionario)
        {
            var funcionario = GetFuncionario(idFuncionario);
            if (funcionario == null)
            {
                Debug.LogWarning("Funcionário não encontrado.");
                return false;
            }

            if (!restricoesPorFuncao.TryGetValue(funcionario.funcao, out var permitidos))
            {
                Debug.LogWarning($"Função {funcionario.funcao} não tem restrições definidas.");
                return false;
            }

            var entregas = GetConsFunc(idFuncionario);
            foreach (var entrega in entregas)
            {
                if (!permitidos.Contains(entrega.idConsumivel))
                {
                    var nomeItem = consumiveis.consumiveis.Find(c => c.id == entrega.idConsumivel)?.nome ?? "Desconhecido";
                    Debug.LogWarning($"Item inválido: {nomeItem} ({entrega.idConsumivel}) entregue por {funcionario.nome} ({funcionario.funcao})");
                    return false;
                }
            }

            return true;
        }

        public List<ModeloConsumivelFuncionario> BuscarModelosConsumiveisFuncionario(int idFuncionario)
        {
            var resultado = new List<ModeloConsumivelFuncionario>();

            // Pega todos os consumíveis desse funcionário
            var consumiveisFuncionario = GetConsFunc(idFuncionario);

            foreach (var entrega in consumiveisFuncionario)
            {
                var cons = consumiveis.consumiveis.Find(c => c.id == entrega.idConsumivel);
                if (cons == null)
                {
                    Debug.LogWarning($"Consumível com ID {entrega.idConsumivel} não encontrado.");
                    continue;
                }

                Transform objetoConsumivel = listaConsumiveisModels.transform.Find(cons.nome);
                if (objetoConsumivel == null)
                {
                    Debug.LogWarning($"Objeto '{cons.nome}' não encontrado em '{listaConsumiveisModels.name}'.");
                    continue;
                }

                var modelos = new List<GameObject>();

                if (objetoConsumivel.childCount > 0)
                {
                    for (int i = 0; i < objetoConsumivel.childCount; i++)
                    {
                        modelos.Add(objetoConsumivel.GetChild(i).gameObject);
                    }
                    Debug.Log($"Encontrados {modelos.Count} modelos para '{cons.nome}'.");
                }
                else
                {
                    modelos.Add(objetoConsumivel.gameObject);
                    Debug.Log($"Usando objeto '{cons.nome}' como modelo único (sem filhos).");
                }

                resultado.Add(new ModeloConsumivelFuncionario
                {
                    nomeConsumivel = cons.nome,
                    quantidade = entrega.qtd,
                    modelos = modelos
                });
            }

            return resultado;
        }
    }
}