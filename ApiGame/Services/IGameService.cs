using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGame.ViewModel;
using ApiGame.InputModel;

namespace ApiGame.Services
{
    public interface IGameService
    {
        Task<List<GameViewModel>> GetGames(int page, int quantity);
        Task<GameViewModel> GetGame(Guid id);
        Task<GameViewModel> Insert(GameInputModel game);
        Task Update(Guid id, GameInputModel game);
        Task Update(Guid id, double price);
        Task Remove(Guid id);
    }
}
