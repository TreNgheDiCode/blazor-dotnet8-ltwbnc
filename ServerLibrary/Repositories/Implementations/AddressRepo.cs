using BaseLibrary.DTOs;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Repositories.Implementations
{
    public class AddressRepo(AppDbContext context) : IAddressRepo
    {
        public async Task<ServiceModel<DistrictList>> GetDistricts(string provinceCode)
        {
            // Kiểm tra xem mã tỉnh/thành có hợp lệ không
            if (string.IsNullOrEmpty(provinceCode))
            {
                return new ServiceModel<DistrictList>
                {
                    Message = "Mã tỉnh/thành không được để trống"
                };
            }

            // Kiểm tra xem mã tỉnh/thành có tồn tại không
            var province = await context.Provinces.FirstOrDefaultAsync(p => p.Code == provinceCode);

            // Trả về thông báo mã tỉnh/thành cung cấp không tồn tại
            if (province == null)
            {
                return new ServiceModel<DistrictList>
                {
                    Message = $"Mã tỉnh/thành {provinceCode} không tồn tại"
                };
            }

            // Lấy danh sách quận/huyện của tỉnh/thành
            var districts = await context.Districts.Where(d => d.ProvinceCode == provinceCode)
                .Select(d => new DistrictItem
                {
                    Code = d.Code,
                    FullName = d.FullName
                }).ToListAsync();

            return new ServiceModel<DistrictList> { Data = new DistrictList { Districts = districts }, Success = true, Message = "Lấy danh sách quận/huyện thành công" };
        }

        public async Task<ServiceModel<ProvinceList>> GetProvinces()
        {
            ServiceModel<ProvinceList> result = new ServiceModel<ProvinceList>();

            try
            {
                var provinces = await context.Provinces.Select(p => new ProvinceItem
                {
                    Code = p.Code,
                    FullName = p.FullName
                }).ToListAsync();

                result.Data = new ProvinceList
                {
                    Provinces = provinces
                };

                result.Success = true;
                result.Message = "Lấy danh sách tỉnh/thành thành công";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<ServiceModel<WardList>> GetWards(string provinceCode, string districtCode)
        {
           // Kiểm tra xem mã tỉnh/thành có hợp lệ không
            if (string.IsNullOrEmpty(provinceCode))
            {
                return new ServiceModel<WardList>
                {
                    Message = "Mã tỉnh/thành không được để trống"
                };
            }

            // Kiểm tra xem mã tỉnh/thành có tồn tại không
            var province = await context.Provinces.FirstOrDefaultAsync(p => p.Code == provinceCode);

            // Trả về thông báo mã tỉnh/thành cung cấp không tồn tại
            if (province == null)
            {
                return new ServiceModel<WardList>
                {
                    Message = $"Mã tỉnh/thành {provinceCode} không tồn tại"
                };
            }

            // Kiểm tra xem mã quận/huyện có hợp lệ không
            if (string.IsNullOrEmpty(districtCode))
            {
                return new ServiceModel<WardList>
                {
                    Message = "Mã quận/huyện không được để trống"
                };
            }

            // Kiểm tra xem mã quận/huyện có tồn tại không
            var district = await context.Districts.FirstOrDefaultAsync(d => d.Code == districtCode);

            // Trả về thông báo mã quận/huyện cung cấp không tồn tại
            if (district == null)
            {
                return new ServiceModel<WardList>
                {
                    Message = $"Mã quận/huyện {districtCode} không tồn tại"
                };
            }

            // Lấy danh sách phường/xã của quận/huyện
            var wards = await context.Wards.Where(w => w.DistrictCode == districtCode)
                .Select(w => new WardItem
                {
                    Code = w.Code,
                    FullName = w.FullName,
                    DistrictCode = w.DistrictCode,
                    ProvinceCode = context.Provinces.Where(p => p.Code == provinceCode).Select(p => p.Code).FirstOrDefault()
                }).ToListAsync();

            return new ServiceModel<WardList> { Data = new WardList { Wards = wards }, Success = true, Message = "Lấy danh sách phường/xã thành công" };
        }
    }
}
