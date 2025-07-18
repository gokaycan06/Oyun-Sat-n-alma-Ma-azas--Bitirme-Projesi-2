namespace Oyun_Mağaza.Models.DTOs
{
    public class LoadBalanceRequest
    {
        public decimal Amount { get; set; }
        public decimal Bonus { get; set; }
        public string PaymentMethod { get; set; }
    }
} 