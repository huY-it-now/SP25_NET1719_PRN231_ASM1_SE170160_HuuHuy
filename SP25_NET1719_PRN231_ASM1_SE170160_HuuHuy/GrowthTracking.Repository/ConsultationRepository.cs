using GrowthTracking.Repository.Base;
using GrowthTracking.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace GrowthTracking.Repository
{
    public class ConsultationRepository : GenericRepository<Consultation>
    {
        public ConsultationRepository()
        {
            
        }

        public async Task<List<Consultation>> GetAll()
        {
            var Consultation = await _context.Consultations.Include(u => u.User).ToListAsync();
            return Consultation;
        }
        public async Task<Consultation> GetByIdAsync(Guid id)
        {
            var consultation = await _context.Consultations.Include(u => u.User).FirstOrDefaultAsync(b => b.Id == id);
            return consultation;
        }
        public async Task<List<Consultation>> Search(Guid consultationId, Guid userId)
        {
            var consultations = await _context.Consultations
                .Include(u => u.User)
                .Where(x => x.Id == consultationId && x.UserId == userId)
                .ToListAsync();

            return consultations;
        }
        public async Task<Consultation> CreateAsync(Consultation consultation)
        {
            var entityEntry = await _context.Consultations.AddAsync(consultation);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }
    }
}
