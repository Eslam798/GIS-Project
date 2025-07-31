using Microsoft.AspNetCore.Mvc;
using UPA.BLL.Interfaces;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.GovernorateVM;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IGovernorateService _governorateService;

        public GovernorateController(IGovernorateService governorateService)
        {
            _governorateService = governorateService;
        }

        [HttpGet]
        [Route("ListGovernorates")]
        public IEnumerable<Governorate> ListGovernorates()
        {
            return _governorateService.ListGovernorates();
        }

        [HttpGet]
        [Route("ListGovernoratesModel")]
        public List<IndexGovernorateVM.GetData> ListGovernoratesModel()
        {
            return _governorateService.ListGovernoratesModel();
        }



    }
}
