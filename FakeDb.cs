using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nancy.Linker.Demo
{
    public delegate FakeDb DbFactory();

    public class FakeDb : IDisposable
    {
        public List<Customer> Customers { get; private set; }

        public FakeDb()
        {
            Customers = new List<Customer>
            {
                new Customer()
                {
                    CustomerId = 123,
                    Name = "Demo Customer"
                }
            };
        }
        
        public void Dispose()
        {
                
        }

        public async Task SaveAsync() { }
    }

    public class Customer
    {
        public enum PricingLevel
        {
            Base = 1,
            Preferred = 2
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }

        public void ChangePricingLevel(PricingLevel pricingLevel)
        {
            
        }

        
    }

}
