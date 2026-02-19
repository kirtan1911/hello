using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SelfTask.Models;

namespace SelfTask.Controllers;

public class NewController: Controller
{
    private readonly MasterDbContext _context;

    private readonly IWebHostEnvironment _env;

    public NewController(MasterDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public IActionResult Index()
    {
        // ViewBag.type = HttpContext.Session.GetString("type");

        var id = Request.Cookies.Where(t=>t.Key=="id").FirstOrDefault();
        var name = Request.Cookies.Where(t=>t.Key=="name").FirstOrDefault();
        var email = Request.Cookies.Where(t=>t.Key=="email").FirstOrDefault();
        var contact = Request.Cookies.Where(t=>t.Key=="contact").FirstOrDefault();
        var password = Request.Cookies.Where(t=>t.Key=="password").FirstOrDefault();
        var image = Request.Cookies.Where(t=>t.Key=="image").FirstOrDefault();
        var CreateBy = Request.Cookies.Where(t=>t.Key=="CreateBy").FirstOrDefault();

        if(name.Value == null)
        {
            return RedirectToAction(nameof(Login));
        }
        
        ViewBag.name = name.Value;
        ViewBag.email = email.Value;
        ViewBag.contact = contact.Value;
        ViewBag.password = password.Value;
        ViewBag.type = Request.Cookies["type"];
        ViewBag.image = image.Value;

        var userid = Convert.ToInt32(id.Value);

        if(ViewBag.type == "Admin")
        {
            ViewData["Data"] = _context.Information.ToList();
        }
        else
        {
            ViewData["Data"] = _context.Information.Where(t=>t.id == userid).ToList();
        }

        return View();
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registration(Information info ,IFormFile image)
    {
        try
        {
            var imagepath = _env.WebRootPath;
            var newfolder = Path.Combine(imagepath, "picture");

            if(!Directory.Exists(newfolder))
            {
                Directory.CreateDirectory(newfolder);
            }

            var extension = Path.GetExtension(image.FileName);
            var randomname = Guid.NewGuid().ToString().Replace("-", "");
            var combine = randomname + extension;
            var storedata = Path.Combine(newfolder, combine);

            using(var stream = new FileStream(storedata, FileMode.Create))
            {
                image.CopyTo(stream);
            }

             var id = Request.Cookies.Where(t=>t.Key=="id").FirstOrDefault();
            // Console.WriteLine(id);

             info.CreateBy = Convert.ToInt32(id.Value);

            info.image = combine;
            _context.Information.Add(info);
            _context.SaveChanges();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]

    public IActionResult Edit(Information info, IFormFile image)
    {
        var data = _context.Information.Where(t => t.id == info.id).FirstOrDefault();

        if(data != null)
        {
            try
            {
                var imagepath = _env.WebRootPath;
                var newfolder = Path.Combine(imagepath, "picture");

                if(!Directory.Exists(newfolder))
                {
                    Directory.CreateDirectory(newfolder);
                }

                var file = Path.Combine(newfolder, data.image);

                if(System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            
                var extension = Path.GetExtension(image.FileName);
                var randomname = Guid.NewGuid().ToString().Replace("-", "");
                var combine = randomname + extension;
                var storedata = Path.Combine(newfolder, combine);

                using(var stream = new FileStream(storedata, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                data.image = combine;
                data.name = info.name;
                data.email = info.email;
                data.password = info.password;
                data.contact = info.contact;
                data.type = info.type;

                CookieOptions options = new CookieOptions()
                {
                    Expires = DateTime.Now.AddHours(1),
                };

                Response.Cookies.Append("id", data.id.ToString(), options);
                Response.Cookies.Append("name", data.name, options);
                Response.Cookies.Append("email", data.email, options);
                Response.Cookies.Append("contact", data.contact, options);
                Response.Cookies.Append("password", data.password, options);
                Response.Cookies.Append("type", data.type, options);
                Response.Cookies.Append("image", data.image, options);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var data = _context.Information.Where(t => t.id == id).FirstOrDefault();

        if(data != null)
        {
            var imagepath = _env.WebRootPath;
            var newfolder = Path.Combine(imagepath, "picture");
            var file = Path.Combine(newfolder, data.image);

            if(System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }

            _context.Information.Remove(data);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var data = _context.Information.Where(t => t.email == email && t.password == password).FirstOrDefault();

     
        if(data == null)
        {
            ViewBag.loginmessage = "Invalid email & password";
        }
        else
        {
            HttpContext.Session.SetString("id", data.id.ToString());
            HttpContext.Session.SetString("name", data.name);
            HttpContext.Session.SetString("email", data.email);
            HttpContext.Session.SetString("contact", data.contact);
            HttpContext.Session.SetString("password", data.password);
            HttpContext.Session.SetString("type", data.type);
            HttpContext.Session.SetString("image", data.image);

            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddHours(1),
            };

            Response.Cookies.Append("id",data.id.ToString(), options);
            Response.Cookies.Append("name",data.name, options);
            Response.Cookies.Append("email",data.email, options);
            Response.Cookies.Append("contact",data.contact, options);
            Response.Cookies.Append("password",data.password, options);
            Response.Cookies.Append("type",data.type, options);
            Response.Cookies.Append("image",data.image, options);

            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    public IActionResult AddEmployee()
    {
        return View();
    }

    [HttpPost]

    public IActionResult AddEmployee(Information info ,IFormFile image)
    {
        try
        {
            var imagepath = _env.WebRootPath;
            var newfolder = Path.Combine(imagepath, "picture");

            if(!Directory.Exists(newfolder))
            {
                Directory.CreateDirectory(newfolder);
            }

            var extension = Path.GetExtension(image.FileName);
            var randomname = Guid.NewGuid().ToString().Replace("-", "");
            var combine = randomname + extension;
            var storedata = Path.Combine(newfolder, combine);

            using(var stream = new FileStream(storedata, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddHours(1),
            };

            var id = Request.Cookies.Where(t=>t.Key=="id").FirstOrDefault();

            info.CreateBy = Convert.ToInt32(id.Value);

            Console.WriteLine(info.name);

            info.image = combine;

            _context.Information.Add(info);
            _context.SaveChanges();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Logout()
    {
        // HttpContext.Session.Clear();

        Response.Cookies.Delete("id");
        Response.Cookies.Delete("name");
        Response.Cookies.Delete("email");
        Response.Cookies.Delete("contact");
        Response.Cookies.Delete("password");
        Response.Cookies.Delete("type");
        Response.Cookies.Delete("image");

        return RedirectToAction(nameof(Login));
    }
}