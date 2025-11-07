using System.Collections.Generic;
using EducationPlatform.Domain.Common;

namespace EducationPlatform.Domain.Entities;

public class Question : BaseEntity
{
    public string Body { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public ICollection<Choice> Choices { get; set; } = new List<Choice>();
}
