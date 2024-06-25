using Microsoft.AspNetCore.Mvc;
using NameResMVC.Models;

namespace NameResMVC.Controllers
{
    public class NameReservationController : Controller
    {
        // GET: NameReservation
        public IActionResult Index()
        {
            return View(new NameReservation()); // Pass an empty model to the view
        }

        [HttpPost]
        public async Task<IActionResult> CheckAvailability(NameReservation model)
        {
            if (ModelState.IsValid)
            {
                // Call the API to check availability (implementation in step 4)
                bool available = await CheckNameAvailability(model.Name, model.ReservationType);

                if (available)
                {
                    return Json(true); // Return true if available
                }
                else
                {
                    ModelState.AddModelError("", "Name is unavailable."); // Add error message
                }
            }

            // If model is invalid or unavailable, return the view with errors
            return View("Index", model);
        }

        private async Task<bool> CheckNameAvailability(string name, string reservationType)
        {
            using (var client = new HttpClient())
            {
                // Construct the API URL with user input as parameters
                string url = $"https://your-api-endpoint.com/check?name={name}{(reservationType != null ? "&type=" + reservationType : "")}";

                // Make a GET request and handle the response
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content.ToLower() == "true"; // Parse JSON response (adjust based on API format)
                }
                else
                {
                    // Handle API errors
                    ModelState.AddModelError("", "Error checking availability.");
                    return false;
                }
            }
        }

    }


}
