using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using ClientAdminLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class AddressService(GetHttpClient httpClient) : IAddressService
    {
        public const string ProvinceUrl = "api/addresses";

        public async Task<ServiceModel<DistrictList>> GetDistricts(string provinceCode)
        {
            var client = httpClient.GetPublicHttpClient();

            var result = await client.GetFromJsonAsync<ServiceModel<DistrictList>>(ProvinceUrl + "/provinces/" + provinceCode + "/districts");

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<DistrictList>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public async Task<ServiceModel<ProvinceList>> GetProvinces()
        {
            var client = httpClient.GetPublicHttpClient();

            var result = await client.GetFromJsonAsync<ServiceModel<ProvinceList>>(ProvinceUrl + "/provinces");

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<ProvinceList>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public async Task<ServiceModel<WardList>> GetWards(string provinceCode, string districtCode)
        {
            var client = httpClient.GetPublicHttpClient();

            var result = await client.GetFromJsonAsync<ServiceModel<WardList>>(ProvinceUrl + "/provinces/" + provinceCode + "/districts/" + districtCode + "/wards");

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<WardList>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }
    }
}
