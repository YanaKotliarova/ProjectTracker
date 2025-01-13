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
    }
}
