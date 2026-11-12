public class Item
{
    public string? Nome { get; set; }
    public int Quantidade { get; set; }
    public string? Tipo { get; set; }

    public Item(string? nome, int quantidade, string? tipo)
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
            Console.WriteLine("Nome do item não pode ser vazio.");
            return null;
        }
        Console.Write("Digite a quantidade do item: ");
        if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade < 0)
        {
            Console.WriteLine("Quantidade inválida. Deve ser um número inteiro não negativo.");
            return null;
        }
        Console.Write("Digite o tipo do item: ");
        string? tipo = Console.ReadLine();
        if (tipo != "Alimento" && tipo != "Roupa" && tipo != "Material")
        {
            Console.WriteLine("Tipo inválido. Deve ser 'Alimento', 'Roupa' ou 'Material'.");
            return null;
        }

        Item novoItem = new Item(nome, quantidade, tipo);
        Console.WriteLine("Item cadastrado com sucesso!");
        return novoItem;
    }
}