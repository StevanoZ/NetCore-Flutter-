using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending, [EnumMember(Value = "Waiting Transfer")]
        WaitingTransfer, [EnumMember(Value = "On Process")]
        OnProcess, [EnumMember(Value = "Failed")]
        Failed, [EnumMember(Value = "Success")]
        Success, [EnumMember(Value = "Waiting Confirmation")]
        WaitingConfirmation
    }
}