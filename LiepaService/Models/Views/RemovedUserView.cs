namespace LiepaService.Models.Views {
    public class RemoveUserView {
        public int Id { get; set; }
    }
    public class DeleteRequestView {
        public RemoveUserView RemoveUser { get; set; }
    }
}