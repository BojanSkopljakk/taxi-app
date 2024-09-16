using DriverVerificationService.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data;

namespace DriverVerificationService.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly IReliableStateManager _stateManager;

        public DriverRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<Driver> GetDriverAsync(string driverId)
        {
            var drivers = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Driver>>("drivers");
            using (var tx = _stateManager.CreateTransaction())
            {
                var driver = await drivers.TryGetValueAsync(tx, driverId);
                return driver.HasValue ? driver.Value : null;
            }
        }

        public async Task CreateOrUpdateDriverAsync(Driver driver)
        {
            var drivers = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Driver>>("drivers");
            using (var tx = _stateManager.CreateTransaction())
            {
                await drivers.SetAsync(tx, driver.DriverId, driver);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Driver>> GetPendingVerificationsAsync()
        {
            var drivers = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Driver>>("drivers");
            var result = new List<Driver>();
            using (var tx = _stateManager.CreateTransaction())
            {
                var enumerator = (await drivers.CreateEnumerableAsync(tx)).GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    if (enumerator.Current.Value.VerificationStatus == VerificationStatus.Pending)
                    {
                        result.Add(enumerator.Current.Value);
                    }
                }
            }
            return result;
        }
    }

}
