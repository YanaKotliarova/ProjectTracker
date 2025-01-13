namespace ProjectTracker.MVVM.Model.Interfaces
{
    public interface IProject
    {
        string Description { get; set; }
        int Id { get; set; }
        List<Issue> Issues { get; set; }
        List<string> Labels { get; set; }
        string Name { get; set; }
        int UserId { get; set; }
    }
}