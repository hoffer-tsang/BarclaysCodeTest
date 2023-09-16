using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTaskLib.Enums;

namespace TodoTaskLib.DTOs
{
    public class PostTask
    {
        public string Name { get; set; } = string.Empty;

        public int? Priority { get; set; }

        public Status? Status { get; set; }
    }
}
