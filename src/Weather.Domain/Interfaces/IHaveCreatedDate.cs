using System;

namespace Weather.Domain.Interfaces
{
    public interface IHaveCreatedTime
    {
        DateTime Created { get; set; }
    }
}
