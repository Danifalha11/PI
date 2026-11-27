using System;
using MySql.Data.MySqlClient;

public enum NivelUrgencia
{
    Baixa,
    Media,
    Alta
}

public class Necessitado
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Cidade { get; set; }
    public string? Bairro { get; set; }
    public string? Nacionalidade { get; set; }
    public string? Telefone { get; set; }
    public NivelUrgencia NivelUrgencia { get; set; }

    public Necessitado(string? nome, string? email, string? cidade,
        string? bairro, string? nacionalidade, string? telefone,
        NivelUrgencia nivelUrgencia)
    {
        Nome = nome;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
        NivelUrgencia = nivelUrgencia;
    }

    public static Necessitado? CadastrarNecessitado()
    {
        Console.WriteLine("\n--- Cadastro de Necessitado ---");

        Console.Write("Nome: ");
        string? nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome inválido.");
            return null;
        }

        Console.Write("Email: ");
        string? email = Console.ReadLine();

        Console.Write("Cidade: ");
        string? cidade = Console.ReadLine();

        Console.Write("Bairro: ");
        string? bairro = Console.ReadLine();

        Console.Write("Nacionalidade: ");
        string? nacionalidade = Console.ReadLine();

        Console.Write("Telefone: ");
        string? telefone = Console.ReadLine();

        Console.WriteLine("\nNível de urgência:");
        Console.WriteLine("0. Baixa");
        Console.WriteLine("1. Média");
        Console.WriteLine("2. Alta");
        Console.Write("Escolha: ");

        if (!int.TryParse(Console.ReadLine(), out int nivel) ||
            nivel < 0 || nivel > 2)
        {
            Console.WriteLine("Nível inválido.");
            return null;
        }

        return new Necessitado(nome, email, cidade, bairro,
            nacionalidade, telefone, (NivelUrgencia)nivel);
    }

    public bool Salvar()
    {
        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = @"INSERT INTO Necessitado
                (Nome, Email, Cidade, Bairro, Nacionalidade, Telefone, NivelUrgencia)
                VALUES
                (@Nome, @Email, @Cidade, @Bairro, @Nacionalidade, @Telefone, @NivelUrgencia)";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@Nome", Nome);
                comando.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Cidade", (object?)Cidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Bairro", (object?)Bairro ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Nacionalidade", (object?)Nacionalidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Telefone", (object?)Telefone ?? DBNull.Value);
                comando.Parameters.AddWithValue("@NivelUrgencia", (int)NivelUrgencia);

                comando.ExecuteNonQuery();

                Id = Convert.ToInt32(comando.LastInsertedId);

                Console.WriteLine("Necessitado cadastrado com sucesso!");
                return true;
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao cadastrar: " + erro.Message);
            return false;
        }
    }

    public static void ListarNecessitados()
    {
        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = "SELECT * FROM Necessitado";

                MySqlCommand comando = new MySqlCommand(sql, conexao);
                MySqlDataReader leitor = comando.ExecuteReader();

                bool encontrou = false;

                Console.WriteLine("\n--- Lista de Necessitados ---");

                while (leitor.Read())
                {
                    encontrou = true;

                    Necessitado necessitado = new Necessitado(
                        leitor["Nome"].ToString(),
                        leitor["Email"].ToString(),
                        leitor["Cidade"].ToString(),
                        leitor["Bairro"].ToString(),
                        leitor["Nacionalidade"].ToString(),
                        leitor["Telefone"].ToString(),
                        (NivelUrgencia)Convert.ToInt32(leitor["NivelUrgencia"])
                    );

                    necessitado.Id = Convert.ToInt32(leitor["Id"]);

                    necessitado.ExibirCadastro();
                    Console.WriteLine("----------------------------");
                }

                leitor.Close();

                if (!encontrou)
                    Console.WriteLine("Nenhum necessitado cadastrado.");
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao listar: " + erro.Message);
        }
    }

    public static Necessitado? BuscarNecessitadoPorId(int id)
    {
        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = "SELECT * FROM Necessitado WHERE Id = @Id";

                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", id);

                MySqlDataReader leitor = comando.ExecuteReader();

                if (leitor.Read())
                {
                    Necessitado necessitado = new Necessitado(
                        leitor["Nome"].ToString(),
                        leitor["Email"].ToString(),
                        leitor["Cidade"].ToString(),
                        leitor["Bairro"].ToString(),
                        leitor["Nacionalidade"].ToString(),
                        leitor["Telefone"].ToString(),
                        (NivelUrgencia)Convert.ToInt32(leitor["NivelUrgencia"])
                    );

                    necessitado.Id = Convert.ToInt32(leitor["Id"]);

                    leitor.Close();
                    return necessitado;
                }

                leitor.Close();
                return null;
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao buscar: " + erro.Message);
            return null;
        }
    }

    public bool Atualizar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
        {
            Console.WriteLine("O nome não pode ficar vazio.");
            return false;
        }

        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = @"UPDATE Necessitado SET
                Nome = @Nome,
                Email = @Email,
                Cidade = @Cidade,
                Bairro = @Bairro,
                Nacionalidade = @Nacionalidade,
                Telefone = @Telefone,
                NivelUrgencia = @NivelUrgencia
                WHERE Id = @Id";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@Id", Id);
                comando.Parameters.AddWithValue("@Nome", Nome);
                comando.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Cidade", (object?)Cidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Bairro", (object?)Bairro ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Nacionalidade", (object?)Nacionalidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Telefone", (object?)Telefone ?? DBNull.Value);
                comando.Parameters.AddWithValue("@NivelUrgencia", (int)NivelUrgencia);

                int resultado = comando.ExecuteNonQuery();

                return resultado > 0;
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao atualizar: " + erro.Message);
            return false;
        }
    }

    public static void AtualizarNecessitado()
    {
        Console.Write("Digite o ID do necessitado: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Necessitado? necessitado = BuscarNecessitadoPorId(id);

        if (necessitado == null)
        {
            Console.WriteLine("Necessitado não encontrado.");
            return;
        }

        Console.WriteLine("\nDeixe vazio para manter o valor atual.");

        necessitado.Nome = LerCampo("Nome", necessitado.Nome);
        necessitado.Email = LerCampo("Email", necessitado.Email);
        necessitado.Cidade = LerCampo("Cidade", necessitado.Cidade);
        necessitado.Bairro = LerCampo("Bairro", necessitado.Bairro);
        necessitado.Nacionalidade = LerCampo("Nacionalidade", necessitado.Nacionalidade);
        necessitado.Telefone = LerCampo("Telefone", necessitado.Telefone);

        Console.WriteLine("\nNível de urgência:");
        Console.WriteLine("0. Baixa");
        Console.WriteLine("1. Média");
        Console.WriteLine("2. Alta");
        Console.Write("Novo nível (ou Enter para manter): ");

        string? entrada = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(entrada))
        {
            if (int.TryParse(entrada, out int nivel) &&
                nivel >= 0 && nivel <= 2)
            {
                necessitado.NivelUrgencia = (NivelUrgencia)nivel;
            }
            else
            {
                Console.WriteLine("Nível inválido. Mantendo o anterior.");
            }
        }

        if (necessitado.Atualizar())
            Console.WriteLine("Necessitado atualizado com sucesso!");
        else
            Console.WriteLine("Não foi possível atualizar.");
    }

    public static string? LerCampo(string nomeCampo, string? valorAtual)
    {
        Console.Write(nomeCampo + " (atual: " + valorAtual + "): ");

        string? novoValor = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(novoValor))
            return valorAtual;

        return novoValor;
    }

    public static void DeletarNecessitado()
    {
        Console.Write("Digite o ID do necessitado: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Necessitado? necessitado = BuscarNecessitadoPorId(id);

        if (necessitado == null)
        {
            Console.WriteLine("Necessitado não encontrado.");
            return;
        }

        Console.Write("Deseja excluir " + necessitado.Nome + "? (S/N): ");
        string? resposta = Console.ReadLine();

        if (resposta?.ToUpper() != "S")
        {
            Console.WriteLine("Exclusão cancelada.");
            return;
        }

        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = "DELETE FROM Necessitado WHERE Id = @Id";

                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", id);

                int resultado = comando.ExecuteNonQuery();

                if (resultado > 0)
                    Console.WriteLine("Necessitado excluído com sucesso!");
                else
                    Console.WriteLine("Nenhum registro foi excluído.");
            }
        }
        catch (MySqlException erro)
        {
            if (erro.Number == 1451)
                Console.WriteLine("Não é possível excluir: existem itens vinculados.");
            else
                Console.WriteLine("Erro ao excluir: " + erro.Message);
        }
    }

    public void ExibirCadastro()
    {
        Console.WriteLine("ID: " + Id);
        Console.WriteLine("Nome: " + Nome);
        Console.WriteLine("Email: " + Email);
        Console.WriteLine("Cidade: " + Cidade);
        Console.WriteLine("Bairro: " + Bairro);
        Console.WriteLine("Nacionalidade: " + Nacionalidade);
        Console.WriteLine("Telefone: " + Telefone);
        Console.WriteLine("Nível de urgência: " + NivelUrgencia);
    }
}
