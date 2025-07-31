using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPA.BLL.Interface;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISetting setting;
        string strInsitute, strInsituteAr, strLogo = "",CopyRight, CopyRightAr;




        public SettingsController(ISetting setting)
        {
            this.setting = setting;
        }
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
           

          


             
                var lstSettings = setting.GetAll().ToList();
                if (lstSettings.Count > 0)
                {
                    foreach (var item in lstSettings)
                    {
                        if (item.KeyName == "Institute")
                        {
                            strInsitute = item.KeyValue;
                            strInsituteAr = item.KeyValueAr;
                        }

                        if (item.KeyName == "Logo")
                            strLogo = item.KeyValue;

                  if(item.KeyName == "CopyRight")
                    {

                        CopyRight = item.KeyValue;
                        CopyRightAr = item.KeyValueAr;
                    }
              

                          

                       



                    }
                    
                }

                return Ok(new
                {
              
                    strInsitute = strInsitute,
                    strInsituteAr = strInsituteAr,
                    strLogo = strLogo,
                   CopyRight = CopyRight,
                   CopyRightAr = CopyRightAr
                });
            }
           
        
    }
}
