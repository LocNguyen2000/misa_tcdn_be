namespace MISA.Web02.Core.MISAAttribute
{
    /// <summary>
    /// Attribute để chỉ định trường của entity là các khóa chính
    /// </summary>
    /// CREATED BY NHLOC - 20/04/2022
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey: Attribute
    {

    }
    /// <summary>
    /// Attribute để chỉ định trường sẽ không được lưu vào CSDL
    /// </summary>
    /// CREATED BY NHLOC - 26/04/2022
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMappedProp : Attribute
    {

    }

    /// <summary>
    /// Attribute để chỉ định trường không được để trống
    /// </summary>
    /// CREATED BY NHLOC - 20/04/2022
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmpty : Attribute
    {
        // Định nghĩa ErrorMsg text báo lỗi nếu trường đó bị trống
        public string? ErrorMsg { get; set; }
        public NotEmpty(string isError)
        {
            ErrorMsg = isError;
        }
    }

    /// <summary>
    /// Attribute để chỉ định tên tiếng việt của trường đó
    /// </summary>
    /// CREATED BY NHLOC - 26/04/2022
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayName : Attribute
    {
        // Định nghĩa trường Name để gán tên
        public string? Name { get; set; }
        public DisplayName(string name)
        {
            Name = name;
        }
    }
    /// <summary>
    /// Attribute để chỉ định trường đó là Guid
    /// </summary>
    /// CREATED BY NHLOC - 26/04/2022
    [AttributeUsage(AttributeTargets.Property)]
    public class IsGuid : Attribute
    {
    }
}