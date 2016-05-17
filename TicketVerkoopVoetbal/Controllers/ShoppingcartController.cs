using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Ticket.Model;
using Ticket.Service;
using TicketVerkoopVoetbal.Models;

namespace TicketVerkoopVoetbal.Controllers
{
    public class ShoppingcartController : Controller
    {
        // GET: Shoppingcart
        public ActionResult Index()
        {

            ShoppingCartViewModel cartListAll = (ShoppingCartViewModel)Session["shoppingCart"];
            //TODO error fixxen 
            AbonnementViewModel m = new AbonnementViewModel();
            
            if (cartListAll != null)
            {
                if (cartListAll.abbo !=  null)
                {
                    m = cartListAll.abbo;
                    ViewBag.Ploeg = m.Club;
                    ViewBag.Plaats = m.Plaats;
                    ViewBag.Prijs = m.Prijs;
                }
            }
            return View(cartListAll);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ShoppingCartViewModel cartList = (ShoppingCartViewModel)Session["shoppingCart"];

            var itemToRemove = cartList.Cart.FirstOrDefault(r => r.WedstrijdId == id);
            if (itemToRemove != null)
            {
                cartList.Cart.Remove(itemToRemove);
                Session["shoppingCart"] = cartList;
            }


            return View("index", cartList);
        }

