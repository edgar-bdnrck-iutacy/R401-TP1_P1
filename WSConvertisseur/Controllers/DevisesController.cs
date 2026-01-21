using Microsoft.AspNetCore.Mvc;
using WSConvertisseur.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WSConvertisseur.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevisesController : ControllerBase
    {
        private List<Devise> devises;
        public DevisesController()
        {
            devises = new List<Devise>
            {
                new Devise(1, "Dollar", 1.08),
                new Devise(2, "Franc Suisse", 1.07),
                new Devise(3, "Yen", 120)
            };
        }

        /// <summary>
        /// Get all currencies.
        /// </summary>
        /// <remarks>
        /// Returns the complete list of available currencies.
        /// </remarks>
        /// <returns>List of currencies</returns>
        /// <response code="200">Returns the list of currencies</response>
        // GET: api/<DevisesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Devise> GetAll()
        {
            return devises;
        }

        /// <summary>
        /// Get a single currency.
        /// </summary>
        /// <remarks>
        /// Returns a currency based on its identifier.
        /// </remarks>
        /// <param name="id">The id of the currency</param>
        /// <returns>The requested currency</returns>
        /// <response code="200">When the currency id is found</response>
        /// <response code="404">When the currency id is not found</response>
        // GET api/<DevisesController>/5
        [HttpGet("{id}", Name = "GetDevise")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Devise))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Devise> GetById(int id)
        {
            Devise? devise = devises.FirstOrDefault(d => d.Id == id);

            if (devise == null)
            {
                return NotFound(); // HTTP 404
            }

            return devise; // HTTP 200 + objet Devise
        }

        /// <summary>
        /// Create a new currency.
        /// </summary>
        /// <remarks>
        /// Adds a new currency to the list.
        /// </remarks>
        /// <param name="devise">The currency to create</param>
        /// <returns>The created currency</returns>
        /// <response code="201">Currency successfully created</response>
        /// <response code="400">Invalid currency data</response>
        // POST api/<DevisesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Devise))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Devise> Post([FromBody] Devise devise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            devises.Add(devise);
            return CreatedAtRoute("GetDevise", new { id = devise.Id }, devise);
        }

        /// <summary>
        /// Update an existing currency.
        /// </summary>
        /// <remarks>
        /// Updates a currency identified by its id.
        /// </remarks>
        /// <param name="id">The id of the currency to update</param>
        /// <param name="devise">The updated currency</param>
        /// <returns>No content</returns>
        /// <response code="204">Currency successfully updated</response>
        /// <response code="400">Invalid data or id mismatch</response>
        /// <response code="404">Currency not found</response>
        // PUT api/<DevisesController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Put(int id, [FromBody] Devise devise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != devise.Id)
            {
                return BadRequest();
            }
            int index = devises.FindIndex((d) => d.Id == id);
            if (index < 0)
            {
                return NotFound();
            }
            devises[index] = devise;
            return NoContent();
        }

        /// <summary>
        /// Delete a currency.
        /// </summary>
        /// <remarks>
        /// Deletes a currency based on its identifier.
        /// </remarks>
        /// <param name="id">The id of the currency to delete</param>
        /// <returns>The deleted currency</returns>
        /// <response code="200">Currency successfully deleted</response>
        /// <response code="404">Currency not found</response>
        // DELETE api/<DevisesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Devise))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Devise> Delete(int id)
        {
            Devise? devise = devises.FirstOrDefault(d => d.Id == id);

            if (devise == null)
            {
                return NotFound(); // HTTP 404
            }

            devises.Remove(devise); // suppression de la devise

            return devise; // HTTP 200 + devise supprimée
                           // ou : return NoContent(); // HTTP 204
        }

    }
}
