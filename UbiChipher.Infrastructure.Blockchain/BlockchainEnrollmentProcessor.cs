using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UbiChipher.Data;

namespace UbiChipher.Infrastructure.Blockchain
{
    public class BlockchainEnrollmentProcessor
    {
        private readonly ICoinService CoinService = new BitcoinService(useTestnet: true);

        public void Enroll(Enrollment enrollment)
        {
            var userAdd = enrollment.Claims[0].PubKey; // Obviously bad code, it's late and I am just testing something.

            var transactionToScanForClaim = CoinService.ListTransactions();
            var txid = transactionToScanForClaim.Last().TxId; // But it may not actually be the last one

            var tx = CoinService.GetTxOut(txid, 0);
            var txx = CoinService.GetTransaction(txid);
            var raw = CoinService.GetRawTransaction(txid, 1);

            var txs = CoinService.ListUnspent();

            var createRawTransactionInput = new CreateRawTransactionInput();
            var createRawTransactionRequest = new CreateRawTransactionRequest()

            CoinService.CreateRawTransaction()

        }
    }
}
