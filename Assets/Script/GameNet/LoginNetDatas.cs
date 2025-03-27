using System;

namespace GameNet
{
    [Serializable]
    public class SeverNetData: GameNetBase
    {
        public int id=0;
        public string name= "";
        public string config= "";
        public string time_added= "";
        public string time_updated= "";

        public override string ToString()
        {
            return "id:"+id.ToString() + "名称"+name +"peizhi"+ config +"状态"+ status +"时间戳"+ time_added + time_updated;
        }
    }

    [Serializable]
    public class AccountNetData: GameNetBase
    {
        public PlayerLoginData data;
    }

    [Serializable]
    public class PlayerLoginData
    {
        public string game_token;
        public AccountLoginData account;
        public RoleLoginData role;
    }

    [Serializable]
    public class AccountLoginData
    {
        public string account_id;
        public string name;
        public string time_added;
        public string time_updated;
    }

    [Serializable]
    public class RoleLoginData
    {
        public string role_id;
        public string name;
        public int count;
        public string time_added;
        public string time_updated;
    }

    [Serializable]
    public class RenameNetData : GameNetBase
    {
        public ReNameData data;
    }

    [Serializable]
    public class ReNameData
    {
        public string role_id;
        public string old_name;
        public string new_name;
    }

    public class RequestRenameData
    {
        public string name;
    }

}