using Core.Aws.Models;
using Cysharp.Threading.Tasks;

namespace Core.Aws.Login
{
    public interface IGetAwsUser
    {
        UniTask Execute(AwsUserModel awsUserModel);
    }
}