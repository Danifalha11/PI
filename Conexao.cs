using MySql.Data.MySqlClient;

public class Conexao
{
    private readonly string connectionString =
        "Server=localhost;Database=SistemaDoacoes;Uid=root;Pwd=Senac2026;";

    public MySqlConnection Conectar()
    {
        return new MySqlConnection(connectionString);
    }
}
