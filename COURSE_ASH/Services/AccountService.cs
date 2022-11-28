using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COURSE_ASH.Services
{
    public class AccountService
    {
        public async Task<AccountInfo> CreateAccountAsync(string login, string password, string repeatPassword)
        {
            return await TempServer.CreateAccountAsync(login, password, repeatPassword);
        }
    }
}
