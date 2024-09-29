using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using PatientScreeningSystem.Data;
using PatientScreeningSystem.Models;
using System;
using System.IO;
using System.Linq;

namespace PatientScreeningSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConverter _converter;  // DinkToPdf converter

        public PatientController(AppDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;  // Injecting the DinkToPdf converter
        }

        // Display the registration page
        public IActionResult Register()
        {
            ViewBag.Departments = new[] { "Cardiology", "Dermatology", "Neurology", "Pediatrics", "Radiology" };
            ViewBag.BloodGroups = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
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

            // Repopulate dropdowns if model state is invalid
            ViewBag.Departments = new[] { "Cardiology", "Dermatology", "Neurology", "Pediatrics", "Radiology" };
            ViewBag.BloodGroups = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
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


        // Action to search for a patient to edit
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


        // Action to view all patients and filter them
        public IActionResult Report(string bloodGroup, string department)
        {
            var patients = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(bloodGroup))
            {
                patients = patients.Where(p => p.BloodGroup == bloodGroup);
            }

            if (!string.IsNullOrEmpty(department))
            {
                patients = patients.Where(p => p.Department == department);
            }

            return View(patients.ToList());
        }

        
      
        public IActionResult ViewPatientDetails(int patientId)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.PatientId == patientId);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

    }
}
