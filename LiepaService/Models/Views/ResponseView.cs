using System.Xml.Serialization;

namespace LiepaService.Models.Views {

    [XmlRoot("Response")]
    public class ResponseView {

        [XmlAttribute]
        public bool Success { get; set; }

        [XmlAttribute]
        public int ErrorId { get; set; }
        public string ErrorMsg { get; set; }
        public UserView User { get; set; }
    }
}