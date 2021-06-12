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
using Microsoft.AspNetCore.Cors;

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


        /// <summary>
        /// Filter movies by minYear
        /// </summary>
        /// <param name="minYear"></param>
        /// <response code="200">Filter movies by date added</response>
        /// <response code="400">Unable to get the movie due to validation error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        //GET: api/Movie/filter/{minYear}
        [HttpGet]
        [Route("filter/{minYear}")]
        public ActionResult<IEnumerable<MovieViewModel>> FilterMovies(int minYear)
        {

            return _context.Movies.Select(movie => _mapper.Map<MovieViewModel>(movie))
                                  .Where(movie => movie.YearOfRelease >= minYear).ToList();
        }

        /// <summary>
        /// Filter movies by date added
        /// </summary>
        /// <response code="200">Filter movies by date added</response>
        /// <response code="400">Unable to get the movie due to validation error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public ActionResult<IEnumerable<MovieViewModel>> FilterMoviesAndOrder(DateTime? fromDate, DateTime? toDate)
        {

            var movieViewModelList = _context.Movies.Select(movie => _mapper.Map<MovieViewModel>(movie)).ToList();

            if(fromDate == null|| toDate == null)
            {
                return movieViewModelList;
            }

           var movieListSorted = movieViewModelList.Where(movie => movie.DateAdded >= fromDate && movie.DateAdded <= toDate).ToList();

           return movieListSorted.OrderByDescending(movie => movie.YearOfRelease).ToList();
        }

        /// <summary>
        /// Get movie with comments
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Get movie with comments</response>
        [ProducesResponseType(StatusCodes.Status200OK)]

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

        /// <summary>
        /// Add a new comment to movie
        /// </summary>
        /// <response code="200">Add a new comment to movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/Comments")]
        public IActionResult PostCommentForMovie(int id, CommentViewModel commentViewModel)
        {
            var comment = _mapper.Map<Comment>(commentViewModel);

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

      

        /// <summary>
        /// Get a movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Get a movie by id</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

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

        /// <summary>
        /// Update a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        /// <response code="204">Update a movie</response>
        /// <response code="400">Unable to update the movie due to validation error</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        // PUT: api/Movie/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieViewModel movieViewModel)
        {

            var movie = _mapper.Map<Movie>(movieViewModel);

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

        /// <summary>
        /// Add a new movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        /// <response code="201">Creates new movie</response>
        /// <response code="400">Unable to create the movie due to validation error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        // POST: api/Movie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieViewModel>> PostMovie(MovieViewModel movieViewModel)
        {
            var movie = _mapper.Map<Movie>(movieViewModel);
            movie.DateAdded = DateTime.Now;

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        /// <summary>
        /// Deelet a movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Delete a movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

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
