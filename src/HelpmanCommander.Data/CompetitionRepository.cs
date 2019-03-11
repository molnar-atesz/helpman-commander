﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HelpmanCommander.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HelpmanCommander.Data
{
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompetitionRepository> _logger;

        public CompetitionRepository(ApplicationDbContext context, ILogger<CompetitionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Competition[]> GetAllCompetitionAsync(bool includeExercises = false)
        {
            _logger.LogInformation($"Getting all competition");

            IQueryable<Competition> query = _context.Competitions.Include(c => c.Stations);

            if (includeExercises)
            {
                query.Include(c => c.Stations).ThenInclude(s => s.Exercises);
            }

            query = query.OrderByDescending(c => c.DateOfEvent);

            return await query.ToArrayAsync();
        }

        public Task<Competition> GetCompetitionAsync(int id, bool includeExercises = false)
        {
            throw new NotImplementedException();
        }
    }
}