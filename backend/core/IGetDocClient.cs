using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace core
{
    public interface IGetDocClient
    {
        Task<DocumentClient> GetDocumentClient();
    }
}