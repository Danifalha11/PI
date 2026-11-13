public enum TipoItem
{
    Alimento,
    Roupa,
    Material
}
public class Item
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Quantidade { get; set; }
    public TipoItem Tipo { get; set; }

    public Item(string? nome, int quantidade, TipoItem tipo)
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
        Console.Write("Digite o tipo do item (Alimento/Roupa/Material): ");
        string? tipoItem = Console.ReadLine();
        if (!Enum.TryParse(tipoItem, true, out TipoItem tipoItemEnum) || !Enum.IsDefined(typeof(TipoItem), tipoItemEnum))
        {
            Console.WriteLine("Tipo inválido. Deve ser 'Alimento', 'Roupa' ou 'Material'.");
            return null;
        }

        Item novoItem = new Item(nome, quantidade, tipoItemEnum);
        Console.WriteLine("Item cadastrado com sucesso!");
        return novoItem;
    }
}
