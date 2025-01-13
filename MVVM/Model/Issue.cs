using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectTracker.MVVM.Model.Interfaces;

namespace ProjectTracker.MVVM.Model
{
    public class Issue : IIssue
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string Priority { get; set; }
        public List<string> Labels { get; set; } = new();
    }
}
