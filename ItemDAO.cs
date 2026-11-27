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

    public static void BuscarQuemPrecisa()
    {
        Console.Write("Digite o tipo de item que deseja buscar (Alimento/Roupa/Material): ");
        string? tipoItem = Console.ReadLine();

        if (!Enum.TryParse(tipoItem, true, out TipoItem tipoItemEnum) ||
            !Enum.IsDefined(typeof(TipoItem), tipoItemEnum))
        {
            Console.WriteLine("Tipo inválido.");
            return;
        }

        Console.Write("Cidade do necessitado (deixe em branco para não filtrar): ");
        string? cidade = Console.ReadLine();

        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = @"SELECT n.Id, n.Nome, n.Cidade, n.Bairro, n.Telefone, n.NivelUrgencia,
                              i.Id AS ItemId, i.Nome AS ItemNome, i.Quantidade
                            FROM Necessitado n
                            JOIN Item i ON i.NecessitadoId = n.Id
                            WHERE i.Tipo = @tipo
                                AND i.Quantidade > 0
                                AND (@cidade IS NULL OR n.Cidade = @cidade)
                            ORDER BY n.NivelUrgencia DESC";
                        
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@tipo", (int)tipoItemEnum);
            cmd.Parameters.AddWithValue("@cidade", string.IsNullOrWhiteSpace(cidade) ? DBNull.Value : cidade.Trim());

            using var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Nenhum necessitado encontrado para o tipo de item especificado.");
                return;
            }

            while (reader.Read())
            {
                string urgencia = reader.GetInt32("NivelUrgencia") switch
                {
                    0 => "Baixa",
                    1 => "Média",
                    2 => "Alta",
                    _ => "Não informada"
                };

                Console.WriteLine($"Necessitado: {reader["Nome"]} (Id {reader.GetInt32("Id")})");
                Console.WriteLine($"Local: {reader["Bairro"]}, {reader["Cidade"]}");
                Console.WriteLine($"Telefone: {reader["Telefone"]}");
                Console.WriteLine($"Urgência: {urgencia}");
                Console.WriteLine($"Precisa de: {reader["ItemNome"]} - {reader.GetInt32("Quantidade")} unidade(s) (Id do item: {reader.GetInt32("ItemId")})");
                Console.WriteLine("====================================================");
            
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao buscar necessitados: {ex.Message}");
        }
    }

    public static void ListarItensPorDono(string coluna, int idDono)
    {
        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();

            string sql = $"SELECT * FROM Item WHERE {coluna} = @idDono";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@idDono", idDono);

            using var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("Nenhum item encontrado para este dono.");
                return;
            }

            while (reader.Read())
            {
                Item item = new Item(
                (reader["Nome"] as string) ?? string.Empty,   
                reader.GetInt32("Quantidade"),
                (TipoItem)reader.GetInt32("Tipo")) { Id = reader.GetInt32("Id") };

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

    public static void ListarItensDoDoador(int doadorId)
    {
        Console.WriteLine("--- Doadores Cadastrados ---");
        Doador.ListarDoadores();

        Console.Write("\nDigite o ID do doador para listar seus itens: ");
        if (!int.TryParse(Console.ReadLine(), out int idDoador))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        ListarItensPorDono("DoadorId", doadorId);
    }

        public static void ListarItensDoNecessitado(int necessitadoId)
    {
        Console.WriteLine("--- Necessitados Cadastrados ---");
        Necessitado.ListarNecessitados();

        Console.Write("\nDigite o ID do necessitado para listar seus itens: ");
        if (!int.TryParse(Console.ReadLine(), out int idNecessitado))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        ListarItensPorDono("DoadorId", necessitadoId);
    }

    public static void RegistrarDoacao()
    {
        Console.Write("Digite o ID do item que deseja doar: ");
        if (!int.TryParse(Console.ReadLine(), out int itemId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.Write("Digite o ID do item que o necessitado precisa: ");
        if (!int.TryParse(Console.ReadLine(), out int itemNecessitadoId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.Write("Digite a quantidade que deseja doar: ");
        if (!int.TryParse(Console.ReadLine(), out int quantidadeDoada) || quantidadeDoada <= 0)
        {
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        try
        {
            using var conn = new Conexao().Conectar();
            conn.Open();
            using var transacao = conn.BeginTransaction();

            string sqlDoador = @"UPDATE Item
                                SET Quantidade = Quantidade - @quantidadeDoada
                                WHERE Id = @itemId
                                AND DoadorId IS NOT NULL
                                AND Quantidade >= @quantidadeDoada";

            using (var cmd = new MySqlCommand(sqlDoador, conn, transacao))
            {
                cmd.Parameters.AddWithValue("@quantidadeDoada", quantidadeDoada);
                cmd.Parameters.AddWithValue("@itemId", itemId);

                if (cmd.ExecuteNonQuery() == 0)
                {
                    transacao.Rollback();
                    Console.WriteLine("Não foi possível registrar a doação. Verifique se o item é de um doador e se há quantidade suficiente.");
                    return;
                }
            }

            string sqlNecessitado = @"UPDATE Item
                                    SET Quantidade = GREATEST(Quantidade - @quantidadeDoada, 0)
                                    WHERE Id = @itemNecessitadoId
                                        AND NecessitadoId IS NOT NULL";

            using (var cmd = new MySqlCommand(sqlNecessitado, conn, transacao))
            {
                cmd.Parameters.AddWithValue("@quantidadeDoada", quantidadeDoada);
                cmd.Parameters.AddWithValue("@itemNecessitadoId", itemNecessitadoId);

                if (cmd.ExecuteNonQuery() == 0)
                {
                    transacao.Rollback();
                    Console.WriteLine("Não foi possível registrar a doação. Verifique se o item informado é de um necessitado.");
                    return;
                }
            }

            transacao.Commit();
            Console.WriteLine("Doação registrada com sucesso!");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Erro ao registrar doação: {ex.Message}");
        }
    }
}
