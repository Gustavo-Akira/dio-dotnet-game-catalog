using ApiGame.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiGame.Repositories
{
    public class GameSqlServerRepository : IGameRepository
    {
        private readonly SqlConnection sqlConnection;
        
        public GameSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<Game> GetGame(Guid id)
        {
            Game game = null;

            var command = $"select * from Games from Id='{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sql = new SqlCommand(command, sqlConnection);
            SqlDataReader reader = await sql.ExecuteReaderAsync();

            while (reader.Read())
            {
                game = transformInGame(reader);
            }

            await sqlConnection.CloseAsync();
            return game;
        }

        public async Task<List<Game>> GetGames(int page, int quantity)
        {
            var games = new List<Game>();

            var command = $"select * from Games order by id offset{((page - 1) * quantity)} rows fetch next {quantity}";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            while (reader.Read())
            {
                games.Add(transformInGame(reader));
            }
            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task<List<Game>> GetGames(string name, string producer)
        {
            var games = new List<Game>();

            var command = $"select * from Games where Nome='{name}' and Producer='{producer}'";

            await sqlConnection.OpenAsync();
            SqlCommand sql = new SqlCommand(command, sqlConnection);
            SqlDataReader reader = await sql.ExecuteReaderAsync();

            while (reader.Read())
            {
                games.Add(transformInGame(reader));
            }

            await sqlConnection.CloseAsync();
            return games;
        }

        public async Task Insert(Game game)
        {
            var command = $"insert Games(Id,Name,Producer,Price) values('{game.Id}','{game.Name}','{game.Price.ToString().Replace(",",".")}')";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Remove(Guid id)
        {
            var command = $"delete from Games where Id='{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Update(Game game)
        {
            var command = $"update Games set Name='{game.Name}', Producer='{game.Producer}', Price={game.Price.ToString().Replace(",", ".")} where Id = '{game.Id}";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }

        private Game transformInGame(SqlDataReader reader)
        {
            return new Game
            {
                Id = (Guid)reader["Id"],
                Name = (string)reader["Name"],
                Producer = (string)reader["Producer"],
                Price = (double)reader["Price"]
            };
        }
    }
}
