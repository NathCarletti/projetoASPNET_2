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
using System.Diagnostics;

namespace projectASPNET.Controllers
{
    public class ProductsController : ApiController
    {
        private projetoDM106Context db = new projetoDM106Context();

        // GET: api/Products
        // [Authorize(Roles ="ADMIN")]
        public IQueryable<Product> GetProducts()
        {
            Trace.TraceInformation("Nome do usuário: " + User.Identity.Name);
            if (User.IsInRole("USER"))
            {
                Trace.TraceInformation("Usuário com papel USER");
                return db.Products;
            }
            else if (User.IsInRole("ADMIN"))
            {
                Trace.TraceInformation("Usuário com papel ADMIN");
                return db.Products;
            }
            else return null;
        }
        /* public IQueryable<Product> GetProducts()
         {
             return db.Products;
         }*/

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            Trace.TraceInformation("Nome do usuário: " + User.Identity.Name);
            if (User.IsInRole("USER"))
            {
                Trace.TraceInformation("Usuário com papel USER");
                return Ok(product);
            }
            else if (User.IsInRole("ADMIN"))
            {
                Trace.TraceInformation("Usuário com papel ADMIN");
                return Ok(product);
            }
            else return null;
        }

        // PUT: api/Products/5
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}