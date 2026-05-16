using System;
using System.Linq;
class TestStripeProps {
    static void Main() {
        Console.WriteLine("Subscription props:");
        foreach (var p in typeof(Stripe.Subscription).GetProperties()) Console.WriteLine(" - " + p.Name);
        Console.WriteLine("Invoice props:");
        foreach (var p in typeof(Stripe.Invoice).GetProperties()) Console.WriteLine(" - " + p.Name);
    }
}
