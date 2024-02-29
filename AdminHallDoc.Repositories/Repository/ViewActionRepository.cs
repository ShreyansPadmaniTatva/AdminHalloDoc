using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.AdminViewModel.ViewDocuments;
using Request = Org.BouncyCastle.Asn1.Ocsp.Request;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class ViewActionRepository : IViewActionRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public ViewActionRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region GetDocumentByRequest
        public async Task<ViewDocuments> GetDocumentByRequest(int? id)
        {
            ViewDocuments doc = await (from req in _context.Requests
                                       join reqClient in _context.Requestclients
                                       on req.Requestid equals reqClient.Requestid into reqClientGroup
                                       from rc in reqClientGroup.DefaultIfEmpty()
                                       where req.Requestid == id
                                       select new ViewDocuments
                                       {
                                           ConfirmationNumber = req.Confirmationnumber,
                                           Firstanme = rc.Firstname ,
                                           Lastanme = rc.Lastname,
                                           RequestID = req.Requestid

                                       }).FirstAsync();

            List<Documents> doclist = _context.Requestwisefiles
                        .Where(r => r.Requestid == id)
                        .OrderByDescending(x => x.Createddate)
                        .Select(r => new Documents
                        {
                            RequestwisefilesId = r.Requestwisefileid,
                            Status = r.Doctype,
                            Createddate = r.Createddate,
                            Filename = r.Filename

                        }).ToList();
            doc.documentslist = doclist;
            return doc;
            
        }
        #endregion

        #region SendFilEmail
        public async Task<bool> SendFilEmail(string ids,int Requestid)
        {

            var v = await GetRequestDetails(Requestid);

            List<int> priceList = ids.Split(',').Select(int.Parse).ToList();
            List<string> files = new List<string>();
            foreach (int price in priceList)
            {
                if (price > 0)
                {
                    var data = await _context.Requestwisefiles.Where(e => e.Requestwisefileid == price).FirstOrDefaultAsync();
                    files.Add(Directory.GetCurrentDirectory() + "\\wwwroot\\Upload" + data.Filename.Replace("Upload/", "").Replace("/", "\\"));
                }
            }

           if( _emailConfig.SendMail(v.Email, "All Document Of Your Request "+v.PatientName,"Heeyy " + v.PatientName+" Kindly Check your Attachments", files))
            {
                return true;
            }
            else {
                return false;
            }

        }
        #endregion

        #region DeleteDocumentByRequest
        public async Task<bool> DeleteDocumentByRequest(string ids)
        {
            List<int> priceList = ids.Split(',').Select(int.Parse).ToList();
            foreach (int price in priceList)
            {
                if (price > 0)
                {
                    var data = await _context.Requestwisefiles.Where(e => e.Requestwisefileid == price).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        try
                        {
                            var v = Directory.GetCurrentDirectory() + "\\wwwroot\\Upload" + data.Filename.Replace("Upload/", "").Replace("/", "\\");
                            // Check if file exists with its full path
                            if (File.Exists(v))
                            {
                                // If file found, delete it
                                File.Delete(v);
                                Console.WriteLine("File deleted.");
                            }
                            else Console.WriteLine("File not found");
                        }
                        catch (IOException ioExp)
                        {
                            Console.WriteLine(ioExp.Message);
                        }

                        _context.Requestwisefiles.Remove(data);
                        _context.SaveChanges();
                        
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;


        }
        #endregion

        #region GetRequestDetails
        public async Task<ViewActions> GetRequestDetails(int? id)
        {

            return await (from req in _context.Requests
                        join reqClient in _context.Requestclients
                        on req.Requestid equals reqClient.Requestid into reqClientGroup
                        from rc in reqClientGroup.DefaultIfEmpty()
                          join phys in _context.Physicians
                        on req.Physicianid equals phys.Physicianid into physGroup
                          from p in physGroup.DefaultIfEmpty()
                          where req.Requestid == id
                        select new ViewActions
                        {
                            PhoneNumber = rc.Phonenumber,
                            ProviderId = p.Physicianid,
                            PatientName = rc.Firstname + rc.Lastname,
                            RequestID = req.Requestid,
                            Email = rc.Email

                        }).FirstAsync();
        }
        #endregion

        #region Send_Link
        public Boolean SendLink(string firstname, string lastname, string email, string phonenumber)
        {
            var baseUrl = httpContextAccessor.HttpContext?.Request.Host;

            _emailConfig.SendMail(email, "Add New Request", firstname +" "+ lastname +" "+ phonenumber+"  " +baseUrl+ "/SubmitRequest");

            return true;
        }
        #endregion

        #region Save_Document
        public Boolean SaveDoc(int Requestid, IFormFile file)
        {
            string UploadDoc = CM.UploadDoc(file, Requestid);

            var requestwisefile = new Requestwisefile
            {
                Requestid = Requestid,
                Filename = UploadDoc,
                Createddate = DateTime.Now,
            };
            _context.Requestwisefiles.Add(requestwisefile);
            _context.SaveChanges();

            return true;
        }
        #endregion

        #region Provider_By_Region
        public async Task<List<Physician>> ProviderbyRegion(int? regionid)
        {
            var result = await _context.Physicians
                        .Where(r => r.Regionid == regionid)
                        .OrderByDescending(x => x.Createddate)
                        .ToListAsync();

            return result;
        }
        #endregion

        #region Assign_Provider
        public async Task<Boolean> AssignProvider(int RequestId, int ProviderId, string notes)
        {
            
                var request = await _context.Requests.FirstOrDefaultAsync(req => req.Requestid == (int)RequestId);
                request.Physicianid = ProviderId;
                _context.Requests.Update(request);
                _context.SaveChanges();

                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = RequestId;
                rsl.Physicianid = ProviderId;
                rsl.Notes = notes;
                rsl.Createddate = DateTime.Now;
                rsl.Status = 1;
                _context.Requeststatuslogs.Update(rsl);
                _context.SaveChanges();

                return true;
           
            
        }
        #endregion

        #region Transfer_Provider
        public async Task<bool> TransferToProvider(ViewActions v)
        {

            var request = await _context.Requests.FirstOrDefaultAsync(req => req.Requestid == v.RequestID);
            request.Physicianid = v.ProviderId;
            _context.Requests.Update(request);
            _context.SaveChanges();

            Requeststatuslog rsl = new Requeststatuslog();
            rsl.Requestid = (int)v.RequestID;
            rsl.Physicianid = v.ProviderId;
            rsl.Transtophysicianid = v.TransferToProviderId;
            rsl.Notes = v.Notes;
            rsl.Createddate = DateTime.Now;
            rsl.Status = 1;
            _context.Requeststatuslogs.Update(rsl);
            _context.SaveChanges();

            return true;


        }
        #endregion

        #region CancelCase

        public async Task<bool> CancelCase(ViewActions v,string ReasonTag)
        {
            try
            {
                var requestData = await _context.Requests.Where(e => e.Requestid == v.RequestID).FirstAsync();
                if (requestData != null)
                {
                    requestData.Casetag = ReasonTag;
                    requestData.Status = 8;
                    _context.Requests.Update(requestData);
                    _context.SaveChanges();

                    Requeststatuslog rsl = new Requeststatuslog
                    {
                        Requestid = (int)v.RequestID,
                        Notes = v.Notes,
                        Status = 8,
                        Createddate = DateTime.Now
                    };
                    _context.Requeststatuslogs.Add(rsl);
                    _context.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #endregion

        #region BlockCase
        public async Task<bool> BlockCase(ViewActions v)
        {

            try
            {
                var requestData = await _context.Requests.Where(e => e.Requestid == v.RequestID).FirstAsync();
                if (requestData != null)
                {

                    requestData.Status = 11;
                    _context.Requests.Update(requestData);
                    _context.SaveChanges();
                    Blockrequest blc = new Blockrequest
                    {
                        Requestid = requestData.Requestid.ToString(),
                        Phonenumber = requestData.Phonenumber,
                        Email = requestData.Email,
                        Reason = v.Notes,
                        Createddate = DateTime.Now,
                        Modifieddate = DateTime.Now


                    };
                    _context.Blockrequests.Add(blc);
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

        #region Assign_Physician
        public async Task<bool> AssignPhysician(ViewActions v)
        {

            var request = await _context.Requests.FirstOrDefaultAsync(req => req.Requestid == v.RequestID);
            request.Physicianid = v.ProviderId;
            request.Status = 2;
            _context.Requests.Update(request);
            _context.SaveChanges();

            Requeststatuslog rsl = new Requeststatuslog();
            rsl.Requestid = (int)v.RequestID;
            rsl.Physicianid = v.ProviderId;
            rsl.Notes = v.Notes;
            rsl.Createddate = DateTime.Now;
            rsl.Status = 2;
            _context.Requeststatuslogs.Update(rsl);
            _context.SaveChanges();

            return true;


        }
        #endregion

        #region SendAgreement
        public Boolean SendAgreement(ViewActions v)
        {

            var res = _context.Requestclients.FirstOrDefault(e => e.Requestid == v.RequestID);

            var agreementUrl = "https://localhost:44376/SendAgreement?RequestID="+ v.RequestID;

            _emailConfig.SendMail(res.Email, "Agreement for your request", $"<a href='{agreementUrl}'>Agree/Disagree</a>");
            return true;
        }
        #endregion

        #region SendAgreement_accept
        public Boolean SendAgreement_accept(int RequestID)
        {
            var request = _context.Requests.Find(RequestID);
            if (request != null)
            {
                request.Status = 4;
                _context.Requests.Update(request);
                _context.SaveChanges();

                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = RequestID;

                rsl.Status = 4;

                rsl.Createddate = DateTime.Now;

                _context.Requeststatuslogs.Add(rsl);
                _context.SaveChanges();

            }
            return true;
        }
        #endregion

        #region SendAgreement_Reject
        public Boolean SendAgreement_Reject(int RequestID, string Notes)
        {
            var request = _context.Requests.Find(RequestID);
            if (request != null)
            {
                request.Status = 7;
                _context.Requests.Update(request);
                _context.SaveChanges();

                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = RequestID;

                rsl.Status = 7;
                rsl.Notes = Notes;

                rsl.Createddate = DateTime.Now;

                _context.Requeststatuslogs.Add(rsl);
                _context.SaveChanges();

            }
            return true;
        }
        #endregion

    }
}
