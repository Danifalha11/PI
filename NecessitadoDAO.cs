
using System;
using System.Collections.Generic;

public enum NivelUrgencia
{
    Baixa,
    Media,
    Alta
}

public class Necessitado
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Cidade { get; set; }
    public string? Bairro { get; set; }
    public string? Nacionalidade { get; set; }
    public string? Telefone { get; set; }

    public NivelUrgencia NivelUrgencia { get; set; }

    public List<Item> ItensNecessarios { get; set; }

    public Necessitado(
        string? nome,
        string? email,
        string? cidade,
        string? bairro,
        string? nacionalidade,
        string? telefone,
        NivelUrgencia nivelUrgencia)
    {
        Nome = nome;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
        NivelUrgencia = nivelUrgencia;

        ItensNecessarios = new List<Item>();
    }
    public void AdicionarItemNecessario(Item item)
    {
        if (item == null)
        {
            Console.WriteLine("Item inválido.");
            return;
        }

        ItensNecessarios.Add(item);

        Console.WriteLine("Item adicionado à lista de necessidades!");
    }
    public void ExibirCadastro()
    {
        Console.WriteLine("\n--- Cadastro do Necessitado ---");
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Cidade: {Cidade}");
        Console.WriteLine($"Bairro: {Bairro}");
        Console.WriteLine($"Nacionalidade: {Nacionalidade}");
        Console.WriteLine($"Telefone: {Telefone}");
        Console.WriteLine($"Nível de urgência: {NivelUrgencia}");

        Console.WriteLine("\nItens necessários:");

        if (ItensNecessarios.Count == 0)
        {
            Console.WriteLine("Nenhum item cadastrado.");
        }
        else
        {
            foreach (Item item in ItensNecessarios)
            {
                item.ExibirItem();
            }
        }
    }
    public void AtualizarCadastro(
        string? nome,
        string? email,
        string? cidade,
        string? bairro,
        string? nacionalidade,
        string? telefone,
        NivelUrgencia nivelUrgencia)
    {
        Nome = nome;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
        NivelUrgencia = nivelUrgencia;

        Console.WriteLine("Cadastro atualizado com sucesso!");
    }
    public void DeletarCadastro()
    {
        Nome = null;
        Email = null;
        Cidade = null;
        Bairro = null;
        Nacionalidade = null;
        Telefone = null;

        ItensNecessarios.Clear();

        Console.WriteLine("Cadastro excluído!");
    }
}
