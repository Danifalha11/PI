
using Microsoft.Data.SqlClient;

public class Conexao
{
    private readonly string connectionString =
        "Server=localhost;Database=SistemaDoacoes;Trusted_Connection=True;TrustServerCertificate=True;";

    public SqlConnection Conectar()
    {
        return new SqlConnection(connectionString);
    }
}
