using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Account
{
    public class UpgradeUserToProCommand : IRequest<Unit>
    {
        public string UserEmail { get; }
        public DateTime TransactionDate { get; }
        public decimal? TransactionValue { get; }

        public UpgradeUserToProCommand(string userEmail, DateTime transactionDate, decimal? transactionValue)
        {
            UserEmail = userEmail;
            TransactionDate = transactionDate;
            TransactionValue = transactionValue;
        }
    }
}
