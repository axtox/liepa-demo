using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiepaService.Models {
    
    public partial class Status {
        public int StatusId { get; set; }
        public string Value { get; set; }
    }
}