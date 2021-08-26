using ApiGame.InputModel;
using ApiGame.ViewModel;
using ApiGame.Repositories;
using ApiGame.Exceptions;
using ApiGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiGame.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;

        }
        
        public async Task<GameViewModel> GetGame(Guid id)
        {
            var game = await _gameRepository.GetGame(id);
            if (game == null)
            {
                return null;
            }
            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public Task<List<GameViewModel>> GetGames(int page, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<GameViewModel> Insert(GameInputModel game)
        {
            var entity = await _gameRepository.GetGames(game.Name, game.Producer);
            if (entity.Count > 0)
                throw new GameAlreadyExistsException();

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };

            await _gameRepository.Insert(gameInsert);

            return new GameViewModel
            {
                Id=gameInsert.Id,
                Name= gameInsert.Name,
                Producer= gameInsert.Producer,
                Price = gameInsert.Price
            };
        }

        public async Task Remove(Guid id)
        {
            var game = await _gameRepository.GetGame(id);

            if (game == null)
                throw new GameDontExistsException();

            await _gameRepository.Remove(id);
        }

        public async Task Update(Guid id, GameInputModel game)
        {
            var entity = await _gameRepository.GetGame(id);
            if (entity == null)
                throw new GameDontExistsException();

            entity.Name = game.Name;
            entity.Producer = game.Producer;
            entity.Price = game.Price;

            await _gameRepository.Update(entity);
        }

        public async Task Update(Guid id, double price)
        {
            var entity = await _gameRepository.GetGame(id);

            if (entity == null)
                throw new GameDontExistsException();

            entity.Price = price;

            await _gameRepository.Update(entity);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }
    }
}
