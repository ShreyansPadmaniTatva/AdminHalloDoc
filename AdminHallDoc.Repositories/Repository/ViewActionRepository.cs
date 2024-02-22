using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class ViewActionRepository : IViewActionRepository
    {
        #region Configuration
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public ViewActionRepository(ApplicationDbContext context, EmailConfiguration emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        public Boolean SendLink(string firstname, string lastname, string email, string phonenumber)
        {

            _emailConfig.SendMail(email, "add new request", "send lnk");

            return true;
        }
    }
}
