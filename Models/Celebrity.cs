using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Celebrities.Models
{
    public class Celebrity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Profession { get; set; }
        public string ImageUrl { get; set; }

    }
}
