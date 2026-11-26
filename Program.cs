using System;

class Program
{
    static void Main(string[] args)
    {
        Conexao conexao = new Conexao();

        try
        {
            using (var banco = conexao.Conectar())
            {
                banco.Open();
                Console.WriteLine("Conectado ao banco com sucesso!");
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Erro ao conectar:");
        }

        Menus.MenuPrincipal();
    }
}