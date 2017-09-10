using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using projetoDM106.Models;
using System.Security.Principal;

namespace projetoDM106.Controllers
{
    public class OrdersController : ApiController
    {
        private projetoDM106Context db = new projetoDM106Context();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            if (User.IsInRole("ADMIN"))
                return db.Orders;
            else return null;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (VerifyOrderUser(User, order) == true)
            {

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }
            if (VerifyOrderUser(User, order) == true)
            {
                db.Entry(order).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (VerifyOrderUser(User, order) == true)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            if (VerifyOrderUser(User, order) == true)
            {
                db.Orders.Remove(order);
                db.SaveChanges();
                return Ok(order);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }

        private bool VerifyOrderUser(IPrincipal user, Order order)
        {
            if ((user.Identity.Name.Equals(order.userName)) || (user.IsInRole("ADMIN"))) return true;
            else return false;
        }
    }
}