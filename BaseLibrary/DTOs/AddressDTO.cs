namespace BaseLibrary.DTOs
{
    public record ProvinceList
    {
        public List<ProvinceItem> Provinces { get; init; } = [];
    }

    public record ProvinceItem
    {
        public string Code { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }

    public record DistrictList
    {
        public List<DistrictItem> Districts { get; init; } = [];
    }

    public record DistrictItem
    {
        public string Code { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
    }

    public record WardList
    {
        public List<WardItem> Wards { get; init; } = [];
    }

    public record WardItem
    {
        public string Code { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public string DistrictCode { get; set; } = null!;
    }
}
