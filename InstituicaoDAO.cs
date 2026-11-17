
using System;
using System.Collections.Generic;

public class InstituicaoDoacao
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? CNPJ { get; set; }
    public string? Email { get; set; }
    public string? Cidade { get; set; }
    public string? Bairro { get; set; }
    public string? Endereco { get; set; }
    public string? Telefone { get; set; }

    public List<Item> ItensNecessarios { get; set; }

    public InstituicaoDoacao(
        string? nome,
        string? cnpj,
        string? email,
        string? cidade,
        string? bairro,
        string? endereco,
        string? telefone)
    {
        Nome = nome;
        CNPJ = cnpj;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Endereco = endereco;
        Telefone = telefone;

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

        Console.WriteLine("Item adicionado com sucesso!");
    }
    public void ExibirCadastro()
    {
        Console.WriteLine("\n--- Cadastro da Instituição ---");
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"CNPJ: {CNPJ}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Cidade: {Cidade}");
        Console.WriteLine($"Bairro: {Bairro}");
        Console.WriteLine($"Endereço: {Endereco}");
        Console.WriteLine($"Telefone: {Telefone}");

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
        string? cnpj,
        string? email,
        string? cidade,
        string? bairro,
        string? endereco,
        string? telefone)
    {
        Nome = nome;
        CNPJ = cnpj;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Endereco = endereco;
        Telefone = telefone;

        Console.WriteLine("Cadastro atualizado com sucesso!");
    }
    public void DeletarCadastro()
    {
        Nome = null;
        CNPJ = null;
        Email = null;
        Cidade = null;
        Bairro = null;
        Endereco = null;
        Telefone = null;

        ItensNecessarios.Clear();

        Console.WriteLine("Cadastro excluído com sucesso!");
    }
}
