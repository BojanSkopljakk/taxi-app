using DriveService.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data;

namespace DriveService.Repositories
{
    public class RideRepository : IRideRepository
    {
        private readonly IReliableStateManager _stateManager;

        public RideRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<Ride> CreateRideAsync(Ride ride)
        {
            var rides = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Ride>>("rides");
            using (var tx = _stateManager.CreateTransaction())
            {
                await rides.AddAsync(tx, ride.RideId, ride);
                await tx.CommitAsync();
                return ride;
            }
        }

        public async Task<Ride> GetRideAsync(Guid rideId)
        {
            var rides = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Ride>>("rides");
            using (var tx = _stateManager.CreateTransaction())
            {
                var ride = await rides.TryGetValueAsync(tx, rideId);
                return ride.HasValue ? ride.Value : null;
            }
        }

        public async Task UpdateRideAsync(Ride ride)
        {
            var rides = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Ride>>("rides");
            using (var tx = _stateManager.CreateTransaction())
            {
                await rides.SetAsync(tx, ride.RideId, ride);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Ride>> GetPendingRidesAsync()
        {
            var rides = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Ride>>("rides");
            var result = new List<Ride>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var enumerator = (await rides.CreateEnumerableAsync(tx)).GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    if (enumerator.Current.Value.Status == RideStatus.Pending)
                    {
                        result.Add(enumerator.Current.Value);
                    }
                }
            }

            return result;
        }
    }

}
