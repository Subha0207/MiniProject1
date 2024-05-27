using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FlightManagementSystemAPI.Repositories
{
    public class PaymentRepository:IRepository<int,Payment>
    {
        private readonly FlightManagementContext _context;

        public PaymentRepository(FlightManagementContext context)
        {
            _context = context;
        }

        public async Task<Payment> Add(Payment item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }

            catch (Exception ex)
            {
                throw new PaymentException("Error while adding Payment", ex);
            }
        }

        public async  Task<Payment> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> Get(int key)
        {
            try
            {

                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == key);
                if (payment == null)
                    throw new PaymentNotFoundException("No such payment is present.");
                return payment;
            }
            catch (PaymentNotFoundException ex)
            {
                throw new PaymentException("Error while getting payment" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new PaymentException("Error while getting paymnet", ex);
            }
        }

        public async  Task<IEnumerable<Payment>> GetAll()
        {
            try
            {

                var payments = await _context.Payments.ToListAsync();
                if (payments.Count<0)
                    throw new PaymentNotFoundException("No such payment is present.");
                return payments;
            }
            catch (PaymentNotFoundException ex)
            {
                throw new PaymentException("Error while getting payment" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new PaymentException("Error while getting paymnet", ex);
            }
        }

        public async Task<Payment> Update(Payment item)
        {
            try
            {
                var payment = await Get(item.PaymentId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (PaymentNotFoundException ex)
            {
                throw new PaymentException("Error occurred while updating flights. " + ex.Message, ex);
            }
            catch (Exception ex)
            {

                throw new PaymentException("Error occurred while updating flights. " + ex.Message, ex);
            }
        }
    }
}
