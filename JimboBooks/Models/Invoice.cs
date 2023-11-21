using System;

namespace JimboBooks.Models;

public class Invoice
{
    public int Id { get; set; }
    
    public string customerName { get; set; }
    
    public string customerEmail { get; set; }
    
    public string shortDesc { get; set; }
    
    public DateTime dueDate { get; set; }
    
    public string invoiceName { get; set; }
    
    public decimal amount { get; set; }

    public string paymentLink { get; set; }

}