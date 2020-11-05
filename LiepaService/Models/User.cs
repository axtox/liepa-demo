using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiepaService.Models {
    public partial class User {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int StatusId {get;set;}
        public virtual Status Status { get; set; }
    }
}