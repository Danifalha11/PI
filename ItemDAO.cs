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
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public TipoItem Tipo { get; set; }
    public bool Disponivel => Quantidade > 0;
    public int? DoadorId { get; set; }
    public int? NecessitadoId { get; set; }

    public Item(string nome, int quantidade, TipoItem tipo)
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
            Console.WriteLine("O nome não pode ser vazio.");
            return null;
        }

        Console.Write("Digite a quantidade do item: ");

        if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade < 0)
        {
            Console.WriteLine("Quantidade inválida, deve ser um número inteiro não negativo.");
            return null;
        }

        Console.Write("Digite o tipo do item (Alimento/Roupa/Material): ");
        string? tipoItem = Console.ReadLine();

        if (!Enum.TryParse(tipoItem, true, out TipoItem tipoItemEnum) ||
            !Enum.IsDefined(typeof(TipoItem), tipoItemEnum))
        {
            Console.WriteLine("Tipo inválido.");
            return null;
        }

        return new Item(nome, quantidade, tipoItemEnum);
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

            string sql = @"INSERT INTO Item
                (Nome, Quantidade, Tipo, DoadorId, NecessitadoId)
                VALUES
                (@nome, @quantidade, @tipo, @doadorId, @necessitadoId)";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", Nome);
            cmd.Parameters.AddWithValue("@quantidade", Quantidade);
            cmd.Parameters.AddWithValue("@tipo", (int)Tipo);
            cmd.Parameters.AddWithValue("@doadorId", (object?)DoadorId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@necessitadoId", (object?)NecessitadoId ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            Id = Convert.ToInt32(cmd.LastInsertedId);

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
        Console.WriteLine($"ID: {Id} || Nome: {Nome} || Quantidade: {Quantidade} || Tipo: {Tipo} || Disponível: {(Disponivel ? "Sim" : "Não")}");
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
                int id = reader.GetInt32(reader.GetOrdinal("Id"));
                string nome = reader["Nome"] as string ?? string.Empty;
                int quantidade = reader.GetInt32(reader.GetOrdinal("Quantidade"));
                TipoItem tipo = (TipoItem)reader.GetInt32(reader.GetOrdinal("Tipo"));

                Item item = new Item(nome, quantidade, tipo)
                {
                    Id = id
                };

                item.ExibirItem();
                Console.WriteLine("====================================================");
            }

            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao listar itens: {ex.Message}");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    public static Item? BuscarItemPorId(int id)
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = "SELECT * FROM Item WHERE Id = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string nome = reader["Nome"] as string ?? string.Empty;
                int quantidade = reader.GetInt32(reader.GetOrdinal("Quantidade"));
                TipoItem tipo = (TipoItem)reader.GetInt32(reader.GetOrdinal("Tipo"));

                Item item = new Item(nome, quantidade, tipo)
                {
                    Id = id,
                    DoadorId = reader["DoadorId"] == DBNull.Value ? null : Convert.ToInt32(reader["DoadorId"]),
                    NecessitadoId = reader["NecessitadoId"] == DBNull.Value ? null : Convert.ToInt32(reader["NecessitadoId"])
                };

                return item;
            }

            Console.WriteLine($"Item com ID {id} não encontrado.");
            return null;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao buscar item: {ex.Message}");
            return null;
        }
    }

    public static string LerCampo(string nomeCampo, string? valorAtual)
    {
        Console.Write($"{nomeCampo} ({valorAtual}): ");

        string? novoValor = Console.ReadLine();

        return string.IsNullOrWhiteSpace(novoValor)
            ? valorAtual ?? string.Empty
            : novoValor;
    }

    public static int LerQuantidade(int atual)
    {
        Console.Write($"Quantidade ({atual}): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return atual;

        if (int.TryParse(input, out int novaQuantidade) && novaQuantidade >= 0)
            return novaQuantidade;

        Console.WriteLine("Quantidade inválida. Deve ser um número inteiro não negativo.");
        return LerQuantidade(atual);
    }

    public static TipoItem LerTipoItem(TipoItem atual)
    {
        Console.Write($"Tipo do item ({atual}): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return atual;

        if (Enum.TryParse(input, true, out TipoItem novoTipo) &&
            Enum.IsDefined(typeof(TipoItem), novoTipo))
        {
            return novoTipo;
        }

        Console.WriteLine("Tipo inválido. Deve ser Alimento, Roupa ou Material.");
        return LerTipoItem(atual);
    }

    public bool Atualizar()
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = @"UPDATE Item SET
                Nome = @nome,
                Quantidade = @quantidade,
                Tipo = @tipo
                WHERE Id = @id";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", Nome);
            cmd.Parameters.AddWithValue("@quantidade", Quantidade);
            cmd.Parameters.AddWithValue("@tipo", (int)Tipo);
            cmd.Parameters.AddWithValue("@id", Id);

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas == 0)
            {
                Console.WriteLine("Nenhum item encontrado para atualizar.");
                return false;
            }

            Console.WriteLine("Item atualizado com sucesso!");
            return true;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao atualizar item: {ex.Message}");
            return false;
        }
    }

    public bool Deletar()
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = "DELETE FROM Item WHERE Id = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", Id);

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas == 0)
            {
                Console.WriteLine("Nenhum item encontrado para deletar.");
                return false;
            }

            Console.WriteLine("Item deletado com sucesso!");
            return true;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao deletar item: {ex.Message}");
            return false;
        }
    }

    public static Item? AtualizarItem()
    {
        Console.Write("Digite o ID do item que deseja atualizar: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return null;
        }

        Item? item = BuscarItemPorId(id);

        if (item == null)
            return null;

        Console.WriteLine("Digite os novos valores para os campos (deixe em branco para manter o valor atual):");

        item.Nome = LerCampo("Nome", item.Nome);
        item.Quantidade = LerQuantidade(item.Quantidade);
        item.Tipo = LerTipoItem(item.Tipo);

        if (!item.Atualizar())
            return null;

        return item;
    }

    public static Item? DeletarItem()
    {
        Console.Write("Digite o ID do item que deseja deletar: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return null;
        }

        Item? item = BuscarItemPorId(id);

        if (item == null)
            return null;

        item.ExibirItem();

        Console.Write("Tem certeza que deseja excluir este item? (S/N): ");
        string? resposta = Console.ReadLine();

        if (resposta?.Trim().ToUpper() != "S")
        {
            Console.WriteLine("Exclusão cancelada.");
            return null;
        }

        if (!item.Deletar())
            return null;

        return item;
    }
}
