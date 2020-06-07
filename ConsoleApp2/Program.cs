using BitcoinLib.Auxiliary;
using BitcoinLib.ExceptionHandling.Rpc;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Responses;
using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var testWallet = new TestWallet();



            Console.ReadLine();
        }
    }


    class TestWallet 
    {

        private string walletImportFormat;
        public string WalletImportFormat { get { return walletImportFormat; } set { walletImportFormat = value; } }


        private string toAddress;
        public string ToAddress { get { return toAddress; } set { toAddress = value; } }//=> wallets[1].PublicKey; //"mnDJL3XEYNKCNwYoJg3RNK56ikTp9nQCVU";

        public string Message { get; set; } = "Stuur Abie al my Bitcoins.";


        string historyDebug = string.Empty;
        public string HistoryDebug { get { return historyDebug; } set { historyDebug = value; } }

        public decimal SendAmount { get; set; } = 0.005m;
        public decimal Fee { get; private set; } = 0.00000300m;


        decimal balance = 0m;
        public decimal Balance { get { return balance; } set { balance = value;  } }


        string address1priv = "cS6XuFcnuRQTXSZiBZC7MNkmhGCixUWfNZ4yDkvLCJX3h161LhgX";
        string address1pub = "mnDJL3XEYNKCNwYoJg3RNK56ikTp9nQCVU";

        string address2priv = "L1wCJhdePmVds249szRqGkUwYxNhxkLHG9xHfdRyPrD87NfuHmWY";
        string address2pub = "n2z9HfufPiMWUBFtFnJ14hsbtP5FJ7UUKw";

        bool rotate = false;

        public TestWallet()
        {
            //var secret = new BitcoinSecret("cS6XuFcnuRQTXSZiBZC7MNkmhGCixUWfNZ4yDkvLCJX3h161LhgX"); //mnDJL3XEYNKCNwYoJg3RNK56ikTp9nQCVU, bitcoin core: importprivkey 
            //var secret = new BitcoinSecret("L1wCJhdePmVds249szRqGkUwYxNhxkLHG9xHfdRyPrD87NfuHmWY"); //  n2z9HfufPiMWUBFtFnJ14hsbtP5FJ7UUKw

            ToAddress = address2pub;

            Init();

            // TEST CODE

            var unsent = CoinService.ListUnspent();

            CreateRawTransactionRequest rawTransaction = new CreateRawTransactionRequest();
            rawTransaction.AddInput(unsent[0].TxId, unsent[0].Vout);
            rawTransaction.AddOutput("mnDJL3XEYNKCNwYoJg3RNK56ikTp9nQCVU", 0.0001m);

            var rts = CoinService.CreateRawTransaction(rawTransaction);

            //CoinService.CreateRawTransaction()





        }

        void RotateWallets(object o)
        {
            rotate = !rotate;

            string addressToDump = rotate ? address1pub : address2pub;
            string addressToImport = rotate ? address2priv : address1priv;

            CoinService.DumpPrivKey(addressToDump);
            CoinService.ImportPrivKey(addressToImport);

            Init();
        }

        private void Init()
        {
            //DebugInfo();

            DoBalance();
            //WalletImportFormat = wallets[0].WalletImportFormat;
            //ToAddress = wallets[1].PublicKey;
        }

        void DoBalance()
        {
            Balance = CoinService.GetBalance();
        }

        private void SendBTC(object obj)
        {
            CoinService.SendToAddress(this.ToAddress, this.SendAmount);
            //CoinService.
        }

        void ShowBalance(object o)
        {
            var url = "https://live.blockcypher.com/btc-testnet/address/";// + wallets[0].PublicKey;

            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        void ShowFees(object o)
        {
            var url = "https://bitcoinfees.earn.com/api/v1/fees/recommended";

            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private readonly ICoinService CoinService = new BitcoinService(useTestnet: true);

        private void DebugInfo()
        {
            try
            {
                Debug.WriteLine("\n\nConnecting to {0} {1}Net via RPC at {2}...", CoinService.Parameters.CoinLongName, (CoinService.Parameters.UseTestnet ? "Test" : "Main"), CoinService.Parameters.SelectedDaemonUrl);

                //  Network difficulty
                var networkDifficulty = CoinService.GetDifficulty();
                Debug.WriteLine("[OK]\n\n{0} Network Difficulty: {1}", CoinService.Parameters.CoinLongName, networkDifficulty.ToString("#,###", CultureInfo.InvariantCulture));

                // Mining info
                var miningInfo = CoinService.GetMiningInfo();
                Debug.WriteLine("[OK]\n\n{0} NetworkHashPS: {1}", CoinService.Parameters.CoinLongName, miningInfo.NetworkHashPS.ToString("#,###", CultureInfo.InvariantCulture));

                //  My balance
                var myBalance = CoinService.GetBalance();
                Debug.WriteLine("\nMy balance: {0} {1}", myBalance, CoinService.Parameters.CoinShortName);

                //  Current block
                Debug.WriteLine("Current block: {0}",
                    CoinService.GetBlockCount().ToString("#,#", CultureInfo.InvariantCulture));

                //  Wallet state
                Debug.WriteLine("Wallet state: {0}", CoinService.IsWalletEncrypted() ? "Encrypted" : "Unencrypted");

                //  Keys and addresses
                if (myBalance > 0)
                {
                    //  My non-empty addresses
                    Debug.WriteLine("\n\nMy non-empty addresses:");

                    var myNonEmptyAddresses = CoinService.ListReceivedByAddress();

                    foreach (var address in myNonEmptyAddresses)
                    {
                        Debug.WriteLine("\n--------------------------------------------------");
                        Debug.WriteLine("Account: " + (string.IsNullOrWhiteSpace(address.Account) ? "(no label)" : address.Account));
                        Debug.WriteLine("Address: " + address.Address);
                        Debug.WriteLine("Amount: " + address.Amount);
                        Debug.WriteLine("Confirmations: " + address.Confirmations);
                        Debug.WriteLine("--------------------------------------------------");
                    }

                    //  My private keys
                    if (bool.Parse(ConfigurationManager.AppSettings["ExtractMyPrivateKeys"]) && myNonEmptyAddresses.Count > 0 && CoinService.IsWalletEncrypted())
                    {
                        const short secondsToUnlockTheWallet = 30;

                        Debug.Write("\nWill now unlock the wallet for " + secondsToUnlockTheWallet + ((secondsToUnlockTheWallet > 1) ? " seconds" : " second") + "...");
                        CoinService.WalletPassphrase(CoinService.Parameters.WalletPassword, secondsToUnlockTheWallet);
                        Debug.WriteLine("[OK]\n\nMy private keys for non-empty addresses:\n");

                        foreach (var address in myNonEmptyAddresses)
                        {
                            Debug.WriteLine("Private Key for address " + address.Address + ": " + CoinService.DumpPrivKey(address.Address));
                        }

                        Debug.Write("\nLocking wallet...");
                        CoinService.WalletLock();
                        Debug.WriteLine("[OK]");
                    }

                    //  My transactions 
                    Debug.WriteLine("\n\nMy transactions: ");
                    var myTransactions = CoinService.ListTransactions(null, int.MaxValue, 0);

                    foreach (var transaction in myTransactions)
                    {
                        Debug.WriteLine("\n---------------------------------------------------------------------------");
                        Debug.WriteLine("Account: " + (string.IsNullOrWhiteSpace(transaction.Account) ? "(no label)" : transaction.Account));
                        Debug.WriteLine("Address: " + transaction.Address);
                        Debug.WriteLine("Category: " + transaction.Category);
                        Debug.WriteLine("Amount: " + transaction.Amount);
                        Debug.WriteLine("Fee: " + transaction.Fee);
                        Debug.WriteLine("Confirmations: " + transaction.Confirmations);
                        Debug.WriteLine("BlockHash: " + transaction.BlockHash);
                        Debug.WriteLine("BlockIndex: " + transaction.BlockIndex);
                        Debug.WriteLine("BlockTime: " + transaction.BlockTime + " - " + UnixTime.UnixTimeToDateTime(transaction.BlockTime));
                        Debug.WriteLine("TxId: " + transaction.TxId);
                        Debug.WriteLine("Time: " + transaction.Time + " - " + UnixTime.UnixTimeToDateTime(transaction.Time));
                        Debug.WriteLine("TimeReceived: " + transaction.TimeReceived + " - " + UnixTime.UnixTimeToDateTime(transaction.TimeReceived));

                        if (!string.IsNullOrWhiteSpace(transaction.Comment))
                        {
                            Debug.WriteLine("Comment: " + transaction.Comment);
                        }

                        if (!string.IsNullOrWhiteSpace(transaction.OtherAccount))
                        {
                            Debug.WriteLine("Other Account: " + transaction.OtherAccount);
                        }

                        if (transaction.WalletConflicts != null && transaction.WalletConflicts.Any())
                        {
                            Debug.Write("Conflicted Transactions: ");

                            foreach (var conflictedTxId in transaction.WalletConflicts)
                            {
                                Debug.Write(conflictedTxId + " ");
                            }

                            Debug.WriteLine("");
                        }

                        Debug.WriteLine("---------------------------------------------------------------------------");
                    }

                    //  Transaction Details
                    Debug.WriteLine("\n\nMy transactions' details:");
                    foreach (var transaction in myTransactions)
                    {
                        // Move transactions don't have a txId, which this logic fails for
                        if (transaction.Category == "move")
                        {
                            continue;
                        }

                        var localWalletTransaction = CoinService.GetTransaction(transaction.TxId);
                        IEnumerable<PropertyInfo> localWalletTrasactionProperties = localWalletTransaction.GetType().GetProperties();
                        IList<GetTransactionResponseDetails> localWalletTransactionDetailsList = localWalletTransaction.Details.ToList();

                        Debug.WriteLine("\nTransaction\n-----------");

                        foreach (var propertyInfo in localWalletTrasactionProperties)
                        {
                            var propertyInfoName = propertyInfo.Name;

                            if (propertyInfoName != "Details" && propertyInfoName != "WalletConflicts")
                            {
                                Debug.WriteLine(propertyInfoName + ": " + propertyInfo.GetValue(localWalletTransaction, null));
                            }
                        }

                        foreach (var details in localWalletTransactionDetailsList)
                        {
                            IEnumerable<PropertyInfo> detailsProperties = details.GetType().GetProperties();
                            Debug.WriteLine("\nTransaction details " + (localWalletTransactionDetailsList.IndexOf(details) + 1) + " of total " + localWalletTransactionDetailsList.Count + "\n--------------------------------");

                            foreach (var propertyInfo in detailsProperties)
                            {
                                Debug.WriteLine(propertyInfo.Name + ": " + propertyInfo.GetValue(details, null));
                            }
                        }
                    }

                    //  Unspent transactions
                    Debug.WriteLine("\nMy unspent transactions:");
                    var unspentList = CoinService.ListUnspent();

                    foreach (var unspentResponse in unspentList)
                    {
                        IEnumerable<PropertyInfo> detailsProperties = unspentResponse.GetType().GetProperties();

                        Debug.WriteLine("\nUnspent transaction " + (unspentList.IndexOf(unspentResponse) + 1) + " of " + unspentList.Count + "\n--------------------------------");

                        foreach (var propertyInfo in detailsProperties)
                        {
                            Debug.WriteLine(propertyInfo.Name + " : " + propertyInfo.GetValue(unspentResponse, null));
                        }
                    }
                }

                //Debug.ReadLine();
            }
            catch (RpcInternalServerErrorException exception)
            {
                var errorCode = 0;
                var errorMessage = string.Empty;

                if (exception.RpcErrorCode.GetHashCode() != 0)
                {
                    errorCode = exception.RpcErrorCode.GetHashCode();
                    errorMessage = exception.RpcErrorCode.ToString();
                }

                Debug.WriteLine("[Failed] {0} {1} {2}", exception.Message, errorCode != 0 ? "Error code: " + errorCode : string.Empty, !string.IsNullOrWhiteSpace(errorMessage) ? errorMessage : string.Empty);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
            }
        }
    }

}
