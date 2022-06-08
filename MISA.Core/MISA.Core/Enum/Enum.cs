using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Enum
{
    /// <summary>
    /// Giới tính:
    /// Male
    /// Created By: Ng Huu Loc - 12/04/2022
    /// </summary>
    public enum GenderEnum
    {
        Male=1,
        Female=0,
        Other=2
    }
    /// <summary>
    /// Trạng thái làm việc
    /// Working: Đang làm việc
    /// Stop: Đã nghỉ việc
    /// Holiday: Nghỉ phép
    /// Created By: Ng Huu Loc - 12/04/2022
    /// </summary>
    public enum WorkStatusEnum
    {
        Working=0,
        Stop=1,
        Holiday=2,
    }
}
