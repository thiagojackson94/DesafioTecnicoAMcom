using System;
using System.Globalization;

namespace Questao1
{
    class Program
    {
        static void Main(string[] args)
        {

            ContaBancaria conta;

            int numero = obterNumeroConta();

            Console.Write("Entre com o titular da conta: ");
            string titular = Console.ReadLine();

            char desejaDeposioInicial = validarDepositoInicial();
            bool ehDepositoInicial = desejaDeposioInicial.Equals('s');
            if (ehDepositoInicial)
            {
                double depositoInicial = obterValorDeposito();
                conta = new ContaBancaria(numero, titular, depositoInicial);
            }
            else
                conta = new ContaBancaria(numero, titular);

            imprimirDadosConta(conta);
            double quantiaDeposito = obterValorDeposito(ehDepositoInicial);
            conta.Depositar(quantiaDeposito);
            imprimirDadosConta(conta);
            double quantiaSaque = obterValorSaque();
            conta.Sacar(quantiaSaque);
            imprimirDadosConta(conta);

            char desejaAlterarNomeTitular = validarAlteracaoNomeTitular();
            if (desejaAlterarNomeTitular.Equals('s'))
            {
                Console.WriteLine("Digite o novo nome do titular: ");
                conta.SetNomeTitular(Console.ReadLine());
                Console.WriteLine("Titular atualizado com sucesso.");
                imprimirDadosConta(conta);
            }

            Console.WriteLine("Movimentações da conta realizada com sucesso.");

            /* Output expected:
            Exemplo 1:

            Entre o número da conta: 5447
            Entre o titular da conta: Milton Gonçalves
            Haverá depósito inicial(s / n) ? s
            Entre o valor de depósito inicial: 350.00

            Dados da conta:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

            Entre um valor para depósito: 200
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

            Entre um valor para saque: 199
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

            Exemplo 2:
            Entre o número da conta: 5139
            Entre o titular da conta: Elza Soares
            Haverá depósito inicial(s / n) ? n

            Dados da conta:
            Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

            Entre um valor para depósito: 300.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

            Entre um valor para saque: 298.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
            */
        }

        private static int obterNumeroConta()
        {
            Console.Write("Entre o número da conta: ");
            int numero;
            if (int.TryParse(Console.ReadLine(), out numero))
                return numero;
            else
            {
                Console.Clear();
                Console.WriteLine("Você deve digitar apenas números.");
                return obterNumeroConta();
            }
        }

        private static double obterValorDeposito(bool ehDepositoInicial = false)
        {
            Console.Write("Entre com um valor de depósito {0}: ", ehDepositoInicial ? "novamente" : "inicial");
            double depositoInicial;
            if (double.TryParse(Console.ReadLine(), out depositoInicial))
                return depositoInicial;
            else
            {
                Console.Clear();
                Console.WriteLine("Valor digitado deve ser um número válido.");
                return obterValorDeposito();
            }
        }

        private static double obterValorSaque()
        {
            Console.Write("Entre com um valor para saque: ");
            double valorSaque;
            if (double.TryParse(Console.ReadLine(), out valorSaque))
                return valorSaque;
            else
            {
                Console.Clear();
                Console.WriteLine("Valor digitado deve ser um número válido.");
                return obterValorSaque();
            }
        }

        private static char validarDepositoInicial()
        {
            Console.Write("Haverá depósito inicial (s/n)? ");
            char resp;
            if (char.TryParse(Console.ReadLine(), out resp) &&
                (resp.ToLower(CultureInfo.InvariantCulture).Equals('s') ||
                 resp.ToLower(CultureInfo.InvariantCulture).Equals('n')))
                return resp.ToLower(CultureInfo.InvariantCulture);
            else
            {
                Console.Clear();
                Console.WriteLine("Valor digitado não corresponde com o esperado.");
                return validarDepositoInicial();
            }
        }

        private static char validarAlteracaoNomeTitular()
        {
            Console.Write("Deseja alterar o nome do titular da conta (s/n)? ");
            char resp;
            if (char.TryParse(Console.ReadLine(), out resp) &&
                (resp.ToLower(CultureInfo.InvariantCulture).Equals('s') ||
                 resp.ToLower(CultureInfo.InvariantCulture).Equals('n')))
                return resp.ToLower(CultureInfo.InvariantCulture);
            else
            {
                Console.Clear();
                Console.WriteLine("Valor digitado não corresponde com o esperado.");
                return validarDepositoInicial();
            }
        }

        private static void imprimirDadosConta(ContaBancaria conta)
        {
            Console.WriteLine();
            Console.WriteLine("Dados atuais da conta:");
            Console.WriteLine(conta.ToString());
            Console.WriteLine();
        }
    }
}
