using NBitcoin;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new BlockchainEnrollmentProcessor();

            x.Balance = 
            x.SendBTC();

            Console.WriteLine();
        }
    }

    public class BlockchainEnrollmentProcessor
    {
        public void SendBTC()
        {
            if (SendAmount > balance) return;
            //List<Coin> toSpend = MinimumCoinsToCoverTransaction(); // Bitcoin doesn't allow spending just the inputs you need from a previous transaction?
            List<Coin> toSpend = this.unspentCoins;

            // For the payment you will need to reference this outpoint in the transaction. You create a transaction as follows:
            var transaction = Transaction.Create(network);
            foreach (var item in toSpend)
            {
                transaction.Inputs.Add(item.Outpoint);
            }


            // To where?
            var hallOfTheMakersAddress = BitcoinAddress.Create(ToAddress, network);


            // How much?
            Money minerFee = new Money(this.Fee, MoneyUnit.BTC);
            Money amoundToSpend = new Money(this.SendAmount, MoneyUnit.BTC);

            transaction.Outputs.Add(amoundToSpend, hallOfTheMakersAddress.ScriptPubKey);
            // Send the change back
            transaction.Outputs
                .Add(new Money(toSpend.Sum(x => x.Amount.ToDecimal(MoneyUnit.BTC)) - amoundToSpend.ToDecimal(MoneyUnit.BTC) - minerFee.ToDecimal(MoneyUnit.BTC), MoneyUnit.BTC),
                this.wallets[0].ScriptPubKey);

            // message
            //var message = "Long live NBitcoin and its makers!";
            var bytes = Encoding.UTF8.GetBytes(this.Message);
            transaction.Outputs.Add(Money.Zero, TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes));

            // sign it
            // Get it from the public address
            //transaction.Inputs[0].ScriptSig = address.ScriptPubKey;
            // OR we can also use the private key 
            //transaction.Inputs[0].ScriptSig = bitcoinPrivateKey.ScriptPubKey;

            foreach (var item in transaction.Inputs)
            {
                item.ScriptSig = this.wallets[0].ScriptPubKey;
            }


            transaction.Sign(this.wallets[0].Key.GetBitcoinSecret(network), toSpend.Select(x => (ICoin)x).AsEnumerable());

            transaction // ToHex, ToBytes, ToString


            //// Propagate your transactions
            //// With QBitNinja
            //BroadcastResponse broadcastResponse = client.Broadcast(transaction).Result;

            //if (!broadcastResponse.Success)
            //{
            //    MessageBox.Show("ErrorCode: " + broadcastResponse.Error.ErrorCode);

            //    MessageBox.Show("Error message: " + broadcastResponse.Error.Reason);
            //}
            //else
            //{
            //    MessageBox.Show("Success! You can check out the hash of the transaciton in any block explorer:");
            //    MessageBox.Show(transaction.GetHash().ToString());
            //}

            //TransactionBuilderTransaction(toSpend);

        }

        private string toAddress;
        public string ToAddress { get { return toAddress; } set { toAddress = value; /*OnPropertyChanged();*/ } }//=> wallets[1].PublicKey; //"mnDJL3XEYNKCNwYoJg3RNK56ikTp9nQCVU";
        public string Message { get; set; } = "Stuur Abie al my Bitcoins.";

        //string historyDebug = string.Empty;
        //public string HistoryDebug { get { return historyDebug; } set { historyDebug = value; OnPropertyChanged(); } }

        public decimal SendAmount { get; set; } = 0.005m;
        public decimal Fee { get; private set; } = 0.00000300m;

        decimal balance = 0m;
        public decimal Balance { get { return balance; } set { balance = value;/* OnPropertyChanged();*/ } }

        Network network = null;

        //QBitNinjaClient client = null;

        List<Coin> unspentCoins = new List<Coin>();

        //BalanceModel balanceModel = null;


    }

}
