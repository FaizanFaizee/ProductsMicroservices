using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsMicroservices.Models;
using ProductsMicroservices.Interfaces;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using ProductsMicroservices.ProductDbContext;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProductsMicroservices.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductContext _context;

        public ProductController(IProductRepository productRepository)
        {

            _productRepository = productRepository;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetProductsAsync();
            return View(products); // Assuming your view expects a list of products

        }

        public async Task<IActionResult> Custom_page()
        {
            
            return View(); // Assuming your view expects a list of products

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByIDAsync(id.Value); // Assuming GetProductByID takes an int
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // This indicates the method handles POST requests from a form
        public async Task<IActionResult> Create()
        {
           return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Description, Price, CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.InsertProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product); // Pass the product back to the View for error handling
        }




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByIDAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description, Price, CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product); // Assuming context is for Product entity
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ProductExists(id) == null) // Check for product existence using id
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-throw the exception for handling in the View
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(product); // Return the Edit view with the product for error display
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByIDAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetProductByIDAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteProductAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task<bool> ProductExists(int id)
        {
            return await _productRepository.GetProductByIDAsync(id) != null;
        }

    }
}
