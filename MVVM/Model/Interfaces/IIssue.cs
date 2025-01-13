namespace ProjectTracker.MVVM.Model.Interfaces
{
    public interface IIssue
    {
        string Comment { get; set; }
        string Description { get; set; }
        int Id { get; set; }
        List<string> Labels { get; set; }
        string Name { get; set; }
        string Priority { get; set; }
        int ProjectId { get; set; }
        string Status { get; set; }
    }
}