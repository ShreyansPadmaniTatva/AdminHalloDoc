using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class PushNotificationRepository : IPushNotificationRepository
    {
        #region Constructor
        private readonly ApplicationDbContext _context;

        public PushNotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region AddNotificationUserData
        public async Task<bool> AddNotificationUserData(string client, string endpoint, string p256dh, string auth)
        {
            try
            {
                var Pushnotificationdata = _context.Pushnotificationdata.Where(r => r.Clientname == client && r.Endpoint == endpoint && r.P256dh == p256dh && r.Auth == auth).FirstOrDefault();

                if (Pushnotificationdata == null )
                {

                    Pushnotificationdatum pushnotificationdata = new Pushnotificationdatum();
                    pushnotificationdata.Clientname = client;
                    pushnotificationdata.Auth = auth;
                    pushnotificationdata.P256dh = p256dh;
                    pushnotificationdata.Endpoint = endpoint;
                    pushnotificationdata.Createddate = DateTime.Now;
                    _context.Pushnotificationdata.Add(pushnotificationdata);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion
        #region GetUserDataList
        public async Task<List<PushNotification>> GetUserDataList()
        {

            try
            {
                List<PushNotification> userDataList = _context.Pushnotificationdata
                                                    .Select(result => new PushNotification
                                                    {
                                                        ClientName = result.Clientname,
                                                        P256dh = result.P256dh,
                                                        EndPoint = result.Endpoint,
                                                        Auth = result.Auth

                                                    }).ToList();
                return userDataList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
        #region GetUserData
        public  PushNotification GetUserData(string aspid)
        {
           
            try
            {
                PushNotification userDataList = _context.Pushnotificationdata
                    .Where(r => r.Clientname == aspid)
                                                    .Select(result => new PushNotification
                                                    {
                                                        ClientName = result.Clientname,
                                                        P256dh = result.P256dh,
                                                        EndPoint = result.Endpoint,
                                                        Auth = result.Auth

                                                    }).FirstOrDefault();
                return userDataList;
            }
            catch (Exception ex)
            {
                return null;
            }   

        }
        #endregion
    }
}
