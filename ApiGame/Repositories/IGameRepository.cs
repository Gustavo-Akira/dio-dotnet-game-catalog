using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGame.Entities;

namespace ApiGame.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> GetGames(int page, int quantity);
        Task<Game> GetGame(Guid id);
        Task<List<Game>> GetGames(string name, string producer);
        Task Insert(Game game);
        Task Update(Game game);
        Task Remove(Guid id);

        void Dispose();

    }
}
