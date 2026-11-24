using System;
using System.Threading;

static void MenuPrincipal()
{
    int opcaoPrincipal = 0;

    while (opcaoPrincipal != 4)
    {
        Console.Clear();
        Console.WriteLine("===================================================");
        Console.WriteLine("Menu Principal");
        Console.WriteLine("1. Menu de Doadores");
        Console.WriteLine("2. Menu de Necessitados");
        Console.WriteLine("3. Menu de Itens");
        Console.WriteLine("4. Sair");
        Console.WriteLine("===================================================");
        Console.Write("Escolha uma opção: ");

        if (int.TryParse(Console.ReadLine(), out opcaoPrincipal))
        {
            switch (opcaoPrincipal)
            {
                case 1:
                    MenuDoadores();
                    break;
                case 2:
                    MenuNecessitados();
                    break;
                case 3:
                    // Lógica para a Opção 3
                    break;
                case 4:
                    Console.WriteLine("Saindo do programa...");
                    Thread.Sleep(3000);
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Entrada inválida. Selecione uma das opções acima.");
        }

        if (opcaoPrincipal != 4)
        {
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}

static void MenuDoadores()
{
    
}

static void MenuNecessitados()
{
    
}