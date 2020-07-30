using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PrintControl.Common
{
    public static class ConfigDefine
    {
        public readonly static string DICOMDIR = "DICOMDIR";
        public readonly static string KOS = "KOS";
        public readonly static string DICOMINFOJSON = "DICOMINFO.JSON";
        public readonly static string KANKE = "KANKE";
        public readonly static string MimeType_Default = "application/octet-stream";
        public readonly static string SECTION = "SECTION";
        public readonly static string SECTIONIDX = "SECTIONIDX";
        public readonly static string AECG = "AECG";

        public static Guid ServiceUID 
        { 
            get 
            {
                return new Guid(ConfigurationManager.AppSettings["ServiceUID"]); 
            }
        }

        public static string TokenKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TokenKey"];
            }
        }

        public static bool VerifyToken
        {
            get
            {
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["VerifyToken"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }                
            }
        }

        public static string AllowUploadFileType
        {
            get
            {
                return ConfigurationManager.AppSettings["AllowUploadFileType"];
            }
        }
        
        public static string GetFileMimeType(string fileExtension)
        {
            return GetFileMimeType(fileExtension, null);
        }
        public static string GetFileMimeType(string fileExtension, string contentType)
        {
            var fileMimeType = ConfigurationManager.AppSettings[fileExtension.ToLower()];
            if (string.IsNullOrEmpty(fileMimeType) && !string.IsNullOrEmpty(contentType))
                fileMimeType = contentType.Split(';')[0];
            if (string.IsNullOrEmpty(fileMimeType))
                fileMimeType = ConfigDefine.MimeType_Default;
            return fileMimeType;
        }

        public static string GetMimeTypeFileExtension(string mimeType)
        {
            return ConfigurationManager.AppSettings[mimeType];
        }

        public static string TransformPPT
        {
            get
            {
                var ret = ConfigurationManager.AppSettings["TransformPPT"];
                if (string.IsNullOrEmpty(ret))
                {
                    return "PDF";
                }
                else
                {
                    return ret.ToUpper();
                }
            }
        }

        public static bool ExamImageShortDir
        {
            get
            {
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["ExamImageShortDir"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DownloadFileName
        {
            get
            {
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["DownloadFileName"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool PdfViewByBrowser
        {
            get
            {
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["PdfViewByBrowser"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool PdfViewByBrowserIELess
        {
            get
            {                
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["PdfViewByBrowserIELess"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool CreateDicomDirFileWhenExist
        {
            get
            {
                bool ret = true;
                if (bool.TryParse(ConfigurationManager.AppSettings["CreateDicomDirFileWhenExist"], out ret))
                {
                    return ret;
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool DICOMQueryNumberResult
        {
            get
            {
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["DICOMQueryNumberResult"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DICOMQueryIgnorePatientIDWhenHasStudyInstanceUID
        {
            get
            {                
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["DICOMQueryIgnorePatientIDWhenHasStudyInstanceUID"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DICOMRetrieveUseFilePath
        {
            get
            {                
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["DICOMRetrieveUseFilePath"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool UserUrlScheme
        {
            get
            {
                bool ret = true;
                if (bool.TryParse(ConfigurationManager.AppSettings["UserUrlScheme"], out ret))
                {
                    return ret;
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool UseUrlHelperAction
        {
            get
            {                
                bool ret = true;
                if (bool.TryParse(ConfigurationManager.AppSettings["UseUrlHelperAction"], out ret))
                {
                    return ret;
                }
                else
                {
                    return true;
                }
            }
        }

        public static string DocumentServiceHostToiCCWebClient
        {
            get
            {                
                return ConfigurationManager.AppSettings["DocumentServiceHostToiCCWebClient"];
            }
        }

        public static bool UploadKOSFileToICCWebClient
        {
            get
            {
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["UploadKOSFileToICCWebClient"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string AWSAccessKeyId
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSAccessKeyId"];
            }
        }

        public static string AWSSecretAccessKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSSecretAccessKey"];
            }
        }

        public static string AliyunOSSAccessKeyId
        {
            get
            {
                return ConfigurationManager.AppSettings["AliyunOSSAccessKeyId"];
            }
        }

        public static string AliyunOSSAccessKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AliyunOSSAccessKey"];
            }
        }

        public static string DocFileExt
        {
            get
            {
                return ConfigurationManager.AppSettings["docFileExt"];
            }
        }

        public static string ImageFileExt
        {
            get
            {
                return ConfigurationManager.AppSettings["imageFileExt"];
            }
        }

        public static string SectionFileExt
        {
            get
            {
                return ConfigurationManager.AppSettings["sectionFileExt"];
            }
        }

        public static string WaveFileExt
        {
            get
            {
                return ConfigurationManager.AppSettings["waveFileExt"];
            }
        }

        public static string VodPlayFileExt
        {
            get
            {
                return ConfigurationManager.AppSettings["vodPlayFileExt"];
            }
        }

        public static string PrintImageReportScaleRatio
        {
            get
            {
                return ConfigurationManager.AppSettings["PrintImageReportScaleRatio"];
            }
        }

        public static decimal Pdf2JpgScaleRatio
        {
            get
            {
                var pdf2JpgScaleRatio = ConfigurationManager.AppSettings["Pdf2JpgScaleRatio"];
                decimal ratioValue = 0;
                if (!string.IsNullOrEmpty(pdf2JpgScaleRatio))
                {
                    if (decimal.TryParse(pdf2JpgScaleRatio, out ratioValue) == false)
                    {
                        ratioValue = 0;
                    }
                }

                return ratioValue;
            }
        }

        public static int Pdf2JpgResolution
        {
            get
            {
                var resolution = ConfigurationManager.AppSettings["Pdf2JpgResolution"];
                int resolutionValue = 300;
                if (!string.IsNullOrEmpty(resolution))
                {
                    if (int.TryParse(resolution, out resolutionValue) == false)
                    {
                        resolutionValue = 300;
                    }
                }

                return resolutionValue;
            }
        }

        public static string IDCASWebApi
        {
            get
            {
                var idcasWebApi = ConfigurationManager.AppSettings["IDCASWebApi"];
                if (!string.IsNullOrEmpty(idcasWebApi))
                {
                    idcasWebApi = idcasWebApi.TrimEnd('/');
                    if (idcasWebApi.EndsWith("v2"))
                    {
                        idcasWebApi = idcasWebApi.Substring(0, idcasWebApi.Length - 2);
                        idcasWebApi = idcasWebApi.TrimEnd('/');
                    }
                }

                return idcasWebApi;
            }
        }

        public static string CRMWebApi
        {
            get
            {
                return ConfigurationManager.AppSettings["CRMWebApi"];
            }
        }

        public static string PathologyWebApi
        {
            get
            {
                return ConfigurationManager.AppSettings["PathologyWebApi"];
            }
        }

        public static bool CreatePathologySlideUploadTask
        {
            get
            {                
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["CreatePathologySlideUploadTask"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool Pdf2JpgView
        {
            get
            {                
                bool ret = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["Pdf2JpgView"], out ret))
                {
                    return ret;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool VideoViewFromClientToCloudStorage
        {
            get
            {                
                bool ret = true;
                if (bool.TryParse(ConfigurationManager.AppSettings["VideoViewFromClientToCloudStorage"], out ret))
                {
                    return ret;
                }
                else
                {
                    return true;
                }
            }
        }

        public static int TempFileDays
        {
            get
            {
                var ret = 3;
                int.TryParse(ConfigurationManager.AppSettings["TempFileDays"], out ret);
                return ret;
            }
        }
    }
}