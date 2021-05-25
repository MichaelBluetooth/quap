using System;

namespace Quap.Permissions
{
    public interface IOwnerable
    {
        Guid getOwnerId();
    }
}