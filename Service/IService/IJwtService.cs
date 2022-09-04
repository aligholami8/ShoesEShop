using System.Threading.Tasks;

namespace Service.IService
{
    public interface IJwtService
    {
        Task<Model.Token> GenerateAsync(Data.User user);
        string GetTest();
    }
}