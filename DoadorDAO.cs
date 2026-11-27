using System;
using MySql.Data.MySqlClient;

public class Doador
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Cidade { get; set; }
    public string? Bairro { get; set; }
    public string? Nacionalidade { get; set; }
    public string? Telefone { get; set; }

    public Doador(string? nome, string? email, string? cidade,
        string? bairro, string? nacionalidade, string? telefone)
    {
        Nome = nome;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
    }

    public static Doador? CadastrarDoador()
    {
        Console.WriteLine("\n--- Cadastro de Doador ---");

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

        return new Doador(nome, email, cidade, bairro, nacionalidade, telefone);
    }

    public bool Salvar()
    {
        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = @"INSERT INTO Doador
                (Nome, Email, Cidade, Bairro, Nacionalidade, Telefone)
                VALUES
                (@Nome, @Email, @Cidade, @Bairro, @Nacionalidade, @Telefone)";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@Nome", Nome);
                comando.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Cidade", (object?)Cidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Bairro", (object?)Bairro ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Nacionalidade", (object?)Nacionalidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Telefone", (object?)Telefone ?? DBNull.Value);

                comando.ExecuteNonQuery();

                Id = Convert.ToInt32(comando.LastInsertedId);

                Console.WriteLine("Doador cadastrado com sucesso!");
                return true;
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao salvar doador: " + erro.Message);
            return false;
        }
    }

    public void ExibirDoadores()
    {
        Console.WriteLine("ID: " + Id);
        Console.WriteLine("Nome: " + Nome);
        Console.WriteLine("Email: " + Email);
        Console.WriteLine("Cidade: " + Cidade);
        Console.WriteLine("Bairro: " + Bairro);
        Console.WriteLine("Nacionalidade: " + Nacionalidade);
        Console.WriteLine("Telefone: " + Telefone);
    }

    public static void ListarDoadores()
    {
        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = "SELECT * FROM Doador";

                MySqlCommand comando = new MySqlCommand(sql, conexao);
                MySqlDataReader leitor = comando.ExecuteReader();

                if (!leitor.HasRows)
                {
                    Console.WriteLine("Nenhum doador encontrado.");
                    leitor.Close();
                    return;
                }

                while (leitor.Read())
                {
                    Doador doador = new Doador(
                        leitor["Nome"].ToString(),
                        leitor["Email"].ToString(),
                        leitor["Cidade"].ToString(),
                        leitor["Bairro"].ToString(),
                        leitor["Nacionalidade"].ToString(),
                        leitor["Telefone"].ToString()
                    );

                    doador.Id = Convert.ToInt32(leitor["Id"]);

                    doador.ExibirDoadores();
                    Console.WriteLine("----------------------------");
                }

                leitor.Close();
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao listar doadores: " + erro.Message);
        }
    }

    public static Doador? BuscarDoadorPorId(int id)
    {
        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = "SELECT * FROM Doador WHERE Id = @Id";

                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", id);

                MySqlDataReader leitor = comando.ExecuteReader();

                if (leitor.Read())
                {
                    Doador doador = new Doador(
                        leitor["Nome"].ToString(),
                        leitor["Email"].ToString(),
                        leitor["Cidade"].ToString(),
                        leitor["Bairro"].ToString(),
                        leitor["Nacionalidade"].ToString(),
                        leitor["Telefone"].ToString()
                    );

                    doador.Id = Convert.ToInt32(leitor["Id"]);

                    leitor.Close();
                    return doador;
                }

                leitor.Close();
                Console.WriteLine("Doador não encontrado.");
                return null;
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao buscar doador: " + erro.Message);
            return null;
        }
    }

    public static string? LerCampo(string nomeCampo, string? valorAtual)
    {
        Console.Write(nomeCampo + " (atual: " + valorAtual + "): ");

        string? novoValor = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(novoValor))
        {
            return valorAtual;
        }

        return novoValor;
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

                string sql = @"UPDATE Doador SET
                Nome = @Nome,
                Email = @Email,
                Cidade = @Cidade,
                Bairro = @Bairro,
                Nacionalidade = @Nacionalidade,
                Telefone = @Telefone
                WHERE Id = @Id";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@Nome", Nome);
                comando.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Cidade", (object?)Cidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Bairro", (object?)Bairro ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Nacionalidade", (object?)Nacionalidade ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Telefone", (object?)Telefone ?? DBNull.Value);
                comando.Parameters.AddWithValue("@Id", Id);

                int resultado = comando.ExecuteNonQuery();

                if (resultado > 0)
                {
                    Console.WriteLine("Doador atualizado com sucesso!");
                    return true;
                }

                Console.WriteLine("Nenhum doador encontrado.");
                return false;
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao atualizar doador: " + erro.Message);
            return false;
        }
    }

    public bool Deletar()
    {
        try
        {
            using (MySqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                string sql = "DELETE FROM Doador WHERE Id = @Id";

                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", Id);

                int resultado = comando.ExecuteNonQuery();

                if (resultado > 0)
                {
                    Console.WriteLine("Doador deletado com sucesso!");
                    return true;
                }

                Console.WriteLine("Doador não encontrado.");
                return false;
            }
        }
        catch (MySqlException erro)
        {
            if (erro.Number == 1451)
            {
                Console.WriteLine("Não é possível excluir: este doador possui itens cadastrados.");
            }
            else
            {
                Console.WriteLine("Erro ao deletar doador: " + erro.Message);
            }

            return false;
        }
    }

    public static Doador? AtualizarDoador()
    {
        Console.Write("Digite o ID do doador: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return null;
        }

        Doador? doador = BuscarDoadorPorId(id);

        if (doador == null)
        {
            return null;
        }

        doador.ExibirDoadores();

        Console.WriteLine("\nDeixe vazio para manter o valor atual.");

        doador.Nome = LerCampo("Nome", doador.Nome);
        doador.Email = LerCampo("Email", doador.Email);
        doador.Cidade = LerCampo("Cidade", doador.Cidade);
        doador.Bairro = LerCampo("Bairro", doador.Bairro);
        doador.Nacionalidade = LerCampo("Nacionalidade", doador.Nacionalidade);
        doador.Telefone = LerCampo("Telefone", doador.Telefone);

        if (doador.Atualizar())
        {
            return doador;
        }

        return null;
    }

    public static Doador? DeletarDoador()
    {
        Console.Write("Digite o ID do doador: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return null;
        }

        Doador? doador = BuscarDoadorPorId(id);

        if (doador == null)
        {
            return null;
        }

        doador.ExibirDoadores();

        Console.Write("Deseja realmente excluir? (S/N): ");
        string? resposta = Console.ReadLine();

        if (resposta?.ToUpper() != "S")
        {
            Console.WriteLine("Exclusão cancelada.");
            return null;
        }

        if (doador.Deletar())
        {
            return doador;
        }

        return null;
    }
}
