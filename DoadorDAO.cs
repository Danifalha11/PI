
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Cms;

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
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao listar doadores: {ex.Message}");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
    
    public static Doador? BuscarDoadorPorId(int id)
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string query = "SELECT * FROM Doador WHERE Id = @Id";
            using var comando = new MySqlCommand(query, conn);
            comando.Parameters.AddWithValue("@Id", id);

            using var reader = comando.ExecuteReader();
            if (reader.Read())
            {
                string? nome = reader["Nome"] as string;
                string? email = reader["Email"] as string;
                string? cidade = reader["Cidade"] as string;
                string? bairro = reader["Bairro"] as string;
                string? nacionalidade = reader["Nacionalidade"] as string;
                string? telefone = reader["Telefone"] as string;

                return new Doador(nome, email, cidade, bairro, nacionalidade, telefone) { Id = id };
            }
            else
            {
                Console.WriteLine($"Nenhum doador encontrado com o ID {id}.");
                return null;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao buscar doador: {ex.Message}");
            return null;
        }
    }

    public static string? LerCampo(string rotulo, string? valorAtual)
    {
        Console.Write($"{rotulo} (atual: {valorAtual ?? "vazio"}): ");
        string? novoValor = Console.ReadLine();
        return string.IsNullOrWhiteSpace(novoValor) ? valorAtual : novoValor;
    }

    public bool Atualizar()
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string query = "UPDATE Doador SET Nome = @Nome, Email = @Email, Cidade = @Cidade, Bairro = @Bairro, Nacionalidade = @Nacionalidade, Telefone = @Telefone WHERE Id = @Id";

            using var comando = new MySqlCommand(query, conn);
            comando.Parameters.AddWithValue("@Nome", Nome);
            comando.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Cidade", (object?)Cidade ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Bairro", (object?)Bairro ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Nacionalidade", (object?)Nacionalidade ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Telefone", (object?)Telefone ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Id", Id);

            comando.ExecuteNonQuery();

            Console.WriteLine("Doador atualizado com sucesso!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar o doador: {ex.Message}");
            return false;
        }
    }

    public bool Deletar()
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string query = "DELETE FROM Doador WHERE Id = @Id";
            using var comando = new MySqlCommand(query, conn);
            comando.Parameters.AddWithValue("@Id", Id);

            int linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas > 0)
            {
                Console.WriteLine("Doador deletado com sucesso!");
                return true;
            }
            else
            {
                Console.WriteLine("Nenhum doador encontrado para deletar.");
                return false;
            }
        }
        catch (MySqlException ex) when (ex.Number == 1451)
        {
            Console.WriteLine($"Erro ao deletar o doador: este doador ainda tem itens cadastrados. Por favor, remova os itens antes de deletar o doador.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao deletar o doador: {ex.Message}");
            return false;
        }
    }

    public static Doador? AtualizarDoador()
    {
        Console.Write("Digite o ID do doador que deseja atualizar: ");
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

        Console.WriteLine("Digite os novos valores para os campos (deixe em branco para manter o valor atual):");
        doador.Nome = LerCampo("Nome", doador.Nome);
        doador.Email = LerCampo("Email", doador.Email);
        doador.Cidade = LerCampo("Cidade", doador.Cidade);
        doador.Bairro = LerCampo("Bairro", doador.Bairro);
        doador.Nacionalidade = LerCampo("Nacionalidade", doador.Nacionalidade);
        doador.Telefone = LerCampo("Telefone", doador.Telefone);

        doador.Atualizar();
        return doador;
    }

    public static Doador? DeletarDoador()
    {
        Console.Write("Digite o ID do doador que deseja deletar: ");
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
        Console.Write("Tem certeza que deseja excluir? (S/N): ");
        string? resposta = Console.ReadLine();

        if (resposta?.Trim().ToUpper() != "S")
        {
            Console.WriteLine("Exclusão cancelada.");
            return null;
        }

        doador.Deletar();
        return doador;
    }
}
