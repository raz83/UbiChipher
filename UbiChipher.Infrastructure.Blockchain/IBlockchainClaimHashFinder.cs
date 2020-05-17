using Data;

namespace UbiChipher.Infrastructure.Blockchain
{
    public interface IBlockchainClaimHashFinder
    {
        string GetClientClaimFingerPrintFromBlockchain(Claim claim);
    }
}