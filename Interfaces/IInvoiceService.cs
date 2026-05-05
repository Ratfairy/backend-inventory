using backend_inventory.DTOs.Invoice;

namespace backend_inventory.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync();
    Task<InvoiceResponseDto?> GetInvoiceByIdAsync(int id);
    Task<IEnumerable<InvoiceResponseDto>> GetReceivedPOsForInvoiceAsync();
    Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto);
    Task<InvoiceResponseDto?> UpdateStatusAsync(int id, UpdateInvoiceStatusDto dto);
    Task<bool> DeleteInvoiceAsync(int id);
}