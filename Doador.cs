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

    public Doador(string? nome, string? email, string? cidade, string? bairro, string? nacionalidade, string? telefone)
    {
        Nome = nome;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
        Itens = [];
    }

    public void ExibirCadastro()
    {
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Nacionalidade: {Nacionalidade}");
        Console.WriteLine($"Telefone: {Telefone}");
    }

    public void AtualizarCadastro(string? nome, string? email, string? nacionalidade, string? telefone)
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
        Nacionalidade = null;
        Telefone = null;
    }
}
