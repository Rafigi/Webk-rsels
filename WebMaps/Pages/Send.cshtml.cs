using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebMaps.Models;
using System.Net.Mail;
using SelectPdf;
using Microsoft.AspNetCore.Hosting;

namespace WebMaps.Pages
{
    public class SendModel : PageModel
    {
        [BindProperty]
        public BrugerInformationer Bruger { get; set; }

        private readonly IHostingEnvironment _hostingEnvironment;

        public SendModel(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSendTo()
        {

            if (ModelState.IsValid)
            {
                await SendMail();
                return CreatePDF();
            }
            else
            {
                return Page();
            }
        }

        private string HTMLBuilder()
        {
            string webRootPath = _hostingEnvironment.WebRootPath + @"/css/PDFStyle.css";

            string th = "<th>";
            //string th1 = "<th style= \"padding-top: 1em; padding-bottom: 1em; text-align: left; background-color: #4CAF50; color: white;\">";
            //string table1 = "<table style = \"margin-top: 3em; position: absolute; margin-left: 30em; width: 45em\">;";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<html> <head>");
            sb.Append("<link rel=\"stylesheet\" type=\"text/css\"  href=\" " + webRootPath + "\"/>");
            sb.Append(@"</head> <body>");
            //Generate brugerinformation
            #region Brugerinformationer
            sb.Append("<form class='Box'>");
            sb.Append("<section class='BoxOne'>");
            sb.Append("<p><label> Navn </label>");
            sb.Append("<input value='" + Bruger.Navn + "'></p>");
            sb.Append("<br/>");
            sb.Append("<p><label> Adresse </label>");
            sb.Append("<input value='" + Bruger.Adresse + "'></p>");
            sb.Append("<br/>");
            sb.Append("<p><label> Postnr/By </label>");
            sb.Append("<input value='" + Bruger.PostBy + "'></p>");
            sb.Append("<br/>");
            sb.Append("<p><label> Afdeling </label>");
            sb.Append("<input value='" + Bruger.Afdeling + "'></p>");
            sb.Append("</section>");

            sb.Append("<section class='BoxOne'>");
            sb.Append("<p><label> Mail </label>");
            sb.Append("<input value='" + Bruger.Mail + "'></p>");
            sb.Append("<br/>");
            sb.Append("<p><label> Bank Regnr </label>");
            sb.Append("<input value='" + Bruger.BankRegnr + "'></p>");
            sb.Append("<br/>");
            sb.Append("<p><label> Bank Kontonr </label>");
            sb.Append("<input value='" + Bruger.BankKontoNr + "'></p>");
            sb.Append("</section>");
            sb.Append("</form>");
            #endregion


            //Generate table
            #region Generate table
            sb.Append("<table class='table'>");
            sb.Append(@"<tr>");
            sb.Append(th);
            sb.Append("Dato");
            sb.Append(@"</th>");
            sb.Append(th);
            sb.Append(@"Start Adresse");
            sb.Append(@"</th>");
            sb.Append(th);
            sb.Append(@"Slut Adresse");
            sb.Append(@"</th>");
            sb.Append(th);
            sb.Append(@"Tur/Retur");
            sb.Append("</th>");
            sb.Append(th);
            sb.Append(@"Gentagelser");
            sb.Append(@"</th>");
            sb.Append(th);
            sb.Append(@"KM");
            sb.Append(@"</th>");
            sb.Append(@"<tr>");
            #endregion

            foreach (var item in WebMaps.Models.BrugerInformationer.KørselListe)
            {
                sb.Append(@"<tr>");
                sb.Append(@"<td>");
                sb.Append(item.Dato.Value.ToShortDateString());
                sb.Append(@"</td>");
                sb.Append(@"<td>");
                sb.Append(item.StartAdresse);
                sb.Append(@"</td>");
                sb.Append(@"<td>");
                sb.Append(item.SlutAdresse);
                sb.Append(@"</td>");
                sb.Append(@"<td>");
                sb.Append(item.TurRetur);
                sb.Append(@"</td>");
                sb.Append(@"<td>");
                sb.Append(item.Gentagelser);
                sb.Append(@"</td>");
                sb.Append(@"<td>");
                sb.Append(item.KM);
                sb.Append(@"</td>");
                sb.Append(@"</tr>");
            }

            sb.Append(@"</table>");
            sb.Append("</body ></html> ");

            return sb.ToString();

        }
        HtmlToPdf converter = new HtmlToPdf();
        private IActionResult CreatePDF()
        {
            PdfDocument doc = converter.ConvertHtmlString(HTMLBuilder());
            doc.DocumentInformation.CreationDate = DateTime.Now;
            var file = doc.Save();
            return File(file, "application/pdf");
        }

        private async Task SendMail()
        {

            PdfDocument doc = converter.ConvertHtmlString(HTMLBuilder());

            MemoryStream pdfStream = new MemoryStream();
            doc.Save(pdfStream);

            pdfStream.Position = 0;
            MailMessage mm = new MailMessage("", "");
            mm.Subject = "Kørsel PDF fra " + Bruger.Navn;
            mm.Body = "Denne mail er sendt fra en hjemmesiden www.illemads.dk/map";
            mm.Attachments.Add(new Attachment(pdfStream, "Kørselsseddel.pdf"));
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.office365.com";
            smtp.EnableSsl = true;
            smtp.Port = 587;
            NetworkCredential NetworkCred = new NetworkCredential();
            smtp.Credentials = NetworkCred;
            NetworkCred.UserName = "";
            NetworkCred.Password = "";

            await smtp.SendMailAsync(mm);
            doc.Close();
        }
    }
}
