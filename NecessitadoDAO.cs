using System;
using System.Collections.Generic;
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

    public List<Item> ItensNecessarios { get; set; } = new List<Item>();

    public Necessitado(
        string? nome,
        string? email,
        string? cidade,
        string? bairro,
        string? nacionalidade,
        string? telefone,
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
            Console.WriteLine("O nome é obrigatório.");
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
            !Enum.IsDefined(typeof(NivelUrgencia), nivel))
        {
            Console.WriteLine("Nível de urgência inválido.");
            return null;
        }

        return new Necessitado(
            nome,
            email,
            cidade,
            bairro,
            nacionalidade,
            telefone,
            (NivelUrgencia)nivel
        );
    }

    // Salva no MySQL
    public bool Salvar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
        {
            Console.WriteLine("O nome é obrigatório.");
            return false;
        }

        if (!Enum.IsDefined(typeof(NivelUrgencia), NivelUrgencia))
        {
            Console.WriteLine("Nível de urgência inválido.");
            return false;
        }

        try
        {
            using MySqlConnection conexao = new Conexao().Conectar();
            conexao.Open();

            string sql = @"INSERT INTO Necessitado
                (Nome, Email, Cidade, Bairro, Nacionalidade,
                 Telefone, NivelUrgencia)
                VALUES
                (@Nome, @Email, @Cidade, @Bairro, @Nacionalidade,
                 @Telefone, @NivelUrgencia)";

            using MySqlCommand comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@Nome", Nome);
            comando.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Cidade", (object?)Cidade ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Bairro", (object?)Bairro ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Nacionalidade", (object?)Nacionalidade ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Telefone", (object?)Telefone ?? DBNull.Value);
            comando.Parameters.AddWithValue("@NivelUrgencia", (int)NivelUrgencia);

            comando.ExecuteNonQuery();

            Id = Convert.ToInt32(comando.LastInsertedId);

            return true;
        }
        catch (MySqlException erro)
        {
            Console.WriteLine("Erro ao salvar necessitado: " + erro.Message);
            return false;
        }
    }

    public static void ListarNecessitados()
    {
        try
        {
            using MySqlConnection conexao = new Conexao().Conectar();
            conexao.Open();

            string sql = "SELECT * FROM Necessitado";

            using MySqlCommand comando = new MySqlCommand(sql, conexao);
            using MySqlDataReader reader = comando.ExecuteReader();

            Console.WriteLine("\n--- Lista de Necessitados ---");

            bool encontrou = false;

            while (reader.Read())
            {
                encontrou = true;

                int nivel = Convert.ToInt32(reader["NivelUrgencia"]);

                Necessitado necessitado = new Necessitado(
                    reader["Nome"].ToString(),
                    reader["Email"] as string,
                    reader["Cidade"] as string,
                    reader["Bairro"] as string,
                    reader["Nacionalidade"] as string,
                    reader["Telefone"] as string,
                    (NivelUrgencia)nivel
                );

                necessitado.Id = Convert.ToInt32(reader["Id"]);

                necessitado.ExibirCadastro();
                Console.WriteLine("-----------------------------------");
            }

            if (!encontrou)
                Console.WriteLine("Nenhum necessitado cadastrado.");
        }
        catch (MySqlException erro)
        {
            Console.WriteLine("Erro ao listar necessitados: " + erro.Message);
        }
    }

    public static Necessitado? BuscarNecessitadoPorId(int id)
    {
        try
        {
            using MySqlConnection conexao = new Conexao().Conectar();
            conexao.Open();

            string sql = "SELECT * FROM Necessitado WHERE Id = @Id";

            using MySqlCommand comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@Id", id);

            using MySqlDataReader reader = comando.ExecuteReader();

            if (reader.Read())
            {
                int nivel = Convert.ToInt32(reader["NivelUrgencia"]);

                Necessitado necessitado = new Necessitado(
                    reader["Nome"].ToString(),
                    reader["Email"] as string,
                    reader["Cidade"] as string,
                    reader["Bairro"] as string,
                    reader["Nacionalidade"] as string,
                    reader["Telefone"] as string,
                    (NivelUrgencia)nivel
                );

                necessitado.Id = Convert.ToInt32(reader["Id"]);

                return necessitado;
            }
        }
        catch (MySqlException erro)
        {
            Console.WriteLine("Erro ao buscar necessitado: " + erro.Message);
        }

        return null;
    }

    public bool Atualizar()
    {
        if (Id <= 0 || string.IsNullOrWhiteSpace(Nome))
            return false;

        try
        {
            using MySqlConnection conexao = new Conexao().Conectar();
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

            using MySqlCommand comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@Id", Id);
            comando.Parameters.AddWithValue("@Nome", Nome);
            comando.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Cidade", (object?)Cidade ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Bairro", (object?)Bairro ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Nacionalidade", (object?)Nacionalidade ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Telefone", (object?)Telefone ?? DBNull.Value);
            comando.Parameters.AddWithValue("@NivelUrgencia", (int)NivelUrgencia);

            int linhas = comando.ExecuteNonQuery();

            return linhas > 0;
        }
        catch (MySqlException erro)
        {
            Console.WriteLine("Erro ao atualizar necessitado: " + erro.Message);
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

        Console.Write($"Nome ({necessitado.Nome}): ");
        string? nome = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nome))
            necessitado.Nome = nome;

        Console.Write($"Email ({necessitado.Email}): ");
        string? email = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(email))
            necessitado.Email = email;

        Console.Write($"Cidade ({necessitado.Cidade}): ");
        string? cidade = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(cidade))
            necessitado.Cidade = cidade;

        Console.Write($"Bairro ({necessitado.Bairro}): ");
        string? bairro = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(bairro))
            necessitado.Bairro = bairro;

        Console.Write($"Nacionalidade ({necessitado.Nacionalidade}): ");
        string? nacionalidade = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nacionalidade))
            necessitado.Nacionalidade = nacionalidade;

        Console.Write($"Telefone ({necessitado.Telefone}): ");
        string? telefone = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(telefone))
            necessitado.Telefone = telefone;

        Console.WriteLine("\nNível de urgência:");
        Console.WriteLine("0. Baixa");
        Console.WriteLine("1. Média");
        Console.WriteLine("2. Alta");
        Console.Write($"Atual: {(int)necessitado.NivelUrgencia}. Deseja alterar? ");

        string? alterarNivel = Console.ReadLine();

        if (alterarNivel == "1")
        {
            Console.Write("Novo nível: ");

            if (int.TryParse(Console.ReadLine(), out int nivel) &&
                Enum.IsDefined(typeof(NivelUrgencia), nivel))
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

        Console.Write($"Deseja excluir {necessitado.Nome}? (s/n): ");
        string? confirmacao = Console.ReadLine();

        if (confirmacao?.ToLower() != "s")
        {
            Console.WriteLine("Exclusão cancelada.");
            return;
        }

        try
        {
            using MySqlConnection conexao = new Conexao().Conectar();
            conexao.Open();

            string sql = "DELETE FROM Necessitado WHERE Id = @Id";

            using MySqlCommand comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@Id", id);

            int linhas = comando.ExecuteNonQuery();

            if (linhas > 0)
                Console.WriteLine("Necessitado excluído com sucesso!");
            else
                Console.WriteLine("Nenhum registro foi excluído.");
        }
        catch (MySqlException)
        {
            Console.WriteLine(
                "Não foi possível excluir. Verifique se existem itens vinculados a esse necessitado."
            );
        }
    }

    public void ExibirCadastro()
    {
        Console.WriteLine("\n--- Cadastro do Necessitado ---");
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Cidade: {Cidade}");
        Console.WriteLine($"Bairro: {Bairro}");
        Console.WriteLine($"Nacionalidade: {Nacionalidade}");
        Console.WriteLine($"Telefone: {Telefone}");
        Console.WriteLine($"Nível de urgência: {NivelUrgencia}");

        Console.WriteLine("\nItens necessários:");

        if (ItensNecessarios.Count == 0)
        {
            Console.WriteLine("A lista de itens não foi carregada.");
        }
        else
        {
            foreach (Item item in ItensNecessarios)
            {
                Console.WriteLine($"Nome: {item.Nome}");
                Console.WriteLine($"Quantidade: {item.Quantidade}");
            }
        }
    }
}
