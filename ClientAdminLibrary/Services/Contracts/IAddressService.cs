using BaseLibrary.DTOs;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IAddressService
    {
        Task<ServiceModel<ProvinceList>> GetProvinces();
        Task<ServiceModel<DistrictList>> GetDistricts(string provinceCode);
        Task<ServiceModel<WardList>> GetWards(string provinceCode, string districtCode);
    }
}
