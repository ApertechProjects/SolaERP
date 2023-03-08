using Microsoft.AspNetCore.Mvc;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Models;

namespace SolaERP.Controllers
{
    //[ApiController]
    //[Route("api/[controller]/[action]")]
    public class OptionController
    {
        public ConfHelper ConfHelper { get; }
        public OptionController(ConfHelper confHelper)
        {
            ConfHelper = confHelper;
        }

        #region ClassModellingActions
        [HttpDelete]
        public async Task SetClassStructureFN()
        {
            string result = (await GetData.FromQuery($"SELECT * FROM dbo.VW_Procedures_List", ConfHelper.DevelopmentUrl)).GetDataTableColumNames();
        }

        [HttpDelete]
        public async void SetClassStructureSP()
        {
            string result = (await GetData.FromQuery($"EXEC SP_ApproveStageRoles_Load 0", ConfHelper.DevelopmentUrl)).GetDataTableColumNames();
        }

        #endregion

        [HttpGet]
        public string Test()
        {
            return "App Running";
        }

    }
}
