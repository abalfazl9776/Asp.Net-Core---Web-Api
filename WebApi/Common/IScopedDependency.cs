using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    //just to mark
    public interface IScopedDependency
    {
        // Scoped objects are the same within a request, but different across different requests.
    }

    public interface ITransientDependency
    {
        // Transient objects are always different; a new instance is provided to every controller and every service.
    }
    
    public interface ISingletonDependency
    {
        // Singleton objects are the same for every object and every request.
    }
}
