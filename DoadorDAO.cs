
using System;
using System.Collections.Generic;
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

    public List<Item> Itens { get; set; }

    public Doador(
        string? nome,
        string? email,
        string? cidade,
        string? bairro,
        string? nacionalidade,
        string? telefone)
    {
        Nome = nome;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Nacionalidade = nacionalidade;
        Telefone = telefone;

        Itens = new List<Item>();
    }

    public static Doador? CadastrarDoador()
    {
        Console.Write("Digite o nome do doador: ");
        string? nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome do doador não pode ser vazio.");
            return null;
        }

        Console.Write("Digite o email do doador: ");
        string? email = Console.ReadLine();

        Console.Write("Digite a cidade do doador: ");
        string? cidade = Console.ReadLine();

        Console.Write("Digite o bairro do doador: ");
        string? bairro = Console.ReadLine();

        Console.Write("Digite a nacionalidade do doador: ");
        string? nacionalidade = Console.ReadLine();

        Console.Write("Digite o telefone do doador: ");
        string? telefone = Console.ReadLine();

        Doador novoDoador = new Doador(nome, email, cidade, bairro, nacionalidade, telefone);

        return novoDoador;
    }

    public bool Salvar()
    {
        try
        {
            using (var conexao = new Conexao().Conectar())
            {
                conexao.Open();
                string query = "INSERT INTO Doador (nome, email, cidade, bairro, nacionalidade, telefone) VALUES (@Nome, @Email, @Cidade, @Bairro, @Nacionalidade, @Telefone)";
                using (var comando = new MySqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", Nome);
                    comando.Parameters.AddWithValue("@Email", Email);
                    comando.Parameters.AddWithValue("@Cidade", Cidade);
                    comando.Parameters.AddWithValue("@Bairro", Bairro);
                    comando.Parameters.AddWithValue("@Nacionalidade", Nacionalidade);
                    comando.Parameters.AddWithValue("@Telefone", Telefone);
                    comando.ExecuteNonQuery();

                    Id = (int)comando.LastInsertedId;
                    Console.WriteLine($"Doador salvo com sucesso! ID do doador: {Id}");
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar o doador: {ex.Message}");
            return false;
        }
    }

    public void ExibirDoadores()
    {
        Console.WriteLine($"ID: {Id} || Nome: {Nome} || Email: {Email} || Cidade: {Cidade} || Bairro: {Bairro} || Nacionalidade: {Nacionalidade} || Telefone: {Telefone}");
    }

    public static void ListarDoadores()
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = "SELECT * FROM Doador";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Nenhum doador encontrado.");
                return;
            }

            while (reader.Read())
            {
                int id = reader.GetInt32("Id");
                string? nome = reader["Nome"] as string;
                string? email = reader["Email"] as string;
                string? cidade = reader["Cidade"] as string;
                string? bairro = reader["Bairro"] as string;
                string? nacionalidade = reader["Nacionalidade"] as string;
                string? telefone = reader["Telefone"] as string;

                Doador doador = new Doador(nome, email, cidade, bairro, nacionalidade, telefone) { Id = id };
                doador.ExibirDoadores();
                Console.WriteLine("====================================================");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao listar doadores: {ex.Message}");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
    
    public void AtualizarCadastro(
        string? nome,
        string? email,
        string? nacionalidade,
        string? telefone)
    {
        Nome = nome;
        Email = email;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
    }

    public void DeletarCadastro()
    {
        Nome = null;
        Email = null;
        Cidade = null;
        Bairro = null;
        Nacionalidade = null;
        Telefone = null;
    }
}
