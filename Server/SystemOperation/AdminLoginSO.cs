using Common.Domain;
using Server.SystemOperation;

namespace Server
{
    internal class AdminLoginSO : SystemOperationBase
    {
        private readonly Admin _admin;
        public IEntity Result { get; set; }

        public AdminLoginSO(Admin argument)
        {
            _admin = argument;
        }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetEntityByID(_admin);
            if (Result != null)
            {
                foreach (Admin admin in Server.loggedIn)
                {
                    if (admin.AdminId == ((Admin)Result).AdminId)
                    {
                        Admin a = new Admin();
                        a.Ime = "Vec ulogovan";
                        Result = a;
                        return;
                    }
                }
                Server.loggedIn.Add((Admin)Result);
            }
        }
    }
}