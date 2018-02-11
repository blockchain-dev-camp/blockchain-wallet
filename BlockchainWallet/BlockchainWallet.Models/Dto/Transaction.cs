using System;
using Newtonsoft.Json;


namespace BlockchainWallet.Models.Dto
{
    public class Transaction
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("fromAddress")]
        public string FromAddress { get; set; }

        [JsonProperty("toAddress")]
        public string ToAddress { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("senderPubKey")]
        public string SenderPubKey { get; set; }

        [JsonProperty("senderSignature")]
        public string SenderSignature { get; set; }
                       
        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonProperty("paid")]
        public bool Paid { get; set; }

        [JsonProperty("dateReceived")]
        public string DateReceived { get; set; }

        [JsonProperty("dateOfSign")]
        public string DateOfSign { get; set; }

        [JsonProperty("minedInBlockIndex")]
        public long MinedInBlockIndex { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }
        

        public DateTime GetDateReceived()
        {
            var date = new DateTime(1970, 1, 1);

            try
            {
                date.AddMilliseconds(long.Parse(this.DateReceived));
            }
            catch (Exception e)
            {
                
            }

            return date;
        }


    }
}
