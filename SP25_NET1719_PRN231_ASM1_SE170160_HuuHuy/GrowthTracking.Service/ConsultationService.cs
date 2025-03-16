using GrowthTracking.Repository;
using GrowthTracking.Repository.Models;

namespace GrowthTracking.Service
{
    public interface IConsultationService
    {
        Task<List<Consultation>> GetAll();
        Task<Consultation> GetById(Guid id);
        Task<Guid> Create(Consultation consultation);
        Task<int> Update(Consultation consultation);
        Task<bool> Delete(Guid id);
        Task<List<Consultation>> Search(Guid consultationId, Guid userId);
    }

    public class ConsultationService : IConsultationService
    {
        private readonly ConsultationRepository _consultationRepository;

        public ConsultationService()
        {
            _consultationRepository = new ConsultationRepository();
        }

        public async Task<Guid> Create(Consultation consultation)
        {
            if (consultation == null)
            {
                throw new ArgumentNullException(nameof(consultation));
            }

            var newConsultation = new Consultation
            {
                Id = Guid.NewGuid(),
                Day = consultation.Day,
                Time = consultation.Time,
                UserId = consultation.UserId,
                CreationDate = DateTime.Now,
                CreatedBy = consultation.CreatedBy,
                IsDeleted = false
            };

            if (consultation.DoctorId == null || consultation.DoctorId == Guid.Empty)
            {
                if (consultation.User == null)
                {
                    throw new ArgumentNullException(nameof(consultation.User));
                }

                var newDoctor = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = consultation.User.FullName,
                    Email = consultation.User.Email,
                    Password = consultation.User.Password,
                    CreationDate = DateTime.Now,
                    IsDeleted = false
                };

                newConsultation.User = newDoctor;
            }

            await _consultationRepository.CreateAsync(newConsultation);

            return newConsultation.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var consultation = await _consultationRepository.GetByIdAsync(id);

            if (consultation != null)
            {
                return await _consultationRepository.RemoveAsync(consultation);
            }

            return false;
        }

        public async Task<List<Consultation>> GetAll()
        {
            return await _consultationRepository.GetAll();
        }

        public async Task<Consultation> GetById(Guid id)
        {
            return await _consultationRepository.GetByIdAsync(id);
        }

        public async Task<List<Consultation>> Search(Guid consultationId, Guid userId)
        {
           return await _consultationRepository.Search(consultationId, userId);
        }

        public Task<int> Update(Consultation consultation)
        {
            return _consultationRepository.UpdateAsync(consultation);
        }
    }
}
