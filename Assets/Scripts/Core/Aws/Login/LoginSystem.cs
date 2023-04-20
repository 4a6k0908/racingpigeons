using Core.Player.Models;
using Cysharp.Threading.Tasks;

namespace Core.Aws.Login
{
    public class LoginSystem
    {
        private readonly PlayerData playerData;

        public LoginSystem(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public async UniTask Login(IGetAwsUser getAwsUser) => await getAwsUser.Execute(playerData.GetAwsUserModel());
    }
}