using Core.Database.Models;
using Cysharp.Threading.Tasks;

namespace Core.Database.Login
{
    public interface IGetAwsUser
    {
        UniTask Execute(AwsUserModel awsUserModel);
    }
}