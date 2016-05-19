using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
                if (cartListAll.abbo != null)
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

            if (cartList.abbo == null)
            {
                if(cartList.Cart == null || cartList.Cart.Count == 0)
                {
                    Session["shoppingCart"] = null;
                    cartList = null;
                }
            }

            return RedirectToAction("index", "ShoppingCart");
        }

        public ActionResult DeleteAbbo()
        {

            ShoppingCartViewModel cartList = (ShoppingCartViewModel)Session["shoppingCart"];
            cartList.abbo = null;

            if (cartList.abbo == null)
            {
                if (cartList.Cart == null || cartList.Cart.Count == 0)
                {
                    Session["shoppingCart"] = null;
                    cartList = null;
                }
            }

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
            if (shopping.Cart != null)
            {
                for (int i = 0; i < shopping.Cart.Count; i++)
                {
                    cart = shopping.Cart[i];
                    Delete(cart.WedstrijdId);
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
            }
            if (shopping.abbo != null)
            {
                // er is een abonnement in de session
                AbonnementViewModel b =  shopping.abbo;
                AbonnementService obboserv = new AbonnementService();
                Abonnement bz = new Abonnement();
                bz.PlaatsId = b.PlaatsId;
                bz.Persoonid = user.Id;
                bz.Seizoenid = b.Seizoenid;
                bz.Clubsid = b.ClubsId;
                obboserv.Add(bz);
                await SendPdfTickets(user, b);
            }
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
        private async Task<ActionResult> SendPdfTickets(AspNetUsers user, AbonnementViewModel b)
        {
            await SendTicketMail(user, CreateAbboPdf(b));
            DeleteAbbo();
            return null;
        }


        private Byte[] CreateAbboPdf(AbonnementViewModel abboView)
        {
            MemoryStream memoryStream = new MemoryStream();
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            StringReader sr = new StringReader("");
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            Byte[] bytes = { 0 };

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    using (memoryStream = new MemoryStream())
                    {
                        using (PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream))
                        {
                            pdfDoc.Open();

                            pdfDoc.NewPage();
                            UserService userService = new UserService();
                            WedstrijdService wedstrijdService = new WedstrijdService();
                            ClubService clubService = new ClubService();
                            StadionService stadionService = new StadionService();
                            VakService vakService = new VakService();
                            SeizoenService seizoen = new SeizoenService();
                            Seizoen sez = seizoen.Get(abboView.Seizoenid);

                            AspNetUsers user = null;
                            Wedstrijd wedstrijd = null;
                            Clubs clubThuis = null;
                            
                            Stadion stadion = null;
                            Vak vak = null;


                            var userid = User.Identity.GetUserId();
                            user = userService.GetUser(userid);
                            clubThuis = clubService.Get(abboView.ClubsId);
                            //stadion = stadionService.Get(wedstrijd.stadionId);
                            vak = vakService.getVak(abboView.PlaatsId);

                            StringBuilder sb = new StringBuilder();

                            sb.AppendLine("<!DOCTYPE html><html><head><title></title><meta charset=\"utf-8\" />");
                            sb.AppendLine("</head><body style=\"padding: 10px;font-family: Arial;\">");
                            sb.AppendLine("<div style=\"padding: 10px; margin:auto; text-align:center;\"><h2>Aankoop Abonnement</h2>	</div>");
                            sb.AppendLine("<div style=\"padding: 10px;\" margin-left: 30px;>");
                            sb.AppendLine("<p>Beste "+ user.FirstName+" "+ user.Name +"");
                            sb.AppendLine("</br></br>");
                            sb.AppendLine("Hierbij ontvangt u het betalingsbewijs van uw abonnement.</br>");
                            sb.AppendLine("Met dit bewijs kan u aan het loket van het stadion een fysiek exemplaar van uw abonnement afhalen.");
                            sb.AppendLine("</br>");
                            sb.AppendLine("</br>");
                            sb.AppendLine("Controleer ook nog eens volgende gegevens:");
                            sb.AppendLine("</br>");
                            sb.AppendLine("Club :"+ clubThuis.naam+ " </br>");
                            sb.AppendLine("Vak : "+ vak.naam+" </br>");
                            sb.AppendLine("Voor het seizoen : " + sez.SeizoenString + " </br>");
                            sb.AppendLine("</br>");
                            sb.AppendLine("Vergeet niet dat u aan het loket "+abboView.Prijs +" euro zal moeten betalen.</br></br>");
                            sb.AppendLine("Met vriendelijke groeten");
                            sb.AppendLine("</br>");
                            sb.AppendLine("Tickerverkoop");
                            sb.AppendLine("</p></div>");
                            sb.AppendLine("</body>");

                            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(sb.ToString()), null);

                            //add the collection to the document
                            for (int k = 0; k < htmlarraylist.Count; k++)
                            {
                                pdfDoc.Add((IElement)htmlarraylist[k]);
                            }

                            pdfDoc.Close();
                            bytes = memoryStream.ToArray();
                            memoryStream.Close();
                        }
                    }
                }


                return bytes;
            }
        }

        [Authorize]
        private Byte[] CreateTicketsPdf(List<int> tickets)
        {
            //controle van eerste ticket of user die methode probeerdt te bereiken wel eigenaar is van ticket
            TicketService ticketService = new TicketService();
            string ticketId = ticketService.GetById(tickets[0]).Persoonid;

            if (ticketId.Equals(User.Identity.GetUserId()))
            { 

            MemoryStream memoryStream = new MemoryStream();
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            StringReader sr = new StringReader("");
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            Byte[] bytes = { 0 };

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        using (memoryStream = new MemoryStream())
                        {
                            using (PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream))
                            {
                                pdfDoc.Open();

                                UserService userService = new UserService();
                                WedstrijdService wedstrijdService = new WedstrijdService();
                                ClubService clubService = new ClubService();
                                StadionService stadionService = new StadionService();
                                VakService vakService = new VakService();

                                AspNetUsers user = null;
                                Wedstrijd wedstrijd = null;
                                Clubs clubThuis = null;
                                Clubs clubBez = null;
                                Stadion stadion = null;
                                Vak vak = null;

                                for (int i = 0; i < tickets.Count; i++)
                                {
                                    pdfDoc.NewPage();

                                    Tickets ticket = ticketService.GetById(tickets[i]);
                                    Tickets ticketOld = null;

                                    if (i > 0) ticketOld = ticketService.GetById(tickets[i - 1]);

                                    if ((i == 0) || (ticket.Wedstrijdid != ticketOld.Wedstrijdid) || (ticket.plaatsId != ticketOld.plaatsId))
                                    {
                                        user = userService.GetUser(ticket.Persoonid);
                                        wedstrijd = wedstrijdService.Get(ticket.Wedstrijdid);
                                        clubThuis = clubService.Get(wedstrijd.thuisPloeg);
                                        clubBez = clubService.Get(wedstrijd.bezoekersPloeg);
                                        stadion = stadionService.Get(wedstrijd.stadionId);
                                        vak = vakService.getVak(ticket.plaatsId);
                                    }

                                    //System.Drawing.Image imgT = System.Drawing.Image.FromFile(clubThuis.logo);
                                    //iTextSharp.text.Image imgT = iTextSharp.text.Image.GetInstance(clubThuis.logo);
                                    //imgT.SetAbsolutePosition(100f, 200f);


                                    var imgBar = iTextSharp.text.Image.GetInstance(CreateBarcode(ticket.barcode.ToString()), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //imgThuis.ScaleAbsoluteHeight(200);
                                    //imgBez.ScaleAbsoluteHeight(200);
                                    //imgThuis.SetAbsolutePosition((pdfDoc.PageSize.Width / 4), 200);
                                    //imgBez.SetAbsolutePosition((pdfDoc.PageSize.Width * 3 / 4), 200);

                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("<!DOCTYPE html><html><head><title></title><meta charset=\"utf-8\" />");
                                    sb.AppendLine("</head><body style=\"margin: 0;font-family: Arial;\">");
                                    sb.AppendLine("<div style=\"margin: 20px;\"><div>");
                                    sb.AppendLine("<div style=\"padding: 10px; color:darkgreen; font-weight: bold; background-color: grey;\"><h2>VoetbalTicketVerkoop.be</h2></div>");
                                    sb.AppendLine("<h4 style=\"font-weight: bold\">Dit is uw ticket</h4>");
                                    sb.AppendLine("<p style=\"float: left;\"><b>" + user.FirstName + " " + user.Name + " </b></p>");
                                    sb.AppendLine("<br /><br />");
                                    sb.AppendLine("<h2 style=\"font-weight: bold\">" + clubThuis.naam + "  vs  " + clubBez.naam + "</h2></div>");
                                    sb.AppendLine("<table style=\"width: 100%\" cellspacing='0' cellpadding='2'>");
                                    sb.AppendLine("<tr><th><br /><br /></th></tr>");
                                    sb.AppendLine("<tr><th>Aanvang wedstrijd: " + wedstrijd.Date);
                                    sb.AppendLine("</th><td style=\"text-align:center\"></td></tr><tr><td sty:e=\"font-weight: bold\">Plaats:</td></tr>");
                                    sb.AppendLine("<tr><td></td>");
                                    sb.AppendLine("<td >" + stadion.naam + "</td> </tr>");
                                    sb.AppendLine("<tr><td></td><td>" + stadion.adres + "</td><td></td></tr>");
                                    sb.AppendLine("<tr></tr><tr><td style=\"font-weight: bold\">Vaknaam: " + vak.naam + "</td></tr>");
                                    sb.AppendLine("<tr><th></th></tr></table>");
                                    sb.AppendLine("<br />");
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
                                    sb.AppendLine("</p></div></body></html>");

                                    List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(sb.ToString()), null);

                                    //add the collection to the document
                                    for (int k = 0; k < htmlarraylist.Count; k++)
                                    {
                                        pdfDoc.Add((IElement)htmlarraylist[k]);
                                    }

                                    pdfDoc.Add(imgBar);

                                }

                                pdfDoc.Close();
                                bytes = memoryStream.ToArray();
                                memoryStream.Close();
                            }
                        }
                    }
                }

                return bytes;

            }
            else
            {
                return null;
            }


        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendTicketMail(AspNetUsers user, Byte[] bytes)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(user.Email));
            message.Attachments.Add(new Attachment(new MemoryStream(bytes), "eTicket.pdf"));
            message.Subject = "VoetbalTickets - Bestelling";
            message.Body = "</p>Beste " + user.FirstName + System.Environment.NewLine + "</p><p>Wij hebben uw bestelling goed ontvangen.</p>";
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

        private static System.Drawing.Image CreateBarcode(string code)
        {
            // Doormiddel van iTextSharp
            Barcode39 barcode = new Barcode39();
            barcode.Code = code;
            barcode.ChecksumText = true;
            barcode.GenerateChecksum = true;
            barcode.StartStopText = true;
            return barcode.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
        }

        public ActionResult Download(int ticketsId)
        {
            TicketService ticketService = new TicketService();
            List<int> ticketList = new List<int>();
            ticketList.Add(ticketsId);
            var ticket = ticketService.GetById(ticketsId);

            if (ticket.Persoonid.Equals(User.Identity.GetUserId()))
            {
                Byte[] pdfByte = CreateTicketsPdf(ticketList);

                Response.Clear();
                MemoryStream ms = new MemoryStream(pdfByte);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-dispotition", "attachement;filename=Tickets.pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
                return new FileStreamResult(ms, "ticket/pdf");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult DeleteTicket(int ticketsId)
        {
            TicketService ticketService = new TicketService();
            var ticket = ticketService.GetById(ticketsId);

            if (ticket.Persoonid.Equals(User.Identity.GetUserId()))
            {
                WedstrijdService wedstrijdService = new WedstrijdService();
                var wedstrijd = wedstrijdService.Get(ticket.Wedstrijdid);

                if (wedstrijd.Date > DateTime.Now.AddDays(7))
                {
                    ticketService.Delete(ticket);
                }
                else
                {
                    TempData["ticketDelDayPass"] = true;
                }

                return RedirectToAction("View", "Profile");
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}