using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTaskLib.DTOs
{
    public class GetTasks
    {
        public List<Task>? Tasks { get; set; }

        public int Count { get; set; }

    }
}
