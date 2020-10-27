using System;

namespace ZEntityFrameworkExtensionsDemo.Models.Interfaces
{
    public interface ICreatedModified
    {
        DateTimeOffset DateCreated { get; set; }
        DateTimeOffset DateModified { get; set; }
    }
}
