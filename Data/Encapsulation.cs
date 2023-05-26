using System;

namespace Snippets;

public class Data
{
  // Encapsulation Example with Sample Data
  public class BankAccount
  {
      private decimal _balance;

      public decimal Balance
      {
          get { return _balance; }
          private set
          {
              if (value < 0)
                  throw new ArgumentException("Balance cannot be negative.");
              _balance = value;
          }
      }

      public BankAccount(decimal initialBalance)
      {
          Balance = initialBalance;
      }

      public void Deposit(decimal amount)
      {
          if (amount <= 0)
              throw new ArgumentException("Deposit amount must be positive.");
          Balance += amount;
      }

      public void Withdraw(decimal amount)
      {
          if (amount <= 0)
              throw new ArgumentException("Withdraw amount must be positive.");
          if (Balance - amount < 0)
              throw new ArgumentException("Insufficient funds.");
          Balance -= amount;
      }
  }
}
