﻿using AdminHalloDoc.Entities.Data;
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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
                                           Email = rc.Email,
                                           PhoneNumber = rc.Phonenumber,
                                           DOB = new DateTime((int)rc.Intyear, (int)Convert.ToInt32(rc.Strmonth), (int)rc.Intdate),
                                           Firstanme = rc.Firstname ,
                                           Lastanme = rc.Lastname,
                                           RequestID = req.Requestid,
                                           RequesClientid = rc.Requestclientid

                                       }).FirstAsync();

            List<Documents> doclist = _context.Requestwisefiles
                        .Where(r => r.Requestid == id )
                        .ToList()
                        .Where(r => r.Isdeleted.Get(0) == false)
                        .OrderByDescending(x => x.Createddate)
                        .Select(r => new Documents
                        {
                            isDeleted = r.Isdeleted.ToString(),
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
        public async Task<bool> SendFilEmail(string ids,int Requestid, string email)
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

           if(await _emailConfig.SendMailAsync(email, "All Document Of Your Request "+v.PatientName,"Heeyy " + v.PatientName+" Kindly Check your Attachments", files))
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
                        //try
                        //{
                        //    var v = Directory.GetCurrentDirectory() + "\\wwwroot\\Upload" + data.Filename.Replace("Upload/", "").Replace("/", "\\");
                        //    // Check if file exists with its full path
                        //    if (File.Exists(v))
                        //    {
                        //        // If file found, delete it
                        //        File.Delete(v);
                        //        Console.WriteLine("File deleted.");
                        //    }
                        //    else Console.WriteLine("File not found");
                        //}
                        //catch (IOException ioExp)
                        //{
                        //    Console.WriteLine(ioExp.Message);
                        //}
                        data.Isdeleted[0] = true;

                        _context.Requestwisefiles.Update(data);
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
                Isdeleted = new BitArray(1),
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
            rsl.Status = 2;
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
           // request.Status = 2;
            _context.Requests.Update(request);
            _context.SaveChanges();

            Requeststatuslog rsl = new Requeststatuslog();
            rsl.Requestid = (int)v.RequestID;
            rsl.Physicianid = v.ProviderId;
            rsl.Notes = v.Notes;
            rsl.Createddate = DateTime.Now;
            //rsl.Status = 2;
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

        #region Clear_Case
        public async Task<bool> ClearCase(int RequestID)
        {
            try
            {
                var requestData = await _context.Requests.Where(e => e.Requestid == RequestID).FirstOrDefaultAsync();
                if (requestData != null)
                {

                    requestData.Status = 10;
                    _context.Requests.Update(requestData);
                    await _context.SaveChangesAsync();

                    Requeststatuslog rsl = new Requeststatuslog
                    {
                        Requestid = RequestID,
                        Status = 10,
                        Createddate = DateTime.Now
                    };
                    _context.Requeststatuslogs.Add(rsl);
                    await _context.SaveChangesAsync();
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

        #region CloseCase
        public async Task<bool> CloseCase(int RequestID)
        {
            try
            {
                var requestData = await _context.Requests.Where(e => e.Requestid == RequestID).FirstOrDefaultAsync();
                if (requestData != null)
                {

                    requestData.Status = 9;
                    _context.Requests.Update(requestData);
                    await _context.SaveChangesAsync();

                    Requeststatuslog rsl = new Requeststatuslog
                    {
                        Requestid = RequestID,
                        Status = 9,
                        Createddate = DateTime.Now
                    };
                    _context.Requeststatuslogs.Add(rsl);
                    await _context.SaveChangesAsync();
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



        #region Encounter
        public ViewEncounter GetEncounterDetailsByRequestID(int RequestID)
        {
            var datareq = _context.Requestclients.FirstOrDefault(e => e.Requestid == RequestID);
            var Data = _context.Encounterforms.FirstOrDefault(e => e.Requestid == RequestID);
            int? a = datareq.Intyear != null ? datareq.Intyear : 0001;
            int? b = Convert.ToInt32(datareq.Strmonth!=null? datareq.Strmonth:1);
            int? c = datareq.Intdate != null ? (int)datareq.Intdate : 01;

            DateTime? fd = new DateTime(datareq.Intyear != null ? (int)datareq.Intyear : 0001, Convert.ToInt32(datareq.Strmonth != null ? datareq.Strmonth : 1), datareq.Intdate != null ? (int)datareq.Intdate : 01) != new DateTime(0001, 01, 01) ? new DateTime((int)datareq.Intyear != 0 ? (int)datareq.Intyear : 1, Convert.ToInt32(datareq.Strmonth != null ? datareq.Strmonth : 1), (int)datareq.Intdate != 0 ? (int)datareq.Intdate : 1) : null;
            if (Data != null)
            {
                ViewEncounter enc = new ViewEncounter
                {
                    ABD = Data.Abd,
                    EncounterID = Data.Encounterformid,
                    Allergies = Data.Allergies,
                    BloodPressureD = Data.Bloodpressurediastolic,
                    BloodPressureS = Data.Bloodpressurediastolic,
                    Chest = Data.Chest,
                    CV = Data.Cv,
                    DOB = new DateTime(datareq.Intyear != null? (int)datareq.Intyear :0001, Convert.ToInt32(datareq.Strmonth != null ? datareq.Strmonth : 1), datareq.Intdate != null ? (int)datareq.Intdate : 01) != new DateTime(0001,01,01)? new DateTime((int)datareq.Intyear != 0 ? (int)datareq.Intyear : 1, Convert.ToInt32(datareq.Strmonth != null ? datareq.Strmonth : 1), (int)datareq.Intdate != 0 ? (int)datareq.Intdate : 1) : null,
                    Date = DateTime.Now,
                    Diagnosis = Data.Diagnosis,
                    Hr = Data.Hr,
                    HistoryOfMedical = Data.Medicalhistory,
                    Heent = Data.Heent,
                    Extr = Data.Extremities,
                    PhoneNumber = datareq.Phonenumber,
                    Email = datareq.Email,
                    HistoryOfP = Data.Historyofpresentillnessorinjury,
                    FirstName = datareq.Firstname,
                    LastName = datareq.Lastname,
                    Followup = Data.Followup,
                    Location = datareq.Location,
                    Medications = Data.Medications,
                    MedicationsDispensed = Data.Medicaldispensed,
                    Neuro = Data.Neuro,
                    O2 = Data.O2,
                    Other = Data.Other,
                    Pain = Data.Pain,
                    Procedures = Data.Procedures,
                    Isfinalize = Data.Isfinalize,
                    Requesid = RequestID,
                    Rr = Data.Rr,
                    Skin = Data.Skin,
                    Temp = Data.Temp,
                    Treatment = Data.TreatmentPlan





                };
                return enc;
            }
            else
            {
                if (datareq != null)
                {
                    ViewEncounter enc = new ViewEncounter
                    {
                        FirstName = datareq.Firstname,
                        PhoneNumber = datareq.Phonenumber,
                        LastName = datareq.Lastname,
                        Location = datareq.Location,
                        DOB = new DateTime(datareq.Intyear != null ? (int)datareq.Intyear : 0001, Convert.ToInt32(datareq.Strmonth != null ? datareq.Strmonth : 1), datareq.Intdate != null ? (int)datareq.Intdate : 01) != new DateTime(0001, 01, 01) ? new DateTime((int)datareq.Intyear != 0 ? (int)datareq.Intyear : 1, Convert.ToInt32(datareq.Strmonth != null ? datareq.Strmonth : 1), (int)datareq.Intdate != 0 ? (int)datareq.Intdate : 1) : null,
                        Date = DateTime.Now,
                        Requesid = RequestID,

                        Email = datareq.Email,
                    };
                    return enc;
                }
                else
                {
                    return new ViewEncounter();
                }


            }



        }



        public bool EditEncounterDetails(ViewEncounter Data, string id)
        {
            //try
            //{
            var admindata = _context.Admins.FirstOrDefault(e => e.Aspnetuserid == id);
            if (Data.EncounterID == 0)
            {
                Encounterform enc = new Encounterform
                {
                    Abd = Data.ABD,
                    Encounterformid = (int)Data.EncounterID,
                    Allergies = Data.Allergies,
                    Bloodpressurediastolic = Data.BloodPressureD,
                    Bloodpressuresystolic = Data.BloodPressureS,
                    Chest = Data.Chest,
                    Cv = Data.CV,
                    Diagnosis = Data.Diagnosis,
                    Hr = Data.Hr,
                    Medicalhistory = Data.HistoryOfMedical,
                    Heent = Data.Heent,
                    Extremities = Data.Extr,
                    Historyofpresentillnessorinjury = Data.HistoryOfP,
                    Followup = Data.Followup,
                    Medications = Data.Medications,
                    Medicaldispensed = Data.MedicationsDispensed,
                    Neuro = Data.Neuro,
                    O2 = Data.O2,
                    Other = Data.Other,
                    Pain = Data.Pain,
                    Procedures = Data.Procedures,
                    Requestid = Data.Requesid,
                    Rr = Data.Rr,
                    Skin = Data.Skin,
                    Temp = Data.Temp,
                    TreatmentPlan = Data.Treatment,
                    Adminid = admindata.Adminid,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,


                };
                _context.Encounterforms.Add(enc);
                _context.SaveChanges();

                return true;

            }
            else
            {
                var encdetails = _context.Encounterforms.FirstOrDefault(e => e.Encounterformid == Data.EncounterID);
                if (encdetails != null)
                {
                    encdetails.Abd = Data.ABD;
                    encdetails.Encounterformid = (int)Data.EncounterID;
                    encdetails.Allergies = Data.Allergies;
                    encdetails.Bloodpressurediastolic = Data.BloodPressureD;
                    encdetails.Bloodpressuresystolic = Data.BloodPressureS;
                    encdetails.Chest = Data.Chest;
                    encdetails.Cv = Data.CV;
                    encdetails.Diagnosis = Data.Diagnosis;
                    encdetails.Hr = Data.Hr;
                    encdetails.Medicalhistory = Data.HistoryOfMedical;
                    encdetails.Heent = Data.Heent;
                    encdetails.Extremities = Data.Extr;
                    encdetails.Historyofpresentillnessorinjury = Data.HistoryOfP;
                    encdetails.Followup = Data.Followup;
                    encdetails.Medications = Data.Medications;
                    encdetails.Medicaldispensed = Data.MedicationsDispensed;
                    encdetails.Neuro = Data.Neuro;
                    encdetails.O2 = Data.O2;
                    encdetails.Other = Data.Other;
                    encdetails.Pain = Data.Pain;
                    encdetails.Procedures = Data.Procedures;
                    encdetails.Requestid = Data.Requesid;
                    encdetails.Rr = Data.Rr;
                    encdetails.Skin = Data.Skin;
                    encdetails.Temp = Data.Temp;
                    encdetails.TreatmentPlan = Data.Treatment;
                    encdetails.Adminid = admindata.Adminid;
                    encdetails.ModifiedDate = DateTime.Now;
                    _context.Encounterforms.Update(encdetails);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }


            return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

        }


        public bool CaseFinalized(ViewEncounter model, string id)
        {
            try
            {
                var data = _context.Encounterforms.FirstOrDefault(e => e.Encounterformid == model.EncounterID);
                data.ModifiedDate = DateTime.Now;
                data.Isfinalize = true;
                _context.Encounterforms.Update(data);
                _context.SaveChanges();


                var final = _context.Requests.FirstOrDefault(e => e.Requestid == model.Requesid);
                final.Modifieddate = DateTime.Now;
                final.Status = 6;
                _context.Requests.Update(final);
                _context.SaveChanges();

                var admindata = _context.Admins.FirstOrDefault(e => e.Aspnetuserid == id);
                Requeststatuslog rs = new Requeststatuslog
                {
                    Requestid = final.Requestid,
                    Status = 6,
                    Createddate = DateTime.Now,
                    Adminid = admindata.Adminid


                };
                _context.Requeststatuslogs.Add(rs);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        #endregion

    }
}
