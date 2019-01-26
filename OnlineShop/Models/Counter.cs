using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Counter : IEntit
    {
        public int counter { get; set; }

        Counter()
        {
            counter = 0;
        }


        public static void createCounterInDatabase()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                db.counters.First();
            }
            catch(InvalidOperationException)
            {
                Counter counter = new Counter();
                db.counters.Add(counter);
                db.SaveChanges();
            }
            
        }

        public static void increaseCounter()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Counter counter = db.counters.First();

            counter.counter = counter.counter + 1;
            db.Entry(counter).State = EntityState.Modified;
            db.SaveChanges();

        }
    }
}