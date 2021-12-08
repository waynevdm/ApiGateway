using AccountModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private static Account _account = new Account
        {
            Number = "1067893425",
            Firstname = "Wayne",
            Surname = "van der Merwe",
            Balance = 0.01M,
        };      

        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountNumber}")]
        public Account Get(string accountNumber)
        {
            return _account;
        }

        [HttpPost]
        [Route("deposit")]
        [Authorize]
        public Account Deposit(AccountUpdate model)
        {
            _account.Balance += model.Amount;
            return _account;
        }

        [HttpPost]
        [Route("withdraw")]
        [Authorize]
        public Account Withdraw(AccountUpdate model)
        {
            _account.Balance -= model.Amount;
            return _account;
        }
    }
}
