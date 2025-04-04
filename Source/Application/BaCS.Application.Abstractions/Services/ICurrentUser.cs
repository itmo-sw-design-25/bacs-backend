namespace BaCS.Application.Abstractions.Services;

public interface ICurrentUser
{
    public Guid UserId { get; }
    public string Email { get; }
    public bool IsAdminIn(params Guid[] locationIds);
}
