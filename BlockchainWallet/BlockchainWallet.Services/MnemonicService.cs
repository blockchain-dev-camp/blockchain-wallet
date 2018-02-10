namespace BlockchainWallet.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Reflection.Metadata;
    using System.Security.Cryptography;
    using Models.Domain;
    using Utils.Extensions;

    public class MnemonicService
    {
        public string GetMnemonics(List<Entropy> data)
        {
            var parts = MnemonicWords.RequiredWords / 2;
            var segments = data.OrderBy(d => d.R)
                .Select((item, index) => new { index, item })
                .GroupBy(x => x.index % parts)
                .Select(x => x.Select(i => i.item))
                .ToList();

            var nums = new List<BigInteger>();
            foreach (var segment in segments)
            {
                var segData = segment.Select(i =>
                    {
                        BigInteger coords = BigInteger.Parse(i.X.RemovePointSeparator() + i.Y.RemovePointSeparator());
                        BigInteger vel = BigInteger.Multiply(
                            BigInteger.Parse(i.Vx.RemovePointSeparator()),
                            BigInteger.Parse(i.Vy.RemovePointSeparator())
                        );

                        return BigInteger.Add(BigInteger.Abs(coords), BigInteger.Abs(vel));
                    })
                    .ToList();

                var index = this.GetRandomNumber(segData.Count - 1);
                var num = segData[index];
                nums.Add(num);
            }

            var mnemonicWords = new BigInteger(MnemonicWords.Words.Count);
            var pos = nums.Select(n => (int)BigInteger.Remainder(n, mnemonicWords)).ToList();
            var remaining = MnemonicWords.RequiredWords - pos.Count();
            for (int i = 0; i < remaining; i++)
            {
                pos.Add(this.GetRandomNumber(MnemonicWords.Words.Count));
            }

            pos.Shuffle();

            return string.Join(" ", pos.Select(p => MnemonicWords.Words[p]));
        }

        public int GetRandomNumber(int maxNum)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4];
                rng.GetBytes(data);
                int value = BitConverter.ToInt32(data, 0);

                return Math.Abs(value % maxNum);
            }
        }
    }
}
