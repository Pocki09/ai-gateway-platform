namespace AiGateway.SharedKernel.Domain;

/// <summary>
/// Strong-typed ID để tránh nhầm lẫn giữa các loại ID.
/// 
/// Vấn đề nếu dùng Guid thô:
///   void AddMember(Guid workspaceId, Guid consumerId) // Dễ nhầm thứ tự!
/// 
/// Với strong-typed ID:
///   void AddMember(WorkspaceId workspaceId, ConsumerId consumerId) // Compiler bắt lỗi
/// </summary>
public abstract record EntityId(Guid Value)
{
    public static Guid NewId() =>  Guid.NewGuid();
    public override string ToString() => Value.ToString();
}