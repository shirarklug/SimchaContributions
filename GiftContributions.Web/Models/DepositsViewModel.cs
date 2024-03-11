using GiftContributions.Data;

namespace GiftContributions.Web.Models
{
    public class DepositsViewModel
    {
        public List<Deposit> Deposits { get; set; }
        public decimal Total { get; set; }
    }
}
