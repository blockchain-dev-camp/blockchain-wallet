namespace BlockchainWallet.Services
{
    using System.Text;

    public class AddressService
    {
        public void CreateAddress()
        {
            var mnemonic = string.Empty; // todo random words
            var bytes = Encoding.ASCII.GetBytes(mnemonic);

            // todo create private key -> public key -> address

            // todo return Model with data for showing
        }
    }
}
