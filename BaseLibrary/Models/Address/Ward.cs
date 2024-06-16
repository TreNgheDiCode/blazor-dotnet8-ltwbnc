using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KimVinhHung.Api.Models.Address;

public partial class Ward
{
    [Key]
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? NameEn { get; set; }

    public string? FullName { get; set; }

    public string? FullNameEn { get; set; }

    public string? CodeName { get; set; }

    public string? DistrictCode { get; set; }

    public int? AdministrativeUnitId { get; set; }

    public virtual AdministrativeUnit? AdministrativeUnit { get; set; }

    public virtual District? DistrictCodeNavigation { get; set; }
}
