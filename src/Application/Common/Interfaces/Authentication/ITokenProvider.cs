namespace Application.Common.Interfaces.Authentication;


public interface ITokenProvider
{
    string Create(Guid userId);
    string CreateRefreshToken();
}