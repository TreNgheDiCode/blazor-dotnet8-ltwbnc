using BaseLibrary.DTOs;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IAddressRepo
    {
        Task<ServiceModel<ProvinceList>> GetProvinces();
        Task<ServiceModel<DistrictList>> GetDistricts(string provinceCode);
        Task<ServiceModel<WardList>> GetWards(string provinceCode, string districtCode);
    }
}
