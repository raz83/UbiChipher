using NBitcoin;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nbitcoin_temp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testw = new TestWallet();
            var tx = testw.GetTransactionData("16c61c5679ffa9bfbdd54d7f4c99c978321e69b919275f80fd882490328c434c");
            testw.SendBTC();

            Console.ReadLine();
        }
    }


    class TestWallet 
    {
        List<Wallet> wallets = new List<Wallet>();

        private string walletImportFormat;
        public string WalletImportFormat { get { return walletImportFormat; } set { walletImportFormat = value; } }

        private string toAddress;
        public string ToAddress { get { return toAddress; } set { toAddress = value; } }//=> wallets[1].PublicKey; //"mnDJL3XEYNKCNwYoJg3RNK56ikTp9nQCVU";
        public string Message { get; set; } = "Stuur Abie al my Bitcoins.";

        string historyDebug = string.Empty;
        public string HistoryDebug { get { return historyDebug; } set { historyDebug = value;  } }

        public decimal SendAmount { get; set; } = 0.0001m;
        //public decimal Fee { get; private set; } = 0.00000300m;
        public decimal Fee { get; private set; } = 0.0000300m;

        decimal balance = 0m;
        public decimal Balance { get { return balance; } set { balance = value;  } }

        Network network = null;
        List<UnspentCoin> unspentCoins = new List<UnspentCoin>();

        public TestWallet()
        {
            //var secret = new BitcoinSecret("cS6XuFcnuRQTXSZiBZC7MNkmhGCixUWfNZ4yDkvLCJX3h161LhgX"); //mnDJL3XEYNKCNwYoJg3RNK56ikTp9nQCVU, bitcoin core: importprivkey 
            //var secret = new BitcoinSecret("L1wCJhdePmVds249szRqGkUwYxNhxkLHG9xHfdRyPrD87NfuHmWY"); //  n2z9HfufPiMWUBFtFnJ14hsbtP5FJ7UUKw

            this.network = Network.TestNet;
            this.wallets.Add(new Wallet(network, "cS6XuFcnuRQTXSZiBZC7MNkmhGCixUWfNZ4yDkvLCJX3h161LhgX"));
            this.wallets.Add(new Wallet(network, "L1wCJhdePmVds249szRqGkUwYxNhxkLHG9xHfdRyPrD87NfuHmWY"));


            //this.wallet = new Wallet(network, "L1wCJhdePmVds249szRqGkUwYxNhxkLHG9xHfdRyPrD87NfuHmWY");

            Init();

            //ButtonCommand = new RelayCommand(new Action<object>(Increase));
        }

        void RotateWallets(object o)
        {

            Wallet temp = wallets[0];
            wallets.RemoveAt(0);
            wallets.Add(temp);

            Init();
        }

        RPCClient rpcClient;

        private void Init()
        {
            rpcClient = new RPCClient("Bob:BobPassword", "127.0.0.1:18332", Network.TestNet);

            //var address = rpcClient.GetNewAddress();

            unspentCoins = rpcClient.ListUnspent().ToList();
            balance = rpcClient.GetBalance().ToDecimal(MoneyUnit.BTC);

            WalletImportFormat = wallets[0].WalletImportFormat;
            ToAddress = wallets[1].PublicKey;
        }



        public void SendBTC()
        {
            if (SendAmount > balance) return;
            //List<Coin> toSpend = MinimumCoinsToCoverTransaction(); // Bitcoin doesn't allow spending just the inputs you need from a previous transaction?
            List<UnspentCoin> toSpend = this.unspentCoins.Where(x => x.Amount.ToDecimal(MoneyUnit.BTC) > 0).ToList();

            // For the payment you will need to reference this outpoint in the transaction. You create a transaction as follows:
            var transaction = Transaction.Create(network);
            foreach (var item in toSpend)
            {
                transaction.Inputs.Add(item.OutPoint);
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


            //transaction.Sign(this.wallets[0].Key.GetBitcoinSecret(network), toSpend.Select(x => (ICoin)x).AsEnumerable());
            transaction.Sign(this.wallets[0].Key.GetBitcoinSecret(network), toSpend.Select(x => x.AsCoin()).AsEnumerable());

            rpcClient.SendRawTransaction(transaction);

        }

        internal object GetTransactionData(string v)
        {
            //return Encoding.UTF8.GetString(rpcClient.GetRawTransaction(new uint256(v)).ToBytes());
            return rpcClient.GetRawTransaction(new uint256(v)).ToString(RawFormat.BlockExplorer);
        }
    }

    class Wallet
    {

        Network network;
        Key privateKey;
        internal string PublicKey => privateKey.PubKey.GetAddress(network).ToString();

        internal string WalletImportFormat => privateKey.ToString(network);

        internal Script ScriptPubKey => privateKey.ScriptPubKey;

        public Key Key => privateKey;

        internal Wallet(Network network, string walletImportFormat = null)
        {
            this.network = network;

            if (walletImportFormat != null)
            {
                var secret = new BitcoinSecret(walletImportFormat);
                this.privateKey = secret.PrivateKey;
            }
            else
            {
                privateKey = new Key();
            }
        }
    }
}
