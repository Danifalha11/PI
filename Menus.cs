
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
                        Thread.Sleep(1000);
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida.");
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

        while (opcaoDoadores != 5)
        {
            Console.Clear();

            Console.WriteLine("===================================================");
            Console.WriteLine("Menu de Doadores");
            Console.WriteLine("1. Cadastrar Doador");
            Console.WriteLine("2. Listar Doadores");
            Console.WriteLine("3. Atualizar Doador");
            Console.WriteLine("4. Deletar Doador");
            Console.WriteLine("5. Voltar ao Menu Principal");
            Console.WriteLine("===================================================");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out opcaoDoadores))
            {
                switch (opcaoDoadores)
                {
                    case 1:
                        Doador? doador = Doador.CadastrarDoador();

                        if (doador != null)
                        {
                            if (doador.Salvar())
                                Console.WriteLine("Doador cadastrado com sucesso!");
                            else
                                Console.WriteLine("Não foi possível cadastrar o doador.");
                        }
                        break;

                    case 2:
                        Doador.ListarDoadores();
                        break;

                    case 3:
                        Doador.AtualizarDoador();
                        break;

                    case 4:
                        Doador.DeletarDoador();
                        break;

                    case 5:
                        Console.WriteLine("Voltando ao Menu Principal...");
                        Thread.Sleep(1000);
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida.");
            }

            if (opcaoDoadores != 5)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    public static void MenuNecessitados()
    {
        int opcaoNecessitados = 0;

        while (opcaoNecessitados != 5)
        {
            Console.Clear();

            Console.WriteLine("===================================================");
            Console.WriteLine("Menu de Necessitados");
            Console.WriteLine("1. Cadastrar Necessitado");
            Console.WriteLine("2. Listar Necessitados");
            Console.WriteLine("3. Atualizar Necessitado");
            Console.WriteLine("4. Deletar Necessitado");
            Console.WriteLine("5. Voltar ao Menu Principal");
            Console.WriteLine("===================================================");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out opcaoNecessitados))
            {
                switch (opcaoNecessitados)
                {
                    case 1:
                        Necessitado? necessitado =
                            Necessitado.CadastrarNecessitado();

                        if (necessitado != null)
                        {
                            if (necessitado.Salvar())
                                Console.WriteLine("Necessitado cadastrado com sucesso!");
                            else
                                Console.WriteLine("Não foi possível cadastrar.");
                        }
                        break;

                    case 2:
                        Necessitado.ListarNecessitados();
                        break;

                    case 3:
                        Necessitado.AtualizarNecessitado();
                        break;

                    case 4:
                        Necessitado.DeletarNecessitado();
                        break;

                    case 5:
                        Console.WriteLine("Voltando ao Menu Principal...");
                        Thread.Sleep(1000);
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida.");
            }

            if (opcaoNecessitados != 5)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    public static void MenuItens()
    {
        int opcaoItens = 0;

        while (opcaoItens != 9)
        {
            Console.Clear();

            Console.WriteLine("===================================================");
            Console.WriteLine("Menu de Itens");
            Console.WriteLine("1. Cadastrar Item");
            Console.WriteLine("2. Listar Itens");
            Console.WriteLine("3. Atualizar Item");
            Console.WriteLine("4. Deletar Item");
            Console.WriteLine("5. Listar Itens de um Doador");
            Console.WriteLine("6. Listar Itens de um Necessitado");
            Console.WriteLine("7. Buscar Quem Precisa");
            Console.WriteLine("8. Registrar Doação");
            Console.WriteLine("9. Voltar ao Menu Principal");
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
                        Item.AtualizarItem();
                        break;

                    case 4:
                        Item.DeletarItem();
                        break;

                    case 5:
                        Item.ListarItensDoDoador();
                        break;

                    case 6:
                        Item.ListarItensDoNecessitado();
                        break;

                    case 7:
                        Item.BuscarQuemPrecisa();
                        break;

                    case 8:
                        Item.RegistrarDoacao();
                        break;

                    case 9:
                        Console.WriteLine("Voltando ao Menu Principal...");
                        Thread.Sleep(1000);
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida.");
            }

            if (opcaoItens != 9)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static void CadastrarEGravarItem()
    {
        Item? item = Item.CadastrarItem();

        if (item == null)
            return;

        Console.WriteLine("\nO item pertence a:");
        Console.WriteLine("1. Doador");
        Console.WriteLine("2. Necessitado");
        Console.Write("Escolha uma opção: ");

        string? escolha = Console.ReadLine();

        if (escolha == "1")
        {
            Doador.ListarDoadores();

            Console.Write("Digite o ID do doador: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            item.DoadorId = id;
        }
        else if (escolha == "2")
        {
            Necessitado.ListarNecessitados();

            Console.Write("Digite o ID do necessitado: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            item.NecessitadoId = id;
        }
        else
        {
            Console.WriteLine("Opção inválida.");
            return;
        }

        if (item.Salvar())
            Console.WriteLine("Item cadastrado com sucesso!");
        else
            Console.WriteLine("Não foi possível cadastrar o item.");
    }
}
