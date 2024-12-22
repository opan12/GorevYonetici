using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GörevYönet.Application.RequestParameters
{
    public class PaginatedTaskViewModel
    {
        public List<TaskViewModel> Tasks { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalTasks { get; set; }
    }
}