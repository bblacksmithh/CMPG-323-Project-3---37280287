using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Data;
using EcoPower_Logistics.Repository;

namespace Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly SuperStoreContext _context;
        private readonly IOrderRepository orderRepository;

        public OrdersController(SuperStoreContext context, IOrderRepository orderRepository)
        {
            _context = context;
            this.orderRepository = orderRepository;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await orderRepository.GetAllOrdersAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderRepository.GetOrderDetailsAsync(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            var customers = await orderRepository.GetAllAsync();

            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,CustomerId,DeliveryAddress")] Order order)
        {
            await orderRepository.AddAsync(order); // Use the repository to add the order
            return RedirectToAction(nameof(Index));

            ViewData["CustomerId"] = new SelectList(await orderRepository.GetAllAsync(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderRepository.GetByIdAsync(id.Value); // Use the repository to get the order by ID

            if (order == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(await orderRepository.GetAllAsync(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,CustomerId,DeliveryAddress")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            try
            {
                await orderRepository.Update(order); // Use the repository to update the order asynchronously
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await orderRepository.ExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
                return RedirectToAction(nameof(Index));
            ViewData["CustomerId"] = new SelectList(await orderRepository.GetAllAsync(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderRepository.GetByIdAsync(id.Value); // Use the repository to get the order by ID

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await orderRepository.DeleteAsync(id); // Use the repository to delete the order
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrderExistsAsync(int id)
        {
            return await orderRepository.ExistsAsync(id);
        }
    }
}
