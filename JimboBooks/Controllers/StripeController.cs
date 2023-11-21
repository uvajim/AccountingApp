using System;
using System.Collections.Generic;
using Stripe;
using JimboBooks.Models;
using Invoice = JimboBooks.Models.Invoice;

//StripeConfiguration.ApiKey = "sk_test_51OB9CIIAE4T4aIfkLpvR3p4RoPT2F8bZ37kQ3ls8jRNHzB5ra3zWWptoMEJVGf5C9dJzSjyotfstGSHFtAlBITJc00VwgYKfPe";


namespace JimboBooks.Controllers
{
    public class StripeController
    {



        private string customerID;
        private string productID;
        private string invoiceID = Guid.NewGuid().ToString();
        private string priceID;

        private void createProduct(string productName)
        {
            var options = new ProductCreateOptions { Name = productName };
            var service = new ProductService();
            Product result = service.Create(options);
            productID = result.Id;

        }

        private void createPrice(decimal amount)
        {
            int convertedAmount = (int)(amount * 100);
            
            var options = new PriceCreateOptions
            {
                Product = productID,
                UnitAmount = convertedAmount,
                Currency = "usd",
            };
            var service = new PriceService();
            Price result = service.Create(options);
            priceID = result.Id;
        }

        private string createPaymentLink()
        {
            var options = new PaymentLinkCreateOptions
            {
                LineItems = new List<PaymentLinkLineItemOptions>
                {
                    new PaymentLinkLineItemOptions { Price = priceID, Quantity = 1 },
                },
            };
            var service = new PaymentLinkService();
            PaymentLink paymentLink = service.Create(options);
            return paymentLink.Url;
        }

        public string finalizePaymentLink(Invoice currInvoice)
        {
            createProduct(currInvoice.invoiceName);
            createPrice(currInvoice.amount);
            return createPaymentLink();
        }

        
        
        
        
        
        
        private void createCustomer(string name, string email, string? shortDesc = "")
        {
            var options = new CustomerCreateOptions
            {
                Name = name,
                Email = email,
                Description = shortDesc,

            };
            var service = new CustomerService();
            Customer result = service.Create(options);
            customerID = result.Id;
        }

        private void createInvoice()
        {
            var options = new InvoiceItemCreateOptions
            {
                Customer = customerID,
                Price = priceID
            };
            var service = new InvoiceItemService();
            InvoiceItem invoice = service.Create(options);
            invoiceID = invoice.Id;

        }

        private void sendInvoice()
        {
            var service = new InvoiceService();
            service.SendInvoice(invoiceID);
        }

        public void publishInvoice(Invoice currInvoice)
        {
            createProduct(currInvoice.invoiceName);
            createPrice(currInvoice.amount);
            createCustomer(currInvoice.customerEmail, currInvoice.customerEmail, shortDesc: currInvoice?.shortDesc);
            createInvoice();
            sendInvoice();
            
        }

    }
}