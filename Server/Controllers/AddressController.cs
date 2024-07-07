using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    [AllowAnonymous]
    public class AddressController(IAddressRepo addressRepo) : ControllerBase
    {
        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var result = await addressRepo.GetProvinces();
            return Ok(result);
        }

        [HttpGet("provinces/${provinceCode}/districts")]
        public async Task<IActionResult> GetDistricts(string provinceCode)
        {
            var result = await addressRepo.GetDistricts(provinceCode);
            return Ok(result);
        }

        [HttpGet("provinces/${provinceCode}/districts/${districtCode}/wards")]
        public async Task<IActionResult> GetWards(string provinceCode, string districtCode)
        {
            var result = await addressRepo.GetWards(provinceCode, districtCode);
            return Ok(result);
        }
    }
}