        public ActionResult DeleteAbbo()
        {

            ShoppingCartViewModel cartList = (ShoppingCartViewModel)Session["shoppingCart"];
            cartList.abbo = null;
            Session["ShoppingCart"] = cartList;
            return RedirectToAction("index", "ShoppingCart");
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Payment()
        {
            UserService userService = new UserService();
            var user = userService.Get(User.Identity.Name);
            int maxTickets = Convert.ToInt32(ConfigurationManager.AppSettings["maxTicket"]);


            //try
            //{
            ShoppingCartViewModel shopping = (ShoppingCartViewModel)Session["ShoppingCart"];
            CartViewModel cart = new CartViewModel();
            WedstrijdService wedstrijdService = new WedstrijdService();
            PlaatsService plaatsService = new PlaatsService();
            StadionService stadionService = new StadionService();
            TicketService ticketService = new TicketService();
            List<int> tickets = new List<int>();
            Tickets ticket = new Tickets();

            for (int i = 0; i < shopping.Cart.Count; i++)
            {
                cart = shopping.Cart[i];
                int ticketsBeschikbaar = plaatsService.GetPlaats(cart.Plaats).aantal - ticketService.getTicketsPerWedstrijdPerVak(cart.WedstrijdId, cart.Plaats);
                int countTicket = ticketService.GetTicketsPerPersoonPerWedstrijd(user.Id, cart.WedstrijdId).Count();

                if (ticketsBeschikbaar >= cart.Aantal)
                {
                    if ((countTicket + cart.Aantal) <= maxTickets)
                    {
                        for (int j = 0; j < cart.Aantal; j++)
                        {
                            ticket.Persoonid = user.Id;
                            ticket.Wedstrijdid = cart.WedstrijdId;
                            ticket.plaatsId = cart.Plaats;
                            ticket.Betaald = true;

                            Boolean unique = false;
                            long barcode = -1;

                            while (!unique)
                            {
                                Random r = new Random();
                                int bar = r.Next(100000000, 999999999);

                                barcode = Convert.ToInt64(bar.ToString() + ticket.Wedstrijdid.ToString() + ticket.plaatsId.ToString());

                                if (ticketService.ZoekTicketBarcode(barcode) == 0) unique = true;
                            }

                            ticket.barcode = barcode;

                            // Ticket toevoegen aan db
                            ticketService.Add(ticket);

                            // Ticket ID in lijst stoppen zodanig deze later terug opgehaald kan worden om aan mail toe te voegen
                            tickets.Add(ticket.id);
                        }
                    }
                    else
                    {
                        TempData["Tickets"] = countTicket;
                        TempData["Thuis"] = cart.ThuisPloegNaam;
                        TempData["Bezoek"] = cart.BezoekersPloegNaam;
                        return RedirectToAction("index", "ShoppingCart");
                    }
                }
                else
                {


                    TempData["Tickets2"] = countTicket;
                    TempData["Thuis"] = cart.ThuisPloegNaam;
                    TempData["Bezoek"] = cart.BezoekersPloegNaam;
                    return RedirectToAction("index", "ShoppingCart");
                }
            }

            await SendPdfTickets(user, tickets);

            //}
            //catch (Exception ex)
            //{
            //    return RedirectToAction("Index", "Error");
            //}

            return View();
        }

        private async Task<ActionResult> SendPdfTickets(AspNetUsers user, List<int> tickets)
        {
            await SendTicketMail(user, CreateTicketsPdf(tickets));

            return null;
        }

        private Byte[] CreateTicketsPdf(List<int> tickets)
        {
            MemoryStream memoryStream = new MemoryStream();
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            pdfDoc.Open();

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    for (int i = 0; i < tickets.Count; i++)
                    {
                        TicketService ticketService = new TicketService();
                        Tickets ticket = ticketService.GetById(tickets[i]);
                        UserService userService = new UserService();
                        AspNetUsers user = userService.GetUser(ticket.Persoonid);
                        WedstrijdService wedstrijdService = new WedstrijdService();
                        Wedstrijd wedstrijd = wedstrijdService.Get(ticket.Wedstrijdid);
                        ClubService clubService = new ClubService();
                        Clubs clubThuis = clubService.Get(wedstrijd.thuisPloeg);
                        Clubs clubBez = clubService.Get(wedstrijd.bezoekersPloeg);
                        StadionService stadionService = new StadionService();
                        Stadion stadion = stadionService.Get(wedstrijd.stadionId);
                        VakService vakService = new VakService();
                        Vak vak = vakService.getVak(ticket.plaatsId);

                        //Create a new StringBuilder object
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("<!DOCTYPE html><html><head><title></title><meta charset=\"utf-8\" />");
                        sb.AppendLine("<style>* {margin: 0;font-family: Arial;}");
                        sb.AppendLine("</style></head><body>");
                        sb.AppendLine("<div style=\"margin: 20px;\">");
                        sb.AppendLine("<h2>VoetbalTicketVerkoop.be</h2>");
                        sb.AppendLine("<h4 style=\"font-weight: bold\">Dit is uw ticket</h4>");
                        sb.AppendLine("<table width=\"100%\" cellspacing='0' cellpadding='2'>");
                        sb.AppendLine("<tr><th>" + user.FirstName + " " + user.Name + "</th><th></th><th>Datum van wedstrijd</th>");
                        sb.AppendLine("</tr><tr><th><br /><br /></th></tr>");
                        //sb.AppendLine("<tr><th><img src=\"~/" + clubThuis.logo + "\" style=\"max-height: 200px; max-width: 200px\" /></th>");
                        //sb.AppendLine("<th><h3>  vs  </h3></th>");
                        //sb.AppendLine("<th><img src=\"~/" + clubBez.logo + "\" style=\"max-height: 200px; max-width: 200px\" /></th></tr>");
                        sb.AppendLine("<tr><td style=\"text-align:center\">" + clubThuis.naam + "</td><td></td>");
                        sb.AppendLine("<td style=\"text-align:center\">" + clubBez.naam + "</td></tr>");
                        sb.AppendLine("<tr><th><br /><br /></th></tr>");
                        sb.AppendLine("<tr><th>Aanvang wedstrijd:");
                        sb.AppendLine("</th><th></th><th>Plaats:</th></tr>");
                        sb.AppendLine("<tr><td style=\"text-align:center\">" + wedstrijd.Date + "</td><td></td>");
                        sb.AppendLine("<td style=\"text-align:center\">" + stadion.naam + "</td> </tr>");
                        sb.AppendLine("<tr><td></td><td></td><td style=\"text-align:center\">" + stadion.adres + "</td></tr>");
                        sb.AppendLine("<tr><td>Vaknaam:</td></tr>");
                        sb.AppendLine("<tr><th>" + vak.naam + "</th></tr>");
                        sb.AppendLine("</table>");
                        sb.AppendLine("<p STYLE=\"text-align: justify;\">");
                        sb.AppendLine("Beste <br /> <br />Dit is het ticket die recht geeft tot u toegang zal geven tot het stadion en het door u gekozen vak.");
                        sb.AppendLine("Het ticket is enkel geldig voor de speel datum en tijdstip dat hierboven vermeld staat en in het stadion dat hierboven vermeld staat.");
                        sb.AppendLine("Het is niet toegelaten om het ticket door te verkopen. <br />");
                        sb.AppendLine("Tickets zijn ook enkel geldig wanneer deze bij voetbaltickets.be zijn aangekocht. ");
                        sb.AppendLine("Wanneer u deze van een andere persoon of website hebt gekocht kunnen wij geen garantie geven dat u toegang zult krijgen tot het stadion.");
                        sb.AppendLine("Wanneer enige twijfel mogelijk is heeft het stadion ook altijd het recht u de toegang te ontzeggen.<br />");
                        sb.AppendLine("Tickets kunnen ook slechts 1 keer gescanned worden. Wanneer een ticket meerdere keren gescanned wordt zal slechts het eerste toegelaten worden tot het stadion.<br /> <br />");
                        sb.AppendLine("Indien u wenst uw ticket te annuleren kan u dit doen tot 1 week voor de wedstrijd.");
                        sb.AppendLine("<br /><br />");
                        sb.AppendLine("Zorg ervoor dat onderstaande barcode goed leesbaar is.");
                        sb.AppendLine("<br /><br />");
                        //sb.AppendLine("<img src=\"~/images/shared/barcodeTemplate.png\" />");
                        sb.AppendLine("</p></div></body></html>");

                        StringReader sr = new StringReader(sb.ToString());

                        List<iTextSharp.text.IElement> elements = HTMLWorker.ParseToList(sr, null);
                        

                        pdfDoc.NewPage();

                        foreach (object item in elements)
                        {
                            pdfDoc.Add((IElement)item);
                        }



                        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);


                        //using (memoryStream)
                        //{
                        //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        //    htmlparser.Parse(sr);

                        //    //MailMessage mm = new MailMessage("sender@gmail.com", "receiver@gmail.com");
                        //    //mm.Subject = "iTextSharp PDF";
                        //    //mm.Body = "iTextSharp PDF Attachment";
                        //    //mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "iTextSharpPDF.pdf"));
                        //    //mm.IsBodyHtml = true;
                        //    //SmtpClient smtp = new SmtpClient();
                        //    //smtp.Host = "smtp.gmail.com";
                        //    //smtp.EnableSsl = true;
                        //    //NetworkCredential NetworkCred = new NetworkCredential();
                        //    //NetworkCred.UserName = "sender@gmail.com";
                        //    //NetworkCred.Password = "<password>";
                        //    //smtp.UseDefaultCredentials = true;
                        //    //smtp.Credentials = NetworkCred;
                        //    //smtp.Port = 587;
                        //    //smtp.Send(mm);
                        //}

                        //if (i + 1 < tickets.Count) pdfDoc.NewPage();
                    }
                }
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                memoryStream.Position = 0;

                return bytes;
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendTicketMail(AspNetUsers user, Byte[] bytes)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(user.Email));
            message.Attachments.Add(new Attachment(new MemoryStream(bytes), "eTicket.pdf"));
            message.Subject = "VoetbalTickets - Bestelling";
            message.Body = "</p>Beste " + user.FirstName+ System.Environment.NewLine + "</p><p>Wij hebben uw bestelling goed ontvangen.</p>";
            message.Body += "<p>In bijlage vindt u de PDF met de door u bestelde tickets, gelieve deze in kleur of zwart-wit af te drukken en deze mee te brengen naar de wedstrijd.</p>";
            message.Body += "<p>Dit geeft recht om binnen te mogen in het stadion in het door u gekozen vak. Zorg er ook voor dat de barcode goed leesbaar is.</p><br />";
            message.Body += "<p>Met vriendelijke groet</p>" + System.Environment.NewLine + "<p>Het VoetbalTickets-team</p>";
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
                return null;
            }
        }
    }
}