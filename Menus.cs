using System;
using System.Threading;

public class Menus
{
    public static void MenuPrincipal()
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
                        MenuItens();
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

    public static void MenuDoadores()
    {
        int opcaoDoadores = 0;
        while (opcaoDoadores != 3)
        {
            Console.Clear();
            Console.WriteLine("===================================================");
            Console.WriteLine("Menu de Doadores");
            Console.WriteLine("1. Cadastrar Doador");
            Console.WriteLine("2. Listar Doadores");
            Console.WriteLine("3. Voltar ao Menu Principal");
            Console.WriteLine("===================================================");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out opcaoDoadores))
            {
                switch (opcaoDoadores)
                {
                    case 1:
                        Doador? doador = Doador.CadastrarDoador();
                        doador?.Salvar();
                        break;
                    case 2:
                        // Fazer depois
                        break;
                    case 3:
                        Console.WriteLine("Voltando ao Menu Principal...");
                        Thread.Sleep(2000);
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

            if (opcaoDoadores != 3)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    public static void MenuNecessitados()
    {
        
    }

    public static void MenuItens()
    {
        int opcaoItens = 0;
        while (opcaoItens != 3)
        {
            Console.Clear();
            Console.WriteLine("===================================================");
            Console.WriteLine("Menu de Itens");
            Console.WriteLine("1. Cadastrar Item");
            Console.WriteLine("2. Listar Itens");
            Console.WriteLine("3. Voltar ao Menu Principal");
            Console.WriteLine("===================================================");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out opcaoItens))
            {
                switch (opcaoItens)
                {
                    case 1:
                        CadastrarEGravarItem();
                        break;
                    case 2:
                        Item.ListarItens();
                        break;
                    case 3:
                        Console.WriteLine("Voltando ao Menu Principal...");
                        Thread.Sleep(2000);
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

            if (opcaoItens != 3)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static void CadastrarEGravarItem()
    {
        Item? item = Item.CadastrarItem();
        if (item == null) return;

        Console.WriteLine("O item pertence a:");
        Console.WriteLine("1. Doador");
        Console.WriteLine("2. Necessitado");
        Console.Write("Escolha uma opção: ");
        string? escolha = Console.ReadLine();

        Console.Write("Digite o Id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Id inválido.");
            return;
        }

        if (escolha == "1")
            item.DoadorId = id;
        else if (escolha == "2")
            item.NecessitadoId = id;
        else
        {
            Console.WriteLine("Opção inválida.");
            return;
        }

        item.Salvar();
    }
}
