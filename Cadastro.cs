
public class Cadastro
{
    public string? Nome { get; set; }
    public int Idade { get; set; }
    public string? Email { get; set; }
    public string? Nacionalidade { get; set; }
    public string? Telefone { get; set; }

    public Cadastro(string? nome, int idade, string? email, string? nacionalidade, string? telefone)
    {
        Nome = nome;
        Idade = idade;
        Email = email;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
    }

    public void ExibirCadastro()
    {
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Idade: {Idade}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Nacionalidade: {Nacionalidade}");
        Console.WriteLine($"Telefone: {Telefone}");
    }

    public void AtualizarCadastro(string? nome, int idade, string? email, string? nacionalidade, string? telefone)
    {
        Nome = nome;
        Idade = idade;
        Email = email;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
    }

    public void DeletarCadastro()
    {
        Nome = null;
        Idade = 0;
        Email = null;
        Nacionalidade = null;
        Telefone = null;
    }
}
