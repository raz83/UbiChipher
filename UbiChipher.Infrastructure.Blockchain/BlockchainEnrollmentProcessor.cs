using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UbiChipher.Data;

namespace UbiChipher.Infrastructure.Blockchain
{
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

    public class BlockchainEnrollmentProcessor
    {
        private readonly ICoinService CoinService = new BitcoinService(useTestnet: true);
        List<Wallet> wallets = new List<Wallet>();

        public BlockchainEnrollmentProcessor()
        {

        }

        public void Enroll(Enrollment enrollment)
        {
            // TODO:  Move UbiChipher\UbiChipher\Nbitcoin temp\Program.cs, WriteTokenHashToBlockchain() code here.
        }

    }
}
