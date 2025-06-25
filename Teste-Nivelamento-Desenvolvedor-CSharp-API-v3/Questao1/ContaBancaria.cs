using System.Globalization;

namespace Questao1;

public class ContaBancaria 
{
    public int Numero { get; }
    
    private string _titular;
    public string Titular
    {
        get => _titular;
        set => _titular = value;
    }

    public double Saldo { get; private set; }

    private const double TaxaSaque = 3.50;

    public ContaBancaria(int numero, string titular)
    {
        Numero = numero;
        Titular = titular;
    }

    public ContaBancaria(int numero, string titular, double depositoInicial) : this(numero, titular)
    {
        Deposito(depositoInicial);
    }

    public void Deposito(double valor)
    {
        Saldo += valor;
    }

    public void Saque(double valor)
    {
        Saldo -= valor + TaxaSaque;
    }

    public override string ToString()
    {
        return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
    }
}
