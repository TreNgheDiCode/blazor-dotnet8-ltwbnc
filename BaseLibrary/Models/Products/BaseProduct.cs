using System.Text.Json.Serialization;

namespace BaseLibrary.Models.Products
{
    public class BaseProduct
    {
        // Mối quan hệ đến bảng Sản phẩm: Một - Nhiều
        [JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}
