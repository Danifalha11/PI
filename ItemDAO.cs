using MySql.Data.MySqlClient;
using System;

public enum TipoItem
{
    Alimento,
    Roupa,
    Material
}

public class Item
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Quantidade { get; set; }
    public TipoItem Tipo { get; set; }
    public bool Disponivel => Quantidade > 0;
    public int? DoadorId { get; set; }
    public int? NecessitadoId { get; set; }

    public Item(string? nome, int quantidade, TipoItem tipo)
    {
        Nome = nome;
        Quantidade = quantidade;
        Tipo = tipo;
    }

    public static Item? CadastrarItem()
    {
        Console.Write("Digite o nome do item: ");

        string? nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome do item não pode ser vazio.");
            return null;
        }

        Console.Write("Digite a quantidade do item: ");

        if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade < 0)
        {
            Console.WriteLine("Quantidade inválida. Deve ser um número inteiro não negativo.");
            return null;
        }

        Console.Write("Digite o tipo do item (Alimento/Roupa/Material): ");

        string? tipoItem = Console.ReadLine();

        if (!Enum.TryParse(tipoItem, true, out TipoItem tipoItemEnum) ||
            !Enum.IsDefined(typeof(TipoItem), tipoItemEnum))
        {
            Console.WriteLine("Tipo inválido. Deve ser 'Alimento', 'Roupa' ou 'Material'.");
            return null;
        }

        Item novoItem = new Item(nome, quantidade, tipoItemEnum);

        return novoItem;
    }

    public bool Salvar()
    {
        if ((DoadorId == null) == (NecessitadoId == null))
        {
            Console.WriteLine("Informe apenas o doador OU o necessitado.");
            return false;
        }

        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = @"INSERT INTO Item (Nome, Quantidade, Tipo, DoadorId, NecessitadoId)
                        VALUES (@nome, @quantidade, @tipo, @doadorId, @necessitadoId)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", Nome);
            cmd.Parameters.AddWithValue("@quantidade", Quantidade);
            cmd.Parameters.AddWithValue("@tipo", (int)Tipo);
            cmd.Parameters.AddWithValue("@doadorId", (object?)DoadorId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@necessitadoId", (object?)NecessitadoId ?? DBNull.Value);
            cmd.ExecuteNonQuery();

            Id = (int)cmd.LastInsertedId;
            Console.WriteLine("Item salvo com sucesso!");
            return true;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao salvar no banco: {ex.Message}");
            return false;
        }
    }
    public void ExibirItem()
    {   
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Quantidade: {Quantidade}");
        Console.WriteLine($"Tipo: {Tipo}");
        Console.WriteLine($"Disponível: {(Disponivel ? "Sim" : "Não")}");
    }

    public static void ListarItens()
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = "SELECT * FROM Item";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Nenhum item encontrado.");
                return;
            }

            while (reader.Read())
            {
                int id = reader.GetInt32("Id");
                string nome = reader.GetString("Nome");
                int quantidade = reader.GetInt32("Quantidade");
                TipoItem tipo = (TipoItem)reader.GetInt32("Tipo");

                Item item = new Item(nome, quantidade, tipo) { Id = id };
                item.ExibirItem();
                Console.WriteLine("====================================================");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao listar itens: {ex.Message}");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
