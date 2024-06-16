namespace BaseLibrary.DTOs
{
    public record CategoryList
    {
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public List<CategoryItem> Categories { get; init; } = [];
    }

    public record CategoryItem
    {
        public int Id { get; set; } // ID danh mục
        public string? Name { get; set; } // Tên danh mục
    }

    public record CreateCategoryDTO
    {
        public string? Name { get; set; } // Tên danh mục
    }

    public record UpdateCategoryDTO
    {
        public string? Name { get; set; } // Tên danh mục
    }
}
