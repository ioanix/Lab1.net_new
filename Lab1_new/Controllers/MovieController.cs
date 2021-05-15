using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab1_new.Data;
using Lab1_new.Models;
using Lab1_new.ViewModels;
using AutoMapper;

namespace Lab1_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MovieController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //GET: api/Movie/filter/{minYear}
        [HttpGet]
        [Route("filter/{minYear}")]
        public ActionResult<IEnumerable<Movie>> FilterMovies(int minYear)
        {

            return _context.Movies.Where(movie => movie.YearOfRelease >= minYear).ToList();
        }


        // GET: api/Movie
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> FilterMoviesAndOrder(DateTime? fromDate, DateTime? toDate)
        {
            if(fromDate == null|| toDate == null)
            {
                return _context.Movies.ToList();
            }

           var movieList = _context.Movies.Where(movie => movie.DateAdded >= fromDate && movie.DateAdded <= toDate).ToList();

           return movieList.OrderByDescending(movie => movie.YearOfRelease).ToList();
        }


        // GET: api/Movie/5/Comments
        [HttpGet("{id}/Comments")]
        public ActionResult<IEnumerable<MovieWithCommentsViewModel>> GetCommentsForMovie(int id)
        {
            var query = _context.Movies.Where(m => m.Id == id).Include(m => m.CommentsList).Select(m => new MovieWithCommentsViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Rating = m.Rating,
                Watched = m.Watched,
                CommentsList = m.CommentsList.Select(mc => new CommentViewModel
                {
                    Id = mc.Id,
                    Text = mc.Text,
                    Important = mc.Important
                })

            }) ;

            var query_v2 = _context.Movies.Where(m => m.Id == id).Include(m => m.CommentsList).Select(m => _mapper.Map<MovieWithCommentsViewModel>(m));

            return query_v2.ToList();
        }


        // POST: api/Movie/5/Comments
        [HttpPost("{id}/Comments")]
        public IActionResult PostCommentForMovie(int id, Comment comment)
        {
            var movie = _context.Movies.Where(p => p.Id == id).Include(p => p.CommentsList).FirstOrDefault();

            if(movie == null)
            {
                return NotFound();
            }

            movie.CommentsList.Add(comment);
            _context.Entry(movie).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }



        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieViewModel>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieViewModel = _mapper.Map<MovieViewModel>(movie);

            return movieViewModel;
        }

        // PUT: api/Movie/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
