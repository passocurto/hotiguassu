using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace jQuery_File_Upload.MVC3.Upload
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        private static ImageCodecInfo jpgEncoder;
        private readonly JavaScriptSerializer js;

        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Fotos/")); } //Path should! always end with '/'
        }

        public UploadHandler()
        {
            js = new JavaScriptSerializer();
            js.MaxJsonLength = 41943040;
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Cache-Control", "private, no-cache");

            HandleMethod(context);
        }

        // Handle request based on method
        private void HandleMethod(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "HEAD":
                case "GET":
                    if (GivenFilename(context)) DeliverFile(context);
                    else ListCurrentFiles(context);
                    break;

                case "POST":
                case "PUT":
                    UploadFile(context);
                    break;

                case "DELETE":
                    DeleteFile(context);
                    break;

                case "OPTIONS":
                    ReturnOptions(context);
                    break;

                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        private static void ReturnOptions(HttpContext context)
        {
            context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
            context.Response.StatusCode = 200;
        }

        // Delete file from the server
        private void DeleteFile(HttpContext context)
        {
            var filePath = StorageRoot + (string)HttpContext.Current.Request.Cookies["idGirl"].Value + "\\" + context.Request["f"];
            var fileMinhaturaPath = StorageRoot + (string)HttpContext.Current.Request.Cookies["idGirl"].Value + "\\" + "Minhatura" + "\\" + context.Request["f"];
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                File.Delete(fileMinhaturaPath);
            }
        }

        // Upload file to the server
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses);
            }
            else
            {
                UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

            WriteJsonIframeSafe(context, statuses);
        }

        // Upload partial file
        private void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> statuses)
        {
            if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var inputStream = context.Request.Files[0].InputStream;
            var fullName = StorageRoot + Path.GetFileName(fileName);

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new FilesStatus(new FileInfo(fullName)));
        }

        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                criaPastaGarota();
                var file = context.Request.Files[i];
                var fu = new FileUpload();
                criaPastaMinhaturaGarota();
                ResizeStream(205, file.InputStream, StorageRoot + (string)HttpContext.Current.Request.Cookies["idGirl"].Value + "\\" + "Minhatura" + "\\" + Path.GetFileName(file.FileName));
                var fullPath = StorageRoot + (string)HttpContext.Current.Request.Cookies["idGirl"].Value + "\\" + Path.GetFileName(file.FileName);
                ResizeImage(file.InputStream, fullPath , 500, 80);
                string fullName = Path.GetFileName(file.FileName);
                statuses.Add(new FilesStatus(fullName, file.ContentLength, fullPath));
            }
        }

        public void ResizeStream(int imageSize, Stream filePath, string outputPath)
        {
            var imagem = System.Drawing.Image.FromStream(filePath);
            int thumbnailSize = imageSize;
            int newWidth, newHeight;

            if (imagem.Width > imagem.Height)
            {
                newWidth = thumbnailSize;
                newHeight = imagem.Height * thumbnailSize / imagem.Width;
            }
            else
            {
                newWidth = imagem.Width * thumbnailSize / imagem.Height;
                newHeight = thumbnailSize;
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(imagem, imageRectangle);

            thumbnailBitmap.Save(outputPath, imagem.RawFormat);
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            imagem.Dispose();
        }

        public static void ResizeImage(Stream arquivo, string outFile, double maxDimension, long level)
        {

            using (System.Drawing.Image inImage = System.Drawing.Image.FromStream(arquivo))
            {
                double width;
                double height;

                if (inImage.Height < inImage.Width)
                {
                    width = maxDimension;
                    height = (maxDimension / (double)inImage.Width) * inImage.Height;
                }
                else
                {
                    height = maxDimension;
                    width = (maxDimension / (double)inImage.Height) * inImage.Width;
                }
                using (Bitmap bitmap = new Bitmap((int)width, (int)height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(inImage, 0, 0, bitmap.Width, bitmap.Height);
                        if (inImage.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                        {
                            if (jpgEncoder == null)
                            {
                                ImageCodecInfo[] ici = ImageCodecInfo.GetImageDecoders();
                                foreach (ImageCodecInfo info in ici)
                                {
                                    if (info.FormatID == ImageFormat.Jpeg.Guid)
                                    {
                                        jpgEncoder = info;
                                        break;
                                    }
                                }
                            }
                            if (jpgEncoder != null)
                            {
                                EncoderParameters ep = new EncoderParameters(1);
                                ep.Param[0] = new EncoderParameter(Encoder.Quality, level);
                                bitmap.Save(outFile, jpgEncoder, ep);
                            }
                            else
                                bitmap.Save(outFile, inImage.RawFormat);
                        }
                        else
                        {
                            graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                            bitmap.Save(outFile, inImage.RawFormat);
                        }
                    }
                }
            }
        }

        public static void GetImageSize(string inFile, out int width, out int height)
        {
            using (Stream stream = new FileStream(inFile, FileMode.Open))
            {
                using (System.Drawing.Image src_image = System.Drawing.Image.FromStream(stream))
                {
                    width = src_image.Width;
                    height = src_image.Height;
                }
            }
        }

        private void criaPastaGarota()
        {
            var cookieGarota = (HttpCookie)HttpContext.Current.Request.Cookies["idGirl"];
            if (cookieGarota != null)
            {
                if (!Directory.Exists(StorageRoot + cookieGarota.Value))
                    Directory.CreateDirectory(StorageRoot + cookieGarota.Value);
                ;
            }
        }

        private void criaPastaMinhaturaGarota()
        {
            var cookieGarota = (HttpCookie)HttpContext.Current.Request.Cookies["idGirl"];
            if (cookieGarota != null)
            {
                if (!Directory.Exists(StorageRoot + cookieGarota.Value + "\\" + "Minhatura"))
                    Directory.CreateDirectory(StorageRoot + cookieGarota.Value + "\\" + "Minhatura");
                ;
            }
        }

        private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                    context.Response.ContentType = "application/json";
                else
                    context.Response.ContentType = "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            var jsonObj = js.Serialize(statuses.ToArray());
            context.Response.Write(jsonObj);
        }

        private static bool GivenFilename(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Request["f"]);
        }

        private void DeliverFile(HttpContext context)
        {
            var filename = context.Request["f"];
            var filePath = StorageRoot + filename;

            if (File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        private void ListCurrentFiles(HttpContext context)
        {
            var files =
                new DirectoryInfo(StorageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(f))
                    .ToArray();

            string jsonObj = js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }

    }
}