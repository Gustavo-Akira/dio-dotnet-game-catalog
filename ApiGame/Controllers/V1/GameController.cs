using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApiGame.Services;
using ApiGame.ViewModel;
using ApiGame.InputModel;
using ApiGame.Exceptions;

namespace ApiGame.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }


        /// <summary>
        /// Search All games with pagination
        /// </summary>
        /// <remarks>
        /// It is not possible to return games without pagination
        /// </remarks>
        /// <param name="page">Page</param>
        /// <param name="quantity">Quantity</param>
        /// <response code="200">Return the list with the games</response>
        /// <response code="204">Case when dont have games</response>
        [HttpGet]
        public async Task<ActionResult<List<GameViewModel>>> GetGames([FromQuery, Range(1,int.MaxValue)] int page=1,[FromQuery, Range(1,50)] int quantity=5)
        {
            var games = await _service.GetGames(page, quantity);
            if (games.Count == 0)
            {
                return NoContent();
            }

            return Ok(games);
        }
        /// <summary>
        /// Insert a Game
        /// </summary>
        /// <param name="game">Game data</param>
        /// <response code="200">Game inserted withh success</response>
        /// <response code="422">This game already exists</response>   
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> Insert([FromBody] GameInputModel game)
        {
            try
            {
                var entity = await _service.Insert(game);

                return Ok(entity);
            }
            catch(GameAlreadyExistsException e)
            {
                return UnprocessableEntity(e.Message);
            }
        }
        /// <summary>
        ///  Update a Game
        /// </summary>
        /// <param name="idGame"> Id Game</param>
        /// <param name="gameInput">Game</param>
        /// <response code="200">Game updated with success</response>
        /// <response code="404">Game dont exists with this id</response>
        /// <returns></returns>
        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromBody] GameInputModel gameInput)
        {
            try
            {
                await _service.Update(idGame,gameInput);

                return Ok();
            }
            catch (GameAlreadyExistsException e)
            {
                return NotFound("Game not found");
            }
        }
        /// <summary>
        ///  Update the price of a game
        /// </summary>
        /// <param name="idGame">Id of the game</param>
        /// <param name="price">The new price</param>
        /// <response code="200">Price updated with success</response>
        /// <response code="404">Game not exists with this id</response>
        [HttpPatch("{idGame:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromRoute] double price)
        {
            try
            {
                await _service.Update(idGame, price);
                
                return Ok();
            }
            catch (GameAlreadyExistsException e)
            {
                return NotFound("Game dont exists");
            }
        }
        /// <summary>
        ///  Remove a Game
        /// </summary>
        /// <param name="idGame">Id of the Game to delete</param>
        /// <response code="200"> Game removed with success</response>
        /// <response code="404">Game dont exists with this id</response>
        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> removeGame([FromRoute] Guid idGame)
        {
            try
            {
                await _service.Remove(idGame);

                return Ok();
            }
            catch (GameDontExistsException e)
            {
                return NotFound("Game dont exists");
            }
        }
    }
}
