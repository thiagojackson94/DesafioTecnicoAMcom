using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public const double valorTaxa = 3.5;
        private int numeroConta;
        private string nomeTitular;
        private double saldo;

        public ContaBancaria(int numeroConta, string nomeTitular, double depositoInicial = 0)
        {
            this.numeroConta = numeroConta;
            this.nomeTitular = nomeTitular;
            this.saldo = depositoInicial;
        }

        public int GetNumeroConta()
            => numeroConta;
        public string GetNomeTitular()
            => nomeTitular;
        public double GetSaldo()
            => saldo;
        public void SetNomeTitular(string nomeTitular)
            => this.nomeTitular = nomeTitular;
        public void Depositar(double valorDeposito)
            => saldo += valorDeposito;
        public void Sacar(double valorSaque)
            => saldo -= valorSaque + valorTaxa;
        public String ToString()
            => string.Format("Conta {0}, Titular: {1}, Saldo: ${2}", GetNumeroConta(), GetNomeTitular(), GetSaldo().ToString("F"));
    }
}
