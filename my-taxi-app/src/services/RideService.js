const RideService = {
    requestRide: async (pickupLocation, dropoffLocation) => {
        const response = await fetch('/api/rides', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('jwtToken')}`,
            },
            body: JSON.stringify({ pickupLocation, dropoffLocation }),
        });

        if (!response.ok) {
            throw new Error('Failed to request ride');
        }

        return await response.json();
    },

    getRideHistory: async () => {
        const response = await fetch('/api/rides/history', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`,
            },
        });

        if (!response.ok) {
            throw new Error('Failed to fetch ride history');
        }

        return await response.json();
    },
};

export default RideService;
