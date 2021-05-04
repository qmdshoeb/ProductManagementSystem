using PMS_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace PMS_WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ProductController : ApiController
    {
        PMSEntities db = new PMSEntities();


        [HttpGet]
        [Route("api/GetProducts")]
        public IEnumerable<Product> GetProduct()
        {
            IList<Product> products = db.Products.ToList<Product>();
            List<Product> products1 = new List<Product>();
            foreach (var p in products)
            {
                Product List = new Product()
                {
                    PID = p.PID,
                    PName = p.PName,
                    ImageName = p.ImageName,
                    ImageCode = p.ImageCode,
                    Discount = p.Discount,
                    IsStock = p.IsStock,
                    Quantity = p.Quantity,
                    Price = p.Price
                };

                products1.Add(List);
            }
            return products1;
        }

        [HttpGet]
        [Route("api/GetProduct/{id}")]
        public Product GetProduct(int id)
        {
            Product product = (from p in db.Products
                                        where p.PID == id
                                        select p).FirstOrDefault();
            Product product1 = new Product()
            {
                PID = product.PID,
                PName = product.PName,
                ImageName = product.ImageName,
                ImageCode = product.ImageCode,
                Discount = product.Discount,
                IsStock = product.IsStock,
                Quantity = product.Quantity,
                Price = product.Price
            };
            //byte[] imgData = product.ImageCode;
            //MemoryStream ms = new MemoryStream(imgData);
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(ms);
            //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
            return product1;
        }

        [HttpPost]
        [Route("api/AddProduct")]
        public HttpResponseMessage AddProduct()
        {
            Product product = new Product();
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            product.PName = httpRequest["PName"];
            product.Price = Convert.ToInt32(httpRequest["Price"]);
            product.Discount = Convert.ToInt32(httpRequest["Discount"]);
            product.Quantity = Convert.ToInt32(httpRequest["Quantity"]);

            if (httpRequest["IsStock"] == "true")
                product.IsStock = true;
            else
                product.IsStock = false;

            if (httpRequest.Files.Count > 0)
            {
                //var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var allowedExtensions = new[] {
                  ".jpg", ".png", ".jpg", "jpeg"
                };

                    var postedFile = httpRequest.Files[file];

                    var fileName = Path.GetFileName(postedFile.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(postedFile.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                    {
                        Stream stream = postedFile.InputStream;
                        BinaryReader binaryReader = new BinaryReader(stream);
                        Byte[] bytes = binaryReader.ReadBytes((int)stream.Length);

                        product.ImageName = fileName;
                        product.ImageCode = bytes;
                        db.Products.Add(product);
                        db.SaveChanges();

                    }
                }
                result = Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        [HttpPut]
        [Route("api/UpdateProduct/{id}")]
        public HttpResponseMessage UpdateProduct(int id)
        {
            Product product = new Product();
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            product.PID= id;
            product.PName = httpRequest["PName"];
            product.Discount = Convert.ToInt32(httpRequest["Discount"]);
            product.Price = Convert.ToInt32(httpRequest["Price"]);
            product.Quantity = Convert.ToInt32(httpRequest["Quantity"]);

            if (httpRequest["IsStock"] == "true")
                product.IsStock = true;
            else
                product.IsStock = false;

            if (httpRequest.Files.Count > 0)
            {
                //var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var allowedExtensions = new[] {
                  ".jpg", ".png", ".jpg", "jpeg"
                };

                    var postedFile = httpRequest.Files[file];

                    var fileName = Path.GetFileName(postedFile.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(postedFile.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                    {
                        Stream stream = postedFile.InputStream;
                        BinaryReader binaryReader = new BinaryReader(stream);
                        Byte[] bytes = binaryReader.ReadBytes((int)stream.Length);

                        product.ImageName = fileName;
                        product.ImageCode = bytes;
                        
                        db.Entry(product).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ProductExists(id))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest);
                            }
                            else
                            {
                                throw;
                            }
                        }

                    }
                }
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.PID == id) > 0;
        }




        [HttpGet]
        [Route("api/GetPlacedOrders")]
        public List<Order> GetPlacedOrders()

        {
            List<Order> a = db.Orders.ToList<Order>();
            return a;
        }

        
        [HttpGet]
        [Route("api/GetCart")]
        public IEnumerable<Cart> GetCart()
        {
            IList<Cart> products = db.Carts.ToList<Cart>();
            List<Cart> products1 = new List<Cart>();
            foreach (var p in products)
            {
                Cart List = new Cart()
                {
                    ProductId = p.ProductId,
                    UserId = p.UserId,
                    CartId = p.CartId,
                    
                };

                products1.Add(List);
            }
            return products1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblOrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderId == id) > 0;
        }
    }



}
