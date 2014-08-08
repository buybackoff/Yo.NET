using System.Threading.Tasks;
using Ractor;

// TODO move to Actors assembly
namespace Yo.Services
{

    public class YoCounter : Actor<string, long> {
        public override async Task<long> Computation(string username) {
            var userCount = await Redis.GetAsync<long>("yo:counter:" + (username ?? "_total_") );
            return userCount;
        }
    }

    public class YoIncrementer : Actor<string, long> {
        public override async Task<long> Computation(string username) {
            var userCount = await Redis.IncrAsync("yo:counter:" + (username ?? "_total_"));
            return userCount;
        }
    }

}
