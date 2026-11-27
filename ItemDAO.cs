
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
    public int? DoadorId { get; set; }
    public int? NecessitadoId { get; set; }

    public bool Disponivel
    {
        get { return Quantidade > 0; }
    }

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
            Console.WriteLine("Nome inválido.");
            return null;
        }

        Console.Write("Digite a quantidade: ");

        if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade < 0)
        {
            Console.WriteLine("Quantidade inválida.");
            return null;
        }

        Console.Write("Digite o tipo (Alimento/Roupa/Material): ");
        string? tipo = Console.ReadLine();

        TipoItem tipoItem;

        if (!Enum.TryParse(tipo, true, out tipoItem))
        {
            Console.WriteLine("Tipo inválido.");
            return null;
        }

        if (!Enum.IsDefined(typeof(TipoItem), tipoItem))
        {
            Console.WriteLine("Tipo inválido.");
            return null;
        }

        return new Item(nome, quantidade, tipoItem);
    }

    public bool Salvar()
    {
        if (DoadorId == null && NecessitadoId == null)
        {
            Console.WriteLine("Informe um doador ou necessitado.");
            return false;
        }

        if (DoadorId != null && NecessitadoId != null)
        {
            Console.WriteLine("Informe apenas um responsável pelo item.");
            return false;
        }

        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                string sql = @"INSERT INTO Item
                (Nome, Quantidade, Tipo, DoadorId, NecessitadoId)
                VALUES
                (@nome, @quantidade, @tipo, @doadorId, @necessitadoId)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@nome", Nome);
                cmd.Parameters.AddWithValue("@quantidade", Quantidade);
                cmd.Parameters.AddWithValue("@tipo", (int)Tipo);
                cmd.Parameters.AddWithValue("@doadorId",
                    DoadorId.HasValue ? (object)DoadorId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@necessitadoId",
                    NecessitadoId.HasValue ? (object)NecessitadoId.Value : DBNull.Value);

                cmd.ExecuteNonQuery();

                Id = Convert.ToInt32(cmd.LastInsertedId);

                Console.WriteLine("Item cadastrado com sucesso!");
                return true;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao cadastrar item: " + ex.Message);
            return false;
        }
    }

    public void ExibirItem()
    {
        Console.WriteLine("ID: " + Id);
        Console.WriteLine("Nome: " + Nome);
        Console.WriteLine("Quantidade: " + Quantidade);
        Console.WriteLine("Tipo: " + Tipo);
        Console.WriteLine("Disponível: " + (Disponivel ? "Sim" : "Não"));
    }

    public static void ListarItens()
    {
        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                string sql = "SELECT * FROM Item";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("Nenhum item encontrado.");
                    return;
                }

                while (reader.Read())
                {
                    Item item = new Item(
                        reader["Nome"].ToString()!,
                        Convert.ToInt32(reader["Quantidade"]),
                        (TipoItem)Convert.ToInt32(reader["Tipo"])
                    );

                    item.Id = Convert.ToInt32(reader["Id"]);

                    item.ExibirItem();
                    Console.WriteLine("----------------------------");
                }

                reader.Close();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao listar itens: " + ex.Message);
        }
    }

    public static Item? BuscarItemPorId(int id)
    {
        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                string sql = "SELECT * FROM Item WHERE Id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Item item = new Item(
                        reader["Nome"].ToString()!,
                        Convert.ToInt32(reader["Quantidade"]),
                        (TipoItem)Convert.ToInt32(reader["Tipo"])
                    );

                    item.Id = Convert.ToInt32(reader["Id"]);

                    if (reader["DoadorId"] != DBNull.Value)
                        item.DoadorId = Convert.ToInt32(reader["DoadorId"]);

                    if (reader["NecessitadoId"] != DBNull.Value)
                        item.NecessitadoId = Convert.ToInt32(reader["NecessitadoId"]);

                    reader.Close();
                    return item;
                }

                reader.Close();

                Console.WriteLine("Item não encontrado.");
                return null;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao buscar item: " + ex.Message);
            return null;
        }
    }

    public static string LerCampo(string nomeCampo, string valorAtual)
    {
        Console.Write(nomeCampo + " (" + valorAtual + "): ");

        string? novoValor = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(novoValor))
            return valorAtual;

        return novoValor;
    }

    public static int LerQuantidade(int atual)
    {
        Console.Write("Quantidade (" + atual + "): ");

        string? entrada = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(entrada))
            return atual;

        int novaQuantidade;

        if (int.TryParse(entrada, out novaQuantidade) && novaQuantidade >= 0)
            return novaQuantidade;

        Console.WriteLine("Quantidade inválida.");
        return LerQuantidade(atual);
    }

    public static TipoItem LerTipoItem(TipoItem atual)
    {
        Console.Write("Tipo (" + atual + "): ");

        string? entrada = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(entrada))
            return atual;

        TipoItem novoTipo;

        if (Enum.TryParse(entrada, true, out novoTipo) &&
            Enum.IsDefined(typeof(TipoItem), novoTipo))
        {
            return novoTipo;
        }

        Console.WriteLine("Tipo inválido.");
        return LerTipoItem(atual);
    }

    public bool Atualizar()
    {
        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                string sql = @"UPDATE Item SET
                Nome = @nome,
                Quantidade = @quantidade,
                Tipo = @tipo
                WHERE Id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@nome", Nome);
                cmd.Parameters.AddWithValue("@quantidade", Quantidade);
                cmd.Parameters.AddWithValue("@tipo", (int)Tipo);
                cmd.Parameters.AddWithValue("@id", Id);

                int resultado = cmd.ExecuteNonQuery();

                if (resultado > 0)
                {
                    Console.WriteLine("Item atualizado com sucesso!");
                    return true;
                }

                Console.WriteLine("Item não encontrado.");
                return false;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao atualizar item: " + ex.Message);
            return false;
        }
    }

    public bool Deletar()
    {
        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                string sql = "DELETE FROM Item WHERE Id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", Id);

                int resultado = cmd.ExecuteNonQuery();

                if (resultado > 0)
                {
                    Console.WriteLine("Item deletado com sucesso!");
                    return true;
                }

                Console.WriteLine("Item não encontrado.");
                return false;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao deletar item: " + ex.Message);
            return false;
        }
    }

    public static Item? AtualizarItem()
    {
        Console.Write("Digite o ID do item: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return null;
        }

        Item? item = BuscarItemPorId(id);

        if (item == null)
            return null;

        item.Nome = LerCampo("Nome", item.Nome);
        item.Quantidade = LerQuantidade(item.Quantidade);
        item.Tipo = LerTipoItem(item.Tipo);

        if (item.Atualizar())
            return item;

        return null;
    }

    public static Item? DeletarItem()
    {
        Console.Write("Digite o ID do item: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return null;
        }

        Item? item = BuscarItemPorId(id);

        if (item == null)
            return null;

        item.ExibirItem();

        Console.Write("Deseja excluir? (S/N): ");
        string? resposta = Console.ReadLine();

        if (resposta?.ToUpper() != "S")
        {
            Console.WriteLine("Exclusão cancelada.");
            return null;
        }

        if (item.Deletar())
            return item;

        return null;
    }

    public static void BuscarQuemPrecisa()
    {
        Console.Write("Digite o tipo do item: ");
        string? tipo = Console.ReadLine();

        TipoItem tipoItem;

        if (!Enum.TryParse(tipo, true, out tipoItem) ||
            !Enum.IsDefined(typeof(TipoItem), tipoItem))
        {
            Console.WriteLine("Tipo inválido.");
            return;
        }

        Console.Write("Digite a cidade (ou deixe vazio): ");
        string? cidade = Console.ReadLine();

        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                string sql = @"SELECT n.Id, n.Nome, n.Cidade, n.Bairro,
                n.Telefone, n.NivelUrgencia, i.Id AS ItemId,
                i.Nome AS ItemNome, i.Quantidade
                FROM Necessitado n
                JOIN Item i ON i.NecessitadoId = n.Id
                WHERE i.Tipo = @tipo
                AND i.Quantidade > 0
                AND (@cidade IS NULL OR n.Cidade = @cidade)
                ORDER BY n.NivelUrgencia DESC";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@tipo", (int)tipoItem);

                if (string.IsNullOrWhiteSpace(cidade))
                    cmd.Parameters.AddWithValue("@cidade", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@cidade", cidade);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("Nenhum necessitado encontrado.");
                    reader.Close();
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine("Necessitado: " + reader["Nome"]);
                    Console.WriteLine("ID: " + reader["Id"]);
                    Console.WriteLine("Cidade: " + reader["Cidade"]);
                    Console.WriteLine("Bairro: " + reader["Bairro"]);
                    Console.WriteLine("Telefone: " + reader["Telefone"]);
                    Console.WriteLine("Urgência: " + reader["NivelUrgencia"]);
                    Console.WriteLine("Item: " + reader["ItemNome"]);
                    Console.WriteLine("Quantidade: " + reader["Quantidade"]);
                    Console.WriteLine("----------------------------");
                }

                reader.Close();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao buscar necessitados: " + ex.Message);
        }
    }

    public static void ListarItensPorDono(string coluna, int idDono)
    {
        if (coluna != "DoadorId" && coluna != "NecessitadoId")
        {
            Console.WriteLine("Coluna inválida.");
            return;
        }

        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                string sql = "SELECT * FROM Item WHERE " + coluna + " = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idDono);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("Nenhum item encontrado.");
                    reader.Close();
                    return;
                }

                while (reader.Read())
                {
                    Item item = new Item(
                        reader["Nome"].ToString()!,
                        Convert.ToInt32(reader["Quantidade"]),
                        (TipoItem)Convert.ToInt32(reader["Tipo"])
                    );

                    item.Id = Convert.ToInt32(reader["Id"]);

                    item.ExibirItem();
                    Console.WriteLine("----------------------------");
                }

                reader.Close();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao listar itens: " + ex.Message);
        }
    }

    public static void ListarItensDoDoador()
    {
        Doador.ListarDoadores();

        Console.Write("Digite o ID do doador: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        ListarItensPorDono("DoadorId", id);
    }

    public static void ListarItensDoNecessitado()
    {
        Necessitado.ListarNecessitados();

        Console.Write("Digite o ID do necessitado: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        ListarItensPorDono("NecessitadoId", id);
    }

    public static void RegistrarDoacao()
    {
        Console.Write("Digite o ID do item do doador: ");

        if (!int.TryParse(Console.ReadLine(), out int itemId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.Write("Digite o ID do item do necessitado: ");

        if (!int.TryParse(Console.ReadLine(), out int itemNecessitadoId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.Write("Digite a quantidade da doação: ");

        if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
        {
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        try
        {
            using (MySqlConnection conn = new Conexao().Conectar())
            {
                conn.Open();

                using (MySqlTransaction transacao = conn.BeginTransaction())
                {
                    string sql = @"UPDATE Item AS necessidade
                    JOIN Item AS doacao ON doacao.Id = @itemId
                    SET necessidade.Quantidade =
                    necessidade.Quantidade - @quantidade
                    WHERE necessidade.Id = @itemNecessitadoId
                    AND doacao.DoadorId IS NOT NULL
                    AND necessidade.NecessitadoId IS NOT NULL
                    AND doacao.Tipo = necessidade.Tipo
                    AND doacao.Quantidade >= @quantidade
                    AND necessidade.Quantidade >= @quantidade";

                    MySqlCommand cmd = new MySqlCommand(sql, conn, transacao);

                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@itemNecessitadoId", itemNecessitadoId);
                    cmd.Parameters.AddWithValue("@quantidade", quantidade);

                    int resultado = cmd.ExecuteNonQuery();

                    if (resultado == 0)
                    {
                        transacao.Rollback();

                        Console.WriteLine(
                            "Doação inválida. Confira os IDs, os tipos e as quantidades.");

                        return;
                    }

                    string sqlDoador = @"UPDATE Item
                    SET Quantidade = Quantidade - @quantidade
                    WHERE Id = @itemId
                    AND DoadorId IS NOT NULL
                    AND Quantidade >= @quantidade";

                    MySqlCommand cmdDoador =
                        new MySqlCommand(sqlDoador, conn, transacao);

                    cmdDoador.Parameters.AddWithValue("@quantidade", quantidade);
                    cmdDoador.Parameters.AddWithValue("@itemId", itemId);

                    int resultadoDoador = cmdDoador.ExecuteNonQuery();

                    if (resultadoDoador == 0)
                    {
                        transacao.Rollback();

                        Console.WriteLine("Não foi possível atualizar o item do doador.");
                        return;
                    }

                    transacao.Commit();

                    Console.WriteLine("Doação registrada com sucesso!");
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao registrar doação: " + ex.Message);
        }
    }
}
