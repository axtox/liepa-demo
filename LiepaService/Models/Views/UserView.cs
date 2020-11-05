using System.Xml.Serialization;

namespace LiepaService.Models.Views {
    [XmlRoot("User")]
    public class UserView { 
        [XmlAttribute]
        public int Id {get;set;}
        [XmlAttribute]
        public string Name {get;set;}
        public string Status {get;set;}

        public UserView(User user) {
            Id = user.UserId;
            Name = user.Name;
            Status = user.Status.Value;
        }
    }
}