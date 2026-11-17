
using System;

public class SiteDoacao : Doador
{
    public int Idade { get; set; }

    public SiteDoacao(
        string? nome,
        int idade,
        string? email,
        string? nacionalidade,
        string? telefone)
        : base(nome, email, null, null, nacionalidade, telefone)
    {
        Idade = idade;
    }

    public void Doacao()
    {
        Console.WriteLine($"{Nome} está realizando uma doação.");
    }
}
