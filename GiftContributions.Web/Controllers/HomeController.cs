using GiftContributions.Data;
using GiftContributions.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace GiftContributions.Web.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = @"Data Source =.\sqlexpress;Initial Catalog = GiftContributions; Integrated Security = true; TrustServerCertificate=true";

        public IActionResult Index()
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            SimchosViewModel vm = new();
            vm.Simchos = mgr.GetSimchos();
            foreach(Simcha s in vm.Simchos)
            {
                int simchaId = s.SimchaId;
            }
            return View(vm);
        }

        public IActionResult Contributors()
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            ContributorsViewModel vm = new();
            vm.Contributors = mgr.GetContributors();
            vm.Total = mgr.GetDepositSum();
            return View(vm);
        }

        [HttpPost]
        public IActionResult NewSimcha(string simchaName, DateTime simchaDate)
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            mgr.AddSimcha(simchaName, simchaDate);
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult AddContributor(Contributor c)
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            mgr.AddContributor(c);
            return Redirect("/Home/Contributors");
        }

        [HttpPost]
        public IActionResult AddDeposit(Deposit d, int contributorId)
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            mgr.AddDeposit(d);
            return Redirect("/Home/Contributors");
        }

        public IActionResult AddContribution(int id)//This shows the contribution window
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            ContributionsViewModel vm = new();
            vm.Contributors = mgr.GetContributors();
            vm.SimchaId = id;
            return View(vm);
        }

        [HttpPost]//This posts the contributions
        public IActionResult AddContributions(List<ContributionInclusion> ci, int simchaId)
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            mgr.DeleteContributions(simchaId);
            foreach (ContributionInclusion c in ci)
            {
                if (c.Include == true)
                {
                    mgr.AddContribution(c.Amount, c.ContributorId, simchaId);
                    mgr.MinusFromContributor(c.ContributorId, c.Amount, DateTime.Now);
                }
            }
            return Redirect("/Home/Index");
        }

        public IActionResult ViewDeposits (int contributorId)
        {
            GiftContributionsManager mgr = new GiftContributionsManager(connectionString);
            DepositsViewModel vm = new DepositsViewModel();
            vm.Deposits = mgr.GetDeposits(contributorId);
            vm.Total = mgr.GetDepositSumForContributor(contributorId);
            return View(vm);
        }

    }
}
