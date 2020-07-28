using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum PaymentMethod
    {
        [EnumMember(Value = "COD")]
        COD,
        [EnumMember(Value = "Bank Transfer")]
        BankTransfer,
        [EnumMember(Value = "COD**")]
        COD_
    }
}
