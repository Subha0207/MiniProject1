﻿using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.PaymentExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Repositories
{
    public class PaymentRepository : IRepository<int, Payment>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(FlightManagementContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Payment> Add(Payment item)
        {
            try
            {
                _logger.LogInformation("Adding new payment.");
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Payment added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding payment.");
                throw new PaymentException("Error while adding payment", ex);
            }
        }

        public async Task<Payment> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting payment with key: {key}");
                var payment = await Get(key);
                _context.Remove(payment);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Payment deleted successfully.");
                return payment;
            }
            catch (PaymentNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting payment: Payment not found.");
                throw new PaymentException("Error occurred while deleting payment. Payment not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting payment.");
                throw new PaymentException("Error occurred while deleting payment.", ex);
            }
        }

        public async Task<Payment> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting payment with PaymentId: {key}");
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == key);
                if (payment == null)
                {
                    _logger.LogWarning("No payment found with the given key.");
                    throw new PaymentNotFoundException("No such payment is present.");
                }
                _logger.LogInformation("Payment details fetched successfully.");
                return payment;
            }
            catch (PaymentNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting payment: " + ex.Message);
                throw new PaymentException("Error while getting payment: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting payment.");
                throw new PaymentException("Error while getting payment", ex);
            }
        }

        public async Task<IEnumerable<Payment>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all payments.");
                var payments = await _context.Payments.ToListAsync();
                if (payments.Count <= 0)
                {
                    _logger.LogWarning("No payments found.");
                    throw new PaymentNotFoundException("No payment is present.");
                }
                _logger.LogInformation("Payments fetched successfully.");
                return payments;
            }
            catch (PaymentNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting payments: " + ex.Message);
                throw new PaymentException("Error while getting payments: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting payments.");
                throw new PaymentException("Error while getting payments", ex);
            }
        }

        public async Task<Payment> Update(Payment item)
        {
            try
            {
                _logger.LogInformation($"Updating payment with PaymentId: {item.PaymentId}");
                var payment = await Get(item.PaymentId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Payment updated successfully.");
                return payment;
            }
            catch (PaymentNotFoundException ex)
            {
                _logger.LogError(ex, "Error while updating payment: " + ex.Message);
                throw new PaymentException("Error while updating payment: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating payment.");
                throw new PaymentException("Error while updating payment", ex);
            }
        }
    }
}