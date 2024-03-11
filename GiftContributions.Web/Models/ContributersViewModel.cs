using GiftContributions.Data;

namespace GiftContributions.Web.Models
{
    public class ContributorsViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public decimal Total { get; set; }
    }
}
