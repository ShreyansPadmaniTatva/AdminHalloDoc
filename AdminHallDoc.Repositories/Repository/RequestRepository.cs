using System;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Repositories.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AdminHalloDoc.Repositories.Repository
{
	public class RequestRepository : IRequestRepository
	{
		private readonly ApplicationDbContext _context;

		public RequestRepository(ApplicationDbContext context)
		{
			_context = context;
		}

        public async Task<int> CountNewRequest()
        {
			int count = 0;
			count = _context.Requests.Count(e => e.Status == 1); ;
			return count;
        }

        public async Task<List<Request>> GetContactAsync()
        {
            return await _context.Requests.Where(r => r.Status == 1).ToListAsync();
        }
    }
}
