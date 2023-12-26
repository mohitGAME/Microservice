using System.Collections.Generic;
using System.Security.Claims;

namespace Mango.GatewaySolution
{
    public class GraphQLUserContext : Dictionary<string, object>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
