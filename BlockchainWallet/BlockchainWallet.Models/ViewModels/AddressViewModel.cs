namespace BlockchainWallet.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddressViewModel
    {
        [Required]
        public string Mnemonic { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string Address { get; set; }
    }
}
