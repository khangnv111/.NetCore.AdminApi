using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.DataAccess;
using AdminApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksContext _context;
        private readonly BooksAccess _bookAccess;

        public BooksController(BooksContext context, BooksAccess booksAccess)
        {
            _context = context;
            _bookAccess = booksAccess;
        }
         
        [HttpGet]
        //[Authorize]
        public ActionResult<IEnumerable<Book>> getBookById(int Id = 0)
        {
            List<Book> list = _context.Book.FromSqlRaw($"SP_GetBooks {Id}").ToList();

            return list;
        }

        [HttpPost]
        public ActionResult<Response> createBooks(Book data)
        {
            try
            {
                var _output = _bookAccess.CreateNewBook(data.BookName, data.Price, data.Category, data.Author);

                return new Response(_output);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}