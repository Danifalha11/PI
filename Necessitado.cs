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

    public Necessitado(string? nome, string? email, string? cidade, string? bairro, string? nacionalidade, string? telefone, NivelUrgencia nivelUrgencia)
    {
        Nome = nome;
        Email = email;
        Cidade = cidade;
        Bairro = bairro;
        Nacionalidade = nacionalidade;
        Telefone = telefone;
        NivelUrgencia = nivelUrgencia;
        ItensNecessarios = [];
    }
}
