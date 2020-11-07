using System.Xml.Serialization;

namespace LiepaService.Models.Views {
    [XmlRoot("Request")]
    public class RequestView {
        public UserView User { get; set; }
    }
}