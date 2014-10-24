using ReferencingSystem.Application.Main.MVC.Models;
using ReferencingSystem.Core.BF;
using ReferencingSystem.Model;
using ReferencingSystem.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace ReferencingSystem.Application.Main.MVC.Controllers
{
    public class OrderController : Controller
    {
        private RsContext db = new RsContext();

        //public async Task<ActionResult> Index(string token)
        //{
        //    Response.Redirect("Error?" + retMsg);
        //}

        [HttpPost]
        public async Task<ActionResult> Index(SubscriptionPlan sub)
        {
            var plan = db.SubscriptionPlan.Where(x => x.Id == sub.Id).FirstOrDefault();

            NVPAPICaller test = new NVPAPICaller();
            string retMsg = "";
            string token = "";

            if (plan != null)
            {
                string amt = plan.Price.ToString();
                HttpContext.Session["payment_amt"] = amt;
                NVPCodec decoder = new NVPCodec();
                bool ret = test.ShortcutExpressCheckout(amt, ref token, ref retMsg, ref decoder);

                var record = db.PaymentRecords.Create();
                record.TransFee = "n/a";
                record.PNRef = "n/a";
                record.PayerId = "n/a";
                record.Action = "S";
                record.Token = token;
                record.PlanId = plan.Id;
                record.ShippingAddress = "n/a";
                record.Amount = amt;
                record.Completed = false;
                record.Message = "";
                record.TaxAmount = "n/a";
                // record info
                record.CreatedUTC = record.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
                record.CreatedBy = record.ModifiedBy = "System";
                if (ret)
                {
                    record.Status = "0";
                    db.PaymentRecords.Add(record);
                    await db.SaveChangesAsync();

                    HttpContext.Session["token"] = token;
                    return Redirect(retMsg);
                }
                else
                {
                    record.Status = decoder["RESULT"].ToLower();
                    await db.SaveChangesAsync();

                    record.Token = token;
                    return Redirect("~/order/Error?" + retMsg);
                }
            }
            else
            {

                return Redirect("~/order/Error?ErrorCode=AmtMissing");
            }
        }

        // GET: Confirm
        public async Task<ActionResult> Confirm(string PAYERID, string TOKEN)
        {
            NVPAPICaller test = new NVPAPICaller();

            string retMsg = "";
            string payerId = PAYERID;
            NVPCodec decoder = new NVPCodec();
            ShippingAddress shippingAddress = new ShippingAddress()
            {
                Street1 = "",
                Street2 = "",
                City = "",
                State = "",
                postCode = ""
            };
            //string paymentType = "S";
            string token = TOKEN;

            bool ret = test.GetShippingDetails(token, ref payerId, ref shippingAddress, ref retMsg, ref decoder);

            var record = db.PaymentRecords.Where(x => x.Token == token).FirstOrDefault();
            if (record != null)
            {
                record.Action = "G";
                // record info
                record.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
                record.ModifiedBy = "System";

            }
            if (ret)
            {
                string shipping = shippingAddress.Street1 + " " + shippingAddress.Street2 + " " + shippingAddress.City + ", " + shippingAddress.State + " " + shippingAddress.postCode;

                ViewBag.Street1 = shippingAddress.Street1;
                ViewBag.Street2 = shippingAddress.Street2;
                ViewBag.City = shippingAddress.City;
                ViewBag.State = shippingAddress.State;
                ViewBag.PostCode = shippingAddress.postCode;
                if (record != null)
                {
                    record.Status = decoder["RESULT"].ToLower();
                    record.PayerId = PAYERID;
                    record.ShippingAddress = shipping;
                    await db.SaveChangesAsync();
                }
                OrderModel model = new OrderModel()
                {
                    amount = Session["payment_amt"].ToString(),
                    PayerId = PAYERID,
                    shippingAddress = shipping,
                    token = TOKEN
                };
                Session["payerId"] = payerId;
                Session["token"] = token;
                return RedirectToAction("ConfirmPayment", "Order", new { amount = model.amount, PayerId = model.PayerId, token = model.token });
            }
            // If result code is greater than 0 (Zero), the transaction is discarded 
            // by the Payflow server. The reason why the transaction is discarded is 
            // evident by the result code value and therefore, you should look at this 
            // result code and decide if 
            // 1. The customer has given some wrong inputs,
            // 2. It's a fraudulent transaction.
            // 3. There's a problem with your merchant account credentials etc.
            // (This is more likely to be caught in your test scenarios.)
            else
            {
                if (record != null)
                {
                    record.Status = "1";
                    record.Message = retMsg;
                    await db.SaveChangesAsync();
                }
                Response.Redirect("Error?" + retMsg);
            }
            return View();
        }

        public async Task<ActionResult> ConfirmPayment(OrderModel model)
        {
            NVPAPICaller test = new NVPAPICaller();

            string retMsg = "";
            string token = model.token;
            string finalPaymentAmount = model.amount;
            string payerId = model.PayerId;
            NVPCodec decoder = new NVPCodec();

            token = Session["token"].ToString();
            payerId = Session["payerId"].ToString();
            finalPaymentAmount = Session["payment_amt"].ToString();

            var record = db.PaymentRecords.Where(x => x.Token == token).FirstOrDefault();
            if (record != null)
            {
                record.Action = "D";

                // record info
                record.ModifiedUTC = SystemConstants.SecondsSinceEpochUTC();
                record.ModifiedBy = "System";

            }


            bool ret = test.ConfirmPayment(finalPaymentAmount, token, payerId, ref decoder, ref retMsg);
            if (ret)
            {

                // Unique transaction ID of the payment.
                string transactionId = decoder["PPREF"];

                // Returns "instant" if the payment is instant or "echeck" if the payment is delayed.
                string paymentType = decoder["PAYMENTTYPE"];

                // The final amount charged, including any shipping and taxes from your Merchant Profile.
                string amt = decoder["AMT"];

                // PayPal fee amount charged for the transaction    
                string feeAmt = decoder["FEEAMT"];

                // Tax charged on the transaction.    
                string taxAmt = decoder["TAXAMT"];

                // PayPal Manager Transaction ID that is used by PayPal to identify this transaction in PayPal Manager reports.
                string pnref = decoder["PNREF"];


                record.TransId = transactionId;
                record.PNRef = pnref;
                record.TransFee = feeAmt;
                record.TaxAmount = taxAmt;
                record.Status = "0";
                record.Message = decoder["RESPMSG"];
                record.Completed = true;
                await db.SaveChangesAsync();
                string message = "";
                int expiry = 0;
                new BusinessFunctionComponents().CreateUserSubscription(User.Identity.GetUserId(), record.PlanId, record.Id, out message, out expiry);

                return RedirectToAction("Success", new { Id = User.Identity.GetUserId() });

            }
            else
            {
                if (record != null)
                {
                    record.Status = "1";
                    record.Message = retMsg;
                    await db.SaveChangesAsync();
                }
                Response.Redirect("Error?" + retMsg);
            }

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Success(string Id)
        {
            var PaymentRecords = db.UserSubscription.Where(x => x.UserId == Id).Include(x => x.PaymentRecord).FirstOrDefault();
            ViewBag.Message = "";
            ViewBag.ExpiryDate = PaymentRecords.ExpiryUTC;
            return View(PaymentRecords != null ? "Success" : "Error", PaymentRecords.PaymentRecord);
        }
    }

}