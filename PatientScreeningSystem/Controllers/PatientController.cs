using Microsoft.AspNetCore.Mvc;
using PatientScreeningSystem.Data;
using PatientScreeningSystem.Models;
using System;
using System.Linq;

namespace PatientScreeningSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        // Display the registration page
        public IActionResult Register()
        {
            return View();
        }

        // Handle form submission for patient registration
        [HttpPost]
        public IActionResult Register(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.EntryTime = DateTime.Now;
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return RedirectToAction("Success");
            }
            return View(patient);
        }

        public IActionResult Success()
        {
            return View();
        }
        public IActionResult EditSuccessful()
        {
            return View();
        }

        // Success page for deletion
        public IActionResult DeleteSuccess()
        {
            return View();
        }
       
        // Display the edit patient page
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(string patientIdOrName)
        {
            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId.ToString() == patientIdOrName ||
                                     p.Name.ToLower() == patientIdOrName.ToLower());

            if (patient == null)
            {
                // Set the ViewData flag to trigger the toast
                ViewData["PatientNotFound"] = "true";
                ModelState.AddModelError(string.Empty, "Patient not found");
                return View();
            }

            return View("EditDetails", patient);
        }


        [HttpPost]
        public IActionResult Update(Patient patient)
        {
            // Ensure EntryTime is within the valid range
            if (patient.EntryTime < new DateTime(1753, 1, 1))
            {
                patient.EntryTime = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Patients.Update(patient);
                    _context.SaveChanges();
                    return RedirectToAction("EditSuccessful");
                }
                catch (Exception ex)
                {
                    // Log the error (optional)
                    ModelState.AddModelError("", "An error occurred while updating the patient details. Please try again.");
                }
            }

            return View("EditDetails", patient);
        }

        //[HttpPost]
        //public IActionResult RemovePatient(string patient)
        //{
        //    var patient = _context.Patients
        //        .FirstOrDefault(p => p.PatientId.ToString() == patientIdOrName ||
        //                             p.Name.ToLower() == patientIdOrName.ToLower());

        //    if (patient == null)
        //    {
        //        // Set the ViewData flag to trigger the toast
        //        ViewData["PatientNotFound"] = "true";
        //        ModelState.AddModelError(string.Empty, "Patient not found");
        //        return View("Delete");
        //    }

        //    try
        //    {
        //        _context.Patients.Remove(patient);
        //        _context.SaveChanges();
        //        return RedirectToAction("DeleteSuccess");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "An error occurred while deleting the patient.");
        //        return View("Delete");
        //    }
        //}

        [HttpPost]
        public IActionResult RemovePatient(int patientId)
        {
            // Find the patient by PatientId
            var patient = _context.Patients.FirstOrDefault(p => p.PatientId == patientId);

            
            try
            {
                // Remove the patient from the database
                _context.Patients.Remove(patient);
                _context.SaveChanges();
                return RedirectToAction("DeleteSuccess");
            }
            catch (Exception ex)
            {
                // Handle any errors during deletion
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the patient.");
                return View("EditDetails", patient);
            }
        }



    }
}
