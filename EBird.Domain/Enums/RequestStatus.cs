using System.ComponentModel;

namespace EBird.Domain.Enums
{
    public enum RequestStatus
    {
        [Description("Waiting")]
        Waiting,
        [Description("Matched")]
        Matched,
        Closed
    }
}