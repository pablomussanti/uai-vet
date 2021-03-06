﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Vet.Data;
using Vet.Domain;

namespace Vet.Websde.Controllers
{
    public class PacienteController : Controller
    {
        private VetDbContext db = new VetDbContext();
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(PacienteController));




        //public ActionResult Convertirimagen(int codigomascota)
        //{
        //    var imagenmascota = db.Pacientes.Where(x => x.Id == codigomascota).FirstOrDefault();
        //    return File(imagenmascota.ImagenMascota, "image/jpeg");
        //}

        public ActionResult getimage(int id)
        {
            Paciente paciente = db.Pacientes.Find(id);
            byte[] byteimagen = paciente.ImagenMascota;

            MemoryStream memorystream = new MemoryStream(byteimagen);
            Image image = Image.FromStream(memorystream);

            memorystream = new MemoryStream();
            image.Save(memorystream, ImageFormat.Jpeg);
            memorystream.Position = 0;

            return File(memorystream, "image/jpg");

        }



        // GET: Paciente
        public ActionResult Index()
        {
            var pacientes = db.Pacientes.Include(p => p.Cliente);
            return View(pacientes.ToList());
        }

        // GET: Paciente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // GET: Paciente/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clientes, "Id", "NombreCompleto");
            return View();
        }

        // POST: Paciente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClientId,Genero,Nombre,Raza,TipodeSangre")] Paciente paciente, HttpPostedFileBase ImagenMascota)
        {
            if (ImagenMascota != null && ImagenMascota.ContentLength > 0)
            {
                HttpPostedFileBase fileBase = Request.Files[0];
                WebImage image = new WebImage(fileBase.InputStream);

                paciente.ImagenMascota = image.GetBytes();



                //byte[] imagendata = null;
                //using (var binarypaciente = new BinaryReader(ImagenMascota.InputStream))
                //{
                //    imagendata = binarypaciente.ReadBytes(ImagenMascota.ContentLength);

                //}
                //paciente.ImagenMascota = imagendata;
            }
            if (ModelState.IsValid)
            {
                db.Pacientes.Add(paciente);
                db.SaveChanges();
                log.Info("Creacion de paciente");
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clientes, "Id", "NombreCompleto", paciente.ClientId);
            return View(paciente);
        }

        // GET: Paciente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clientes, "Id", "NombreCompleto", paciente.ClientId);
            return View(paciente);
        }

        // POST: Paciente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClientId,Genero,Nombre,Raza,TipodeSangre")] Paciente paciente, HttpPostedFileBase ImagenMascota)
        {
            if (ImagenMascota != null && ImagenMascota.ContentLength > 0)
            {
                byte[] imagendata = null;
                using (var binarypaciente = new BinaryReader(ImagenMascota.InputStream))
                {
                    imagendata = binarypaciente.ReadBytes(ImagenMascota.ContentLength);

                }
                paciente.ImagenMascota = imagendata;
            }
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                log.Info("Edicion de paciente");
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clientes, "Id", "NombreCompleto", paciente.ClientId);
            return View(paciente);
        }

        // GET: Paciente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // POST: Paciente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paciente paciente = db.Pacientes.Find(id);
            db.Pacientes.Remove(paciente);
            db.SaveChanges();
            log.Info("Eliminacion de paciente");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
