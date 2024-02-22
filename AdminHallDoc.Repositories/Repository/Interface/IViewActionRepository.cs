using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IViewActionRepository
    {
        public Boolean SendLink(string firstname, string lastname, string email, string phonenumber);
    }
}
