using System.Windows;
using Stripe;
using Application = System.Windows.Application;

namespace JimboBooks
{           
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize Stripe with your secret key
            StripeConfiguration.ApiKey = "sk_test_51OB9CIIAE4T4aIfkLpvR3p4RoPT2F8bZ37kQ3ls8jRNHzB5ra3zWWptoMEJVGf5C9dJzSjyotfstGSHFtAlBITJc00VwgYKfPe";
            
            // Rest of your startup code...
        }
    }
}