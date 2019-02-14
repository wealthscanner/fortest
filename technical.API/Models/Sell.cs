namespace technical.API.Models
{
    public class Sell
    {
        public int SellerId { get; set; }

        public int AssetId { get; set; }

        public User Seller { get; set; }

        public User Asset { get; set; }
    }
}