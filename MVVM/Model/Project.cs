using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectTracker.MVVM.Model.Interfaces;

namespace ProjectTracker.MVVM.Model
{
    public class Project : IProject
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Labels { get; set; } = new();
        public List<Issue> Issues { get; set; } = new();

        public Project() { }

        public Project(int userId, string name, string? description, 
            List<string>? labels = default, List<Issue>? issues = default)
        {
            UserId = userId;
            Name = name;
            Description = description ?? "No description provided";
            Labels = labels ?? new();
            Issues = issues ?? new();
        }
    }
}
