using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AdminHalloDoc.Entities.ViewModel
{
    public static class CM
    {
        #region UploadFile
        public static string UploadProviderDoc(IFormFile UploadFile, int Physicianid, string FileName)
        {
            string upload_path = null;
            if (UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload\\Physician\\" + Physicianid;
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string newfilename = FileName;

                string fileNameWithPath = Path.Combine(path, newfilename);
                upload_path = FilePath.Replace("wwwroot\\Upload\\Physician\\", "/Upload/Physician/") + "/" + newfilename;


                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    UploadFile.CopyTo(stream);
                }


            }

            return upload_path;
        }
        #endregion

        #region UploadFile
        public static string UploadDoc(IFormFile UploadFile, int Requestid)
        {
            string upload_path = null;
            if (UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload\\" + Requestid;
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string newfilename = $"{Path.GetFileNameWithoutExtension(UploadFile.FileName)}-{DateTime.Now.ToString("yyyyMMddhhmmss")}.{Path.GetExtension(UploadFile.FileName).Trim('.')}";

                string fileNameWithPath = Path.Combine(path, newfilename);
                upload_path = FilePath.Replace("wwwroot\\Upload\\", "/Upload/") + "/" + newfilename;


                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    UploadFile.CopyTo(stream);
                }


            }

            return upload_path;
        }
        #endregion

        public static string Encode(this int? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "userId cannot be null");
            }

            byte[] userIdBytes = BitConverter.GetBytes(userId.Value);
            return Convert.ToBase64String(userIdBytes);
        }

        public static int? Decode(this string encodedUserId)
        {
            if (string.IsNullOrEmpty(encodedUserId))
            {
                throw new ArgumentNullException(nameof(encodedUserId), "encodedUserId cannot be null or empty");
            }
            try
            {
                byte[] userIdBytes = Convert.FromBase64String(encodedUserId);
                return BitConverter.ToInt32(userIdBytes, 0);
            }
            catch (FormatException ex)
            {
                return null;
            }
        }

    }
}
