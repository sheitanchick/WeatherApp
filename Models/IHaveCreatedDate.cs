using System;

namespace Weather.Models
{
    public interface IHaveCreatedTime
    {
        DateTime Created { get; set; }
    }
}
